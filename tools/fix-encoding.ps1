# 修复编码：默认转为 UTF-8 with BOM，例外见 common.ps1
# 用法: pwsh -NoProfile -File tools/fix-encoding.ps1 [<path>...] [-DryRun] [-Backup]
#   UTF-8    -> 字节级补上 / 去掉 BOM，不做编解码
#   UTF-16   -> .NET 解码后转换
#   GBK/ANSI -> 按 GBK(936) 解码，往返验证通过才转换
#   -Backup 修改前生成 .bak；-DryRun 只列出不写入

param(
    [Parameter(Position = 0, ValueFromRemainingArguments = $true)]
    [string[]]$Root = @((Split-Path $PSScriptRoot -Parent)),
    [switch]$DryRun,
    [switch]$Backup
)

. (Join-Path $PSScriptRoot 'common.ps1')

$converted = 0
$skipped   = 0
$failed    = 0

function Write-TextFile {
    param(
        [System.IO.FileInfo]$File,
        [string]$Text,
        [bool]$BomRequired
    )

    if ($DryRun) {
        return
    }
    if ($Backup) {
        Copy-Item -LiteralPath $File.FullName "$($File.FullName).bak"
    }
    $encoding = New-Object System.Text.UTF8Encoding($BomRequired)
    [System.IO.File]::WriteAllText($File.FullName, $Text, $encoding)
}

function Write-ByteFile {
    param(
        [System.IO.FileInfo]$File,
        [byte[]]$Bytes
    )

    if ($DryRun) {
        return
    }
    if ($Backup) {
        Copy-Item -LiteralPath $File.FullName "$($File.FullName).bak"
    }
    [System.IO.File]::WriteAllBytes($File.FullName, $Bytes)
}

foreach ($file in Get-TextFile $Root) {
    $bytes = [System.IO.File]::ReadAllBytes($file.FullName)

    if ($bytes.Length -eq 0) {
        $skipped++
        continue
    }

    $bomRequired = Test-BomRequired $file.Name

    if (Test-Utf16Bom $bytes) {
        $isLittleEndian = $bytes[0] -eq 0xFF
        $sourceEncoding = if ($isLittleEndian) { [System.Text.Encoding]::Unicode } else { [System.Text.Encoding]::BigEndianUnicode }
        $text = $sourceEncoding.GetString($bytes, 2, $bytes.Length - 2)
        Write-Host "UTF-16     -> UTF-8: $($file.FullName)" -ForegroundColor Yellow
        Write-TextFile $file $text $bomRequired
        $converted++
        continue
    }

    if (Test-ValidUtf8 $bytes) {
        $hasBom = Test-Utf8Bom $bytes
        if ($bomRequired -and -not $hasBom) {
            Write-Host "UTF-8      -> UTF-8 BOM: $($file.FullName)" -ForegroundColor Cyan
            $newBytes = New-Object byte[] ($bytes.Length + 3)
            [System.Array]::Copy($Utf8Bom, 0, $newBytes, 0, 3)
            [System.Array]::Copy($bytes, 0, $newBytes, 3, $bytes.Length)
            Write-ByteFile $file $newBytes
            $converted++
        }
        elseif (-not $bomRequired -and $hasBom) {
            Write-Host "UTF-8 BOM  -> UTF-8: $($file.FullName)" -ForegroundColor Cyan
            $newBytes = New-Object byte[] ($bytes.Length - 3)
            [System.Array]::Copy($bytes, 3, $newBytes, 0, $bytes.Length - 3)
            Write-ByteFile $file $newBytes
            $converted++
        }
        else {
            $skipped++
        }
        continue
    }

    # 往返验证：GBK -> string -> GBK 字节须与原字节一致
    try {
        $gbk = [System.Text.Encoding]::GetEncoding(936)
        $text = $gbk.GetString($bytes)
        $roundTrip = $gbk.GetBytes($text)

        $roundTripOk = ($roundTrip.Length -eq $bytes.Length)
        if ($roundTripOk) {
            for ($i = 0; $i -lt $bytes.Length; $i++) {
                if ($bytes[$i] -ne $roundTrip[$i]) {
                    $roundTripOk = $false
                    break
                }
            }
        }

        if (-not $roundTripOk) {
            Write-Warning ($Messages.EncodingGbkRoundTripFailed -f $file.FullName)
            $failed++
            continue
        }

        Write-Host "GBK/ANSI   -> UTF-8: $($file.FullName)" -ForegroundColor Yellow
        Write-TextFile $file $text $bomRequired
        $converted++
    }
    catch {
        Write-Warning ($Messages.EncodingCannotConvert -f $file.FullName, $_)
        $failed++
    }
}

Write-Host ""
if ($DryRun) { Write-Host "[DryRun] " -ForegroundColor Magenta -NoNewline }
Write-Host ($Messages.FixSummary -f $converted, $skipped) -ForegroundColor Cyan
if ($failed -gt 0) { Write-Host ($Messages.FixFailed -f $failed) -ForegroundColor Red }
