# 工具与环境规则

适用环境：Windows 原生（开发、调试、构建、运行）；WSL（agent 运行环境，仓库位于 `/mnt/<盘符>/...`）。

## 通用规则

1. 默认使用当前运行环境的原生工具（git、rg、fd、dotnet、pwsh 等），不得假设某个环境特有的工具、别名、shell 函数或全局配置存在。
2. 写入仓库的脚本与文档命令须在 Windows 与 WSL 下均可运行（优先使用 pwsh、dotnet）；做不到时必须标注适用环境。
3. 文档中的仓库路径统一使用 `/` 分隔的相对路径；代码中用语言自带的路径 API 拼接，不手写分隔符。
4. 换行与编码以 `.editorconfig`、`.gitattributes` 为准，不依赖本机 git 的换行转换配置（`core.autocrlf`）：
    - 默认：CRLF + UTF-8 with BOM。
    - `.editorconfig`、`.gitattributes`、`.gitignore`、`.gitmodules`：CRLF + UTF-8 无 BOM。
    - `*.sh`：LF + UTF-8 无 BOM。
5. 环境不确定时先探测再执行，例如 `uname`、`pwd -P`、`df -T .`，不要凭路径形状推断。
6. 工具缺失或行为不同时，不得为了适配本机而修改项目文件。
7. 自动化代理新建或整体重写文件后（WSL 下的工具默认写出 LF + 无 BOM），必须运行编码与换行检查脚本，不合规时用对应的修复脚本修正（见"仓库脚本"）。

## WSL 规则

1. 访问 Windows 盘上的仓库时，Linux 原生工具可能明显变慢（常见于 `git status`、`git log`、全仓库搜索）；无明显问题时以好用为准，明显变慢时改用 Windows 版工具。
2. `win-*` 是开发者个人的 WSL 包装脚本（位于 `~/.local/bin`），不属于仓库：`win-git`、`win-dotnet`、`win-pwsh`、`win-rg`、`win-fd`、`win-jq`、`win-yq`。
3. `win-*` 不得写入仓库脚本或项目文档中的标准命令；不存在时直接使用原生工具。
4. 脚本或子进程不得依赖 shell 函数或 alias；需要时调用包装脚本的完整路径。
5. drvfs 挂载下，GNU `sed -i` 可能因无法保留文件权限而失败（`preserving permissions ... Operation not permitted`），改用编辑工具或 pwsh 修改文件。

### 使用 Windows 版工具时的差异

1. 参数中的 Linux 绝对路径（如 `/tmp/x`）不会被转换，使用相对路径，或先用 `wslpath -w` 转换。
2. `rg`、`fd` 的 Windows 版输出可能含反斜杠路径，传给 Linux 工具前先规范化。
3. 环境变量默认不会传给 Windows 程序，需要在 `WSLENV` 中声明。`/w` 表示仅在 WSL 调用 Windows 程序时传递，`/u` 表示仅在 Windows 调用 WSL 程序时传递，`/p` 表示值是路径。
4. `win-git` 已转发 `GIT_AUTHOR_NAME`、`GIT_AUTHOR_EMAIL`、`GIT_AUTHOR_DATE`、`GIT_COMMITTER_NAME`、`GIT_COMMITTER_EMAIL`、`GIT_COMMITTER_DATE`。未转发的变量会被静默忽略，作者与日期会回落为当前值而不报错；批量创建或改写提交前，必须先用 `git var GIT_AUTHOR_IDENT` 自检。

## 仓库脚本

1. 编码与换行的检查、修复使用 `tools/` 下的脚本，需要 PowerShell 7+（`pwsh`），Windows 与 WSL 均可运行：

    ```powershell
    pwsh -NoProfile -File tools/check-encoding.ps1 [<路径>...]
    pwsh -NoProfile -File tools/check-line-ending.ps1 [<路径>...]
    pwsh -NoProfile -File tools/fix-encoding.ps1 [<路径>...] [-DryRun] [-Backup]
    pwsh -NoProfile -File tools/fix-line-ending.ps1 [<路径>...] [-DryRun]
    ```

2. 路径以位置参数传入，可传多个目录或文件（`pwsh -Command` 方式下也可用 `-Root a,b`），默认为仓库根目录；扫描时跳过 `artifacts`、`bin`、`obj`、`node_modules`、`TestResults`、`references`、`.git`、`.vs` 目录。
3. 文件范围与编码 / 换行规则集中定义在 `tools/common.ps1`，修改时必须与 `.editorconfig`、`.gitattributes` 保持一致。
4. 先修编码再修换行：`fix-line-ending.ps1` 会跳过 UTF-16 文件。
5. 对已有代码批量执行 `fix-*` 属于大范围写入，执行前必须限定路径范围或先用 `-DryRun` 确认。
6. 输出文本通过 `Import-LocalizedData` 本地化：默认（英文）文本为 `tools/Messages.psd1`，翻译放在 `tools/<区域性>/Messages.psd1`（目前为 `zh-Hans`）；按 UI 语言逐级回退（如 `zh-CN` → `zh-Hans`），无对应翻译或翻译缺键时使用默认文本。不要新建 `tools/en-US/` 目录，否则默认文本会改从该目录加载。WSL 下 UI 语言由 `LANG` 决定。
7. 新增或修改输出文本时，必须同时更新默认文本与所有翻译文件，键名保持一致。

## 构建与运行

1. 本项目只能在 Windows 上构建（WinUI 3 的 XAML 编译与 MSIX 打包依赖 Windows）。在 WSL 中构建时使用 `win-dotnet`；不得用 Linux 版 `dotnet` 还原或构建本项目，两者的 SDK 版本及 `obj/`、`project.assets.json` 路径格式互不兼容。
2. 自动化代理执行构建时加 `-nodeReuse:false`，避免残留的 MSBuild 节点进程锁定文件。
3. `bin/`、`obj/`、`artifacts/` 不得提交。
