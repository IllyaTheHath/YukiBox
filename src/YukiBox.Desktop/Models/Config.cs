using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.Models
{
    public class Config
    {
        public Config_System System { get; private set; }

        public Config_Taskbar Taskbar { get; private set; }

        public Config()
        {
            System = new();
            Taskbar = new Config_Taskbar();
        }
    }

    public class Config_System
    {
        public String Language
        {
            get => LocalSettingHelper.Get<String>();
            set => LocalSettingHelper.Set(value);
        }
    }

    public class Config_Taskbar
    {
        public String SearchBoxTextDefault
        {
            get => LocalSettingHelper.Get<String>();
            set => LocalSettingHelper.Set(value);
        }

        public Boolean SearchBoxTextUpdateEnable
        {
            get => LocalSettingHelper.Get<Boolean>();
            set => LocalSettingHelper.Set(value);
        }

        public Boolean CompatibleStartIsBack
        {
            get => LocalSettingHelper.Get<Boolean>();
            set => LocalSettingHelper.Set(value);
        }

        public String MusicPlayer
        {
            get => LocalSettingHelper.Get<String>();
            set => LocalSettingHelper.Set(value);
        }
    }
}