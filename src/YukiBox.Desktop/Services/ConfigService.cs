using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

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