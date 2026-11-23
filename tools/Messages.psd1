# 默认（英文）文本，各语言翻译放在 <区域性>/Messages.psd1，键名保持一致
ConvertFrom-StringData @'
EmptyFilesSkipped          = {0} empty file(s) skipped
CheckSummary               = Compliant: {0} | Issues: {1}
FixSummary                 = Converted: {0} | Already OK: {1}
FixFailed                  = Failed: {0}
EncodingAllOk              = [OK] All {0} file(s) have the expected encoding.
EncodingMissingBom         = === UTF-8 missing BOM ({0}) ===
EncodingUnexpectedBom      = === UTF-8 unexpected BOM ({0}) ===
EncodingGbkRoundTripFailed = [SKIP] GBK round-trip verification failed: {0}
EncodingCannotConvert      = [SKIP] Cannot convert: {0} ({1})
LineEndingAllOk            = [OK] All {0} file(s) have the expected line endings.
LineEndingExpected         = {0} (expected {1})
LineEndingWrong            = === Wrong line ending ({0}) ===
LineEndingUtf16            = === UTF-16, fix encoding first ({0}) ===
LineEndingUtf16Skipped     = [SKIP] UTF-16 file, run fix-encoding.ps1 first: {0}
LineEndingLoneCr           = Contains lone CR, left untouched: {0}
'@
