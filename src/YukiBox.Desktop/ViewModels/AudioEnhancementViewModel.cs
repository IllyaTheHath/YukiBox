using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Hooks;

namespace YukiBox.Desktop.ViewModels;

public class AudioEnhancementViewModel : ViewModelBase
{
    private bool _enableWheelVolume;

    public bool EnableWheelVolume
    {
        get => _enableWheelVolume;
        set
        {
            if (SetProperty(ref _enableWheelVolume, value))
            {
                ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeEnable = value;
                HooksHelper.Instance.WheelVolumeHookHelper.IsEnabled = value;
            }
        }
    }

    private bool _enableWheelVolumeAdjust;

    public bool EnableWheelVolumeAdjust
    {
        get => _enableWheelVolumeAdjust;
        set
        {
            if (SetProperty(ref _enableWheelVolumeAdjust, value))
            {
                ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeAdjustEnable = value;
                HooksHelper.Instance.WheelVolumeHookHelper.IsAdjustEnabled = value;
            }
        }
    }

    private bool _enableWheelVolumeMiddleClickMute;

    public bool EnableWheelVolumeMiddleClickMute
    {
        get => _enableWheelVolumeMiddleClickMute;
        set
        {
            if (SetProperty(ref _enableWheelVolumeMiddleClickMute, value))
            {
                ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeMiddleClickMuteEnable = value;
                HooksHelper.Instance.WheelVolumeHookHelper.IsMiddleClickMuteEnabled = value;
            }
        }
    }

    private bool _reverseWheelVolume;

    public bool ReverseWheelVolume
    {
        get => _reverseWheelVolume;
        set
        {
            if (SetProperty(ref _reverseWheelVolume, value))
            {
                ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeReverse = value;
                HooksHelper.Instance.WheelVolumeHookHelper.IsReversed = value;
            }
        }
    }

    public AudioEnhancementViewModel()
    {
        _enableWheelVolume = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeEnable;
        _enableWheelVolumeAdjust = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeAdjustEnable;
        _enableWheelVolumeMiddleClickMute = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeMiddleClickMuteEnable;
        _reverseWheelVolume = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeReverse;
    }
}
