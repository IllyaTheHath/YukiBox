using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

namespace YukiBox.Desktop.Helpers
{
    public static class LocalSettingHelper
    {
        public static T Get<T>([CallerMemberName] String key = "")
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
                {
                    var value = ApplicationData.Current.LocalSettings.Values[key]?.ToString();

                    if (typeof(T).IsEnum)
                    {
                        return (T)Enum.Parse(typeof(T), value);
                    }

                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
            catch { }
            return default;
        }

        public static Boolean Set<T>(T value, [CallerMemberName] String key = "")
        {
            try
            {
                if (value is not null)
                {
                    ApplicationData.Current.LocalSettings.Values[key] = value.ToString();
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static async Task Clear()
        {
            await ApplicationData.Current.ClearAsync();
        }
    }
}