# 检查换行符：默认 CRLF，例外见 common.ps1
# 用法: pwsh -NoProfile -File tools/check-line-ending.ps1 [<path>...]

param(
    [Parameter(Position = 0, ValueFromRemainingArguments = $true)]
    [string[]]$Root = @((Split-Path $PSScriptRoot -Parent))
)

. (Join-Path $PSScriptRoot 'common.ps1')

$wrongFiles = [System.Collections.Generic.List[string]]::new()
$mixedFiles = [System.Collections.Generic.List[string]]::new()
$crFiles    = [System.Collections.Generic.List[string]]::new()
$utf16Files = [System.Collections.Generic.List[string]]::new()
$okCount    = 0
$emptyCount = 0

foreach ($file in Get-TextFile $Root) {
    $bytes = [System.IO.File]::ReadAllBytes($file.FullName)

    if ($bytes.Length -eq 0) {
        $emptyCount++
        continue
    }

    # UTF-16 无法按字节判断换行，需先修复编码
    if (Test-Utf16Bom $bytes) {
        $utf16Files.Add($file.FullName)
        continue
    }

    $hasCrlf = $false
    $hasLoneCr = $false
    $hasLoneLf = $false

    for ($i = 0; $i -lt $bytes.Length; $i++) {
        if ($bytes[$i] -eq 0x0D) {
            if (($i + 1) -lt $bytes.Length -and $bytes[$i + 1] -eq 0x0A) {
                $hasCrlf = $true
                $i++
            }
            else {
                $hasLoneCr = $true
            }
        }
        elseif ($bytes[$i] -eq 0x0A) {
            $hasLoneLf = $true
        }
    }

    $expected = Get-ExpectedLineEnding $file.Name
    $entry = $Messages.LineEndingExpected -f $file.FullName, $expected

    if ($hasLoneCr) {
        $crFiles.Add($entry)
    }
    elseif ($hasCrlf -and $hasLoneLf) {
        $mixedFiles.Add($entry)
    }
    elseif (($expected -eq 'CRLF' -and $hasLoneLf) -or ($expected -eq 'LF' -and $hasCrlf)) {
        $wrongFiles.Add($entry)
    }
    else {
        # 符合期望，或无换行的单行文件
        $okCount++
    }
}

$totalBad = $wrongFiles.Count + $mixedFiles.Count + $crFiles.Count + $utf16Files.Count
$emptyText = $Messages.EmptyFilesSkipped -f $emptyCount

if ($totalBad -eq 0) {
    Write-Host ($Messages.LineEndingAllOk -f $okCount) -ForegroundColor Green
    if ($emptyCount -gt 0) { Write-Host "     ($emptyText)" -ForegroundColor DarkGray }
    exit 0
}

if ($wrongFiles.Count -gt 0) {
    Write-Host ($Messages.LineEndingWrong -f $wrongFiles.Count) -ForegroundColor Yellow
    $wrongFiles | ForEach-Object { Write-Host "  $_" }
}

if ($mixedFiles.Count -gt 0) {
    Write-Host "=== Mixed (CRLF + LF) ($($mixedFiles.Count)) ===" -ForegroundColor Red
    $mixedFiles | ForEach-Object { Write-Host "  $_" }
}

if ($crFiles.Count -gt 0) {
    Write-Host "=== CR ($($crFiles.Count)) ===" -ForegroundColor Red
    $crFiles | ForEach-Object { Write-Host "  $_" }
}

if ($utf16Files.Count -gt 0) {
    Write-Host ($Messages.LineEndingUtf16 -f $utf16Files.Count) -ForegroundColor Red
    $utf16Files | ForEach-Object { Write-Host "  $_" }
}

Write-Host ""
Write-Host ($Messages.CheckSummary -f $okCount, $totalBad) -ForegroundColor Cyan
if ($emptyCount -gt 0) { Write-Host $emptyText -ForegroundColor DarkGray }
exit 1
