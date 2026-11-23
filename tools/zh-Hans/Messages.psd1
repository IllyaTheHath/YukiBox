# 简体中文文本，键名与 tools/Messages.psd1 保持一致
ConvertFrom-StringData @'
EmptyFilesSkipped          = {0} 个空文件已跳过
CheckSummary               = 符合规范: {0} | 异常: {1}
FixSummary                 = 已转换: {0} | 无需处理: {1}
FixFailed                  = 失败: {0}
EncodingAllOk              = [OK] 全部 {0} 个文件编码符合规范。
EncodingMissingBom         = === UTF-8 缺少 BOM ({0}) ===
EncodingUnexpectedBom      = === UTF-8 不应带 BOM ({0}) ===
EncodingGbkRoundTripFailed = [SKIP] GBK 往返验证失败，跳过: {0}
EncodingCannotConvert      = [SKIP] 无法转换: {0} ({1})
LineEndingAllOk            = [OK] 全部 {0} 个文件换行符符合规范。
LineEndingExpected         = {0} (期望 {1})
LineEndingWrong            = === 换行符不符 ({0}) ===
LineEndingUtf16            = === UTF-16，需先修复编码 ({0}) ===
LineEndingUtf16Skipped     = [SKIP] UTF-16 文件，请先运行 fix-encoding.ps1: {0}
LineEndingLoneCr           = 包含单独的 CR，未处理: {0}
'@
