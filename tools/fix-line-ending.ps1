# 修复换行符：默认转为 CRLF，例外见 common.ps1
# 用法: pwsh -NoProfile -File tools/fix-line-ending.ps1 [<path>...] [-DryRun]
#   只处理 CRLF 与单独的 LF；单独的 CR 保留并警告；UTF-16 文件跳过（先修复编码）

param(
    [Parameter(Position = 0, ValueFromRemainingArguments = $true)]
    [string[]]$Root = @((Split-Path $PSScriptRoot -Parent)),
    [switch]$DryRun
)

. (Join-Path $PSScriptRoot 'common.ps1')

$converted = 0
$skipped   = 0

foreach ($file in Get-TextFile $Root) {
    $bytes = [System.IO.File]::ReadAllBytes($file.FullName)

    if ($bytes.Length -eq 0) {
        $skipped++
        continue
    }

    if (Test-Utf16Bom $bytes) {
        Write-Warning ($Messages.LineEndingUtf16Skipped -f $file.FullName)
        $skipped++
        continue
    }

    $useCrlf = (Get-ExpectedLineEnding $file.Name) -eq 'CRLF'
    $hasLoneCr = $false
    $changed = $false

    $result = [System.Collections.Generic.List[byte]]::new($bytes.Length + ($bytes.Length -shr 4))
    for ($i = 0; $i -lt $bytes.Length; $i++) {
        $isCrlf = $bytes[$i] -eq 0x0D -and ($i + 1) -lt $bytes.Length -and $bytes[$i + 1] -eq 0x0A
        if ($isCrlf -or $bytes[$i] -eq 0x0A) {
            if ($useCrlf) {
                $result.Add(0x0D)
            }
            $result.Add(0x0A)
            if ($isCrlf) {
                $i++
                $changed = $changed -or -not $useCrlf
            }
            else {
                $changed = $changed -or $useCrlf
            }
            continue
        }
        if ($bytes[$i] -eq 0x0D) {
            $hasLoneCr = $true
        }
        $result.Add($bytes[$i])
    }

    if ($hasLoneCr) {
        Write-Warning ($Messages.LineEndingLoneCr -f $file.FullName)
    }

    if (-not $changed) {
        $skipped++
        continue
    }

    $target = if ($useCrlf) { 'CRLF' } else { 'LF' }
    Write-Host "-> ${target}: $($file.FullName)"
    if (-not $DryRun) {
        [System.IO.File]::WriteAllBytes($file.FullName, $result.ToArray())
    }
    $converted++
}

Write-Host ""
if ($DryRun) { Write-Host "[DryRun] " -ForegroundColor Magenta -NoNewline }
Write-Host ($Messages.FixSummary -f $converted, $skipped) -ForegroundColor Cyan
