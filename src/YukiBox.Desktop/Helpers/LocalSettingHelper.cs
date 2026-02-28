using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Windows.Storage;

namespace YukiBox.Desktop.Helpers
{
    public static class LocalSettingHelper
    {
        public static T Get<T>([CallerMemberName] string key = "")
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

        public static bool Set<T>(T value, [CallerMemberName] string key = "")
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