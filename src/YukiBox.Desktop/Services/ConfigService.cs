using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.Services;

public class ConfigService : IConfigService
{
    public Config CurrentConfig { get; private set; }

    public ConfigService()
    {
        CurrentConfig = new();
    }

    public async Task ClearAsync()
    {
        await LocalSettingHelper.ClearAsync();
    }
}
