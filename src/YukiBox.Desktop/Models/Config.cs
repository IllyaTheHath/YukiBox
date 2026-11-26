using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Models;

public class Config
{
    public Config_System System { get; private set; }

    public Config_AudioEnhancement AudioEnhancement { get; private set; }

    public Config()
    {
        System = new();
        AudioEnhancement = new();
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

public class Config_AudioEnhancement
{
    public bool WheelVolumeEnable
    {
        get => LocalSettingHelper.Get<bool>();
        set => LocalSettingHelper.Set(value);
    }

    public bool WheelVolumeAdjustEnable
    {
        get => LocalSettingHelper.Get(true);
        set => LocalSettingHelper.Set(value);
    }

    public int WheelVolumeStep
    {
        get => LocalSettingHelper.Get(2);
        set => LocalSettingHelper.Set(value);
    }

    public bool WheelVolumeReverse
    {
        get => LocalSettingHelper.Get<bool>();
        set => LocalSettingHelper.Set(value);
    }

    public bool WheelVolumeMiddleClickMuteEnable
    {
        get => LocalSettingHelper.Get(true);
        set => LocalSettingHelper.Set(value);
    }
}
