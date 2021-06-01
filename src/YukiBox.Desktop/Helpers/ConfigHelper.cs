using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.Helpers
{
    public static class ConfigHelper
    {
        public static Config CurrentConfig { get; private set; }

        static ConfigHelper()
        {
            CurrentConfig = new();
        }

        public static async Task Clear()
        {
            await LocalSettingHelper.Clear();
        }
    }
}