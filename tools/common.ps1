# tools 脚本公共部分：文件范围、编码 / 换行规则（须与 .editorconfig、.gitattributes 一致）、本地化文本

# 先加载默认（英文）文本 Messages.psd1，再按 UI 语言加载 <区域性>/Messages.psd1 逐键覆盖，缺失的键保留默认值
$Messages = Import-LocalizedData -BaseDirectory $PSScriptRoot -FileName 'Messages.psd1' -UICulture 'en-US'
Import-LocalizedData -BindingVariable localizedMessages -BaseDirectory $PSScriptRoot -FileName 'Messages.psd1' -ErrorAction SilentlyContinue
if ($localizedMessages) {
    foreach ($key in $localizedMessages.Keys) {
        $Messages[$key] = $localizedMessages[$key]
    }
}

$TextFilePatterns = @(
    # C# / F#
    '*.cs', '*.fs', '*.fsi', '*.fsx', '*.razor',
    # 项目 / 解决方案
    '*.csproj', '*.fsproj', '*.sln', '*.slnx',
    '*.props', '*.targets', '*.nuspec', '*.resx',
    # XAML / 应用清单 / 发布配置
    '*.xaml', '*.appxmanifest', '*.manifest', '*.pubxml',
    # 前端
    '*.js', '*.ts', '*.jsx', '*.tsx', '*.vue', '*.svelte',
    '*.css', '*.scss', '*.sass', '*.less', '*.html', '*.htm',
    # 数据 / 配置
    '*.json', '*.xml', '*.yaml', '*.yml', '*.toml', '*.ini', '*.env', '*.config', '*.svg',
    # 文档
    '*.md', '*.mdc', '*.txt', '*.rst',
    # 脚本
    '*.ps1', '*.psm1', '*.psd1', '*.sh', '*.bat', '*.cmd',
    # 容器 / CI
    '*.dockerignore',
    # Git / 编辑器
    '*.editorconfig', '*.globalconfig', '*.gitattributes', '*.gitignore', '*.gitmodules',
    # 其他
    '*.sql', '*.graphql', '*.proto'
)

$ExcludedDirectoryNames = @('artifacts', 'bin', 'obj', 'node_modules', 'TestResults', 'references', '.git', '.vs')

# 不带 BOM 的例外，其余要求 UTF-8 with BOM
$NoBomPatterns = @('.editorconfig', '.gitattributes', '.gitignore', '.gitmodules', '*.sh')

# 使用 LF 的例外，其余要求 CRLF
$LfPatterns = @('*.sh')

$Utf8Bom = [byte[]](0xEF, 0xBB, 0xBF)

function Test-NameMatch {
    param(
        [string]$Name,
        [string[]]$Patterns
    )

    foreach ($pattern in $Patterns) {
        if ($Name -like $pattern) {
            return $true
        }
    }
    return $false
}

function Test-BomRequired {
    param([string]$Name)

    $required = -not (Test-NameMatch $Name $NoBomPatterns)
    return $required
}

function Get-ExpectedLineEnding {
    param([string]$Name)

    if (Test-NameMatch $Name $LfPatterns) {
        return 'LF'
    }
    return 'CRLF'
}

function Get-TextFileFromDirectory {
    param([System.IO.DirectoryInfo]$Directory)

    foreach ($child in $Directory.EnumerateFileSystemInfos()) {
        if ($child.Attributes -band [System.IO.FileAttributes]::ReparsePoint) {
            continue
        }

        if ($child -is [System.IO.DirectoryInfo]) {
            if ($ExcludedDirectoryNames -notcontains $child.Name) {
                Get-TextFileFromDirectory $child
            }
        }
        elseif (Test-NameMatch $child.Name $TextFilePatterns) {
            $child
        }
    }
}

# 目录递归扫描（跳过排除目录），文件按扩展名过滤
function Get-TextFile {
    param([string[]]$Path)

    foreach ($entry in $Path) {
        $item = Get-Item -LiteralPath $entry -Force -ErrorAction Stop
        if ($item.PSIsContainer) {
            Get-TextFileFromDirectory $item
        }
        elseif (Test-NameMatch $item.Name $TextFilePatterns) {
            $item
        }
    }
}

function Test-Utf16Bom {
    param([byte[]]$Bytes)

    $isUtf16 = $Bytes.Length -ge 2 -and
        (($Bytes[0] -eq 0xFF -and $Bytes[1] -eq 0xFE) -or
         ($Bytes[0] -eq 0xFE -and $Bytes[1] -eq 0xFF))
    return $isUtf16
}

function Test-Utf8Bom {
    param([byte[]]$Bytes)

    $hasBom = $Bytes.Length -ge 3 -and
        $Bytes[0] -eq 0xEF -and $Bytes[1] -eq 0xBB -and $Bytes[2] -eq 0xBF
    return $hasBom
}

# UTF-8 往返编解码：非法字节会解码为 U+FFFD，重新编码后与原字节不一致
function Test-ValidUtf8 {
    param([byte[]]$Bytes)

    $utf8NoBom = New-Object System.Text.UTF8Encoding($false)
    $decoded   = $utf8NoBom.GetString($Bytes)
    $reEncoded = $utf8NoBom.GetBytes($decoded)

    if ($reEncoded.Length -ne $Bytes.Length) {
        return $false
    }
    for ($i = 0; $i -lt $Bytes.Length; $i++) {
        if ($Bytes[$i] -ne $reEncoded[$i]) {
            return $false
        }
    }
    return $true
}
