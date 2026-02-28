using System.Threading.Tasks;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.Services
{
    public class ConfigService : IConfigService
    {
        public Config CurrentConfig { get; private set; }

        public ConfigService()
        {
            CurrentConfig = new();
        }

        public async Task Clear()
        {
            await LocalSettingHelper.Clear();
        }
    }
}