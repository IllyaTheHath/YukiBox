# 检查编码：默认 UTF-8 with BOM，例外见 common.ps1
# 用法: pwsh -NoProfile -File tools/check-encoding.ps1 [<path>...]

param(
    [Parameter(Position = 0, ValueFromRemainingArguments = $true)]
    [string[]]$Root = @((Split-Path $PSScriptRoot -Parent))
)

. (Join-Path $PSScriptRoot 'common.ps1')

$utf16Files         = [System.Collections.Generic.List[string]]::new()
$ansiFiles          = [System.Collections.Generic.List[string]]::new()
$missingBomFiles    = [System.Collections.Generic.List[string]]::new()
$unexpectedBomFiles = [System.Collections.Generic.List[string]]::new()
$okCount            = 0
$emptyCount         = 0

foreach ($file in Get-TextFile $Root) {
    $bytes = [System.IO.File]::ReadAllBytes($file.FullName)

    if ($bytes.Length -eq 0) {
        $emptyCount++
        continue
    }

    if (Test-Utf16Bom $bytes) {
        $utf16Files.Add($file.FullName)
        continue
    }

    if (-not (Test-ValidUtf8 $bytes)) {
        $ansiFiles.Add($file.FullName)
        continue
    }

    $hasBom = Test-Utf8Bom $bytes
    $bomRequired = Test-BomRequired $file.Name
    if ($bomRequired -and -not $hasBom) {
        $missingBomFiles.Add($file.FullName)
    }
    elseif (-not $bomRequired -and $hasBom) {
        $unexpectedBomFiles.Add($file.FullName)
    }
    else {
        $okCount++
    }
}

$totalBad = $utf16Files.Count + $ansiFiles.Count + $missingBomFiles.Count + $unexpectedBomFiles.Count
$emptyText = $Messages.EmptyFilesSkipped -f $emptyCount

if ($totalBad -eq 0) {
    Write-Host ($Messages.EncodingAllOk -f $okCount) -ForegroundColor Green
    if ($emptyCount -gt 0) { Write-Host "     ($emptyText)" -ForegroundColor DarkGray }
    exit 0
}

if ($utf16Files.Count -gt 0) {
    Write-Host "=== UTF-16 ($($utf16Files.Count)) ===" -ForegroundColor Red
    $utf16Files | ForEach-Object { Write-Host "  $_" }
}

if ($ansiFiles.Count -gt 0) {
    Write-Host "=== GBK / ANSI ($($ansiFiles.Count)) ===" -ForegroundColor Red
    $ansiFiles | ForEach-Object { Write-Host "  $_" }
}

if ($missingBomFiles.Count -gt 0) {
    Write-Host ($Messages.EncodingMissingBom -f $missingBomFiles.Count) -ForegroundColor Yellow
    $missingBomFiles | ForEach-Object { Write-Host "  $_" }
}

if ($unexpectedBomFiles.Count -gt 0) {
    Write-Host ($Messages.EncodingUnexpectedBom -f $unexpectedBomFiles.Count) -ForegroundColor Yellow
    $unexpectedBomFiles | ForEach-Object { Write-Host "  $_" }
}

Write-Host ""
Write-Host ($Messages.CheckSummary -f $okCount, $totalBad) -ForegroundColor Cyan
if ($emptyCount -gt 0) { Write-Host $emptyText -ForegroundColor DarkGray }
exit 1
