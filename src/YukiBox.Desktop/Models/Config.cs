using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Models;

public class Config
{
    public Config_System System { get; private set; }

    public Config_Taskbar Taskbar { get; private set; }

    public Config()
    {
        System = new();
        Taskbar = new();
    }
}

public class Config_System
{
    public string Language
    {
        get => LocalSettingHelper.Get<string>();
        set => LocalSettingHelper.Set(value);
    }
}

public class Config_Taskbar
{
    public bool WheelVolumeEnable
    {
        get => LocalSettingHelper.Get<bool>();
        set => LocalSettingHelper.Set(value);
    }
}
