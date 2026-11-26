using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.Helpers;

public static class ConfigHelper
{
    public static Config CurrentConfig { get; private set; }

    static ConfigHelper()
    {
        CurrentConfig = new();
    }

    public static async Task ClearAsync()
    {
        await LocalSettingHelper.ClearAsync();
    }
}
