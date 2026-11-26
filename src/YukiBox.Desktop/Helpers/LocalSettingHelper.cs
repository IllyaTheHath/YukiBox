using System.Runtime.CompilerServices;

using Windows.Storage;

namespace YukiBox.Desktop.Helpers;

public static class LocalSettingHelper
{
    public static T Get<T>([CallerMemberName] string key = "")
    {
        T value = Get<T>(default, key);
        return value;
    }

    public static T Get<T>(T defaultValue, [CallerMemberName] string key = "")
    {
        try
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                string value = ApplicationData.Current.LocalSettings.Values[key]?.ToString();

                if (typeof(T).IsEnum)
                {
                    T enumValue = (T)Enum.Parse(typeof(T), value);
                    return enumValue;
                }

                T convertedValue = (T)Convert.ChangeType(value, typeof(T));
                return convertedValue;
            }
        }
        catch { }
        return defaultValue;
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

    public static async Task ClearAsync()
    {
        await ApplicationData.Current.ClearAsync();
    }
}
