using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Models
{
    public class Config
    {
        public Config_System System { get; set; }

        public Config()
        {
            System = new();
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
}