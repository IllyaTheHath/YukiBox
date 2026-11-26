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
                OnPropertyChanged(nameof(IsWheelVolumeAdjustAvailable));
                OnPropertyChanged(nameof(IsWheelVolumeModifierAvailable));
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
                OnPropertyChanged(nameof(IsWheelVolumeAdjustAvailable));
                OnPropertyChanged(nameof(IsWheelVolumeModifierAvailable));
            }
        }
    }

    public bool IsWheelVolumeAdjustAvailable => EnableWheelVolume && EnableWheelVolumeAdjust;

    private double _wheelVolumeStep;

    public double WheelVolumeStep
    {
        get => _wheelVolumeStep;
        set
        {
            if (SetProperty(ref _wheelVolumeStep, value))
            {
                int step = (int)value;
                ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeStep = step;
                HooksHelper.Instance.WheelVolumeHookHelper.VolumeStep = step;
            }
        }
    }

    private bool _enableWheelVolumeModifier;

    public bool EnableWheelVolumeModifier
    {
        get => _enableWheelVolumeModifier;
        set
        {
            if (SetProperty(ref _enableWheelVolumeModifier, value))
            {
                ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeModifierEnable = value;
                HooksHelper.Instance.WheelVolumeHookHelper.IsModifierEnabled = value;
                OnPropertyChanged(nameof(IsWheelVolumeModifierAvailable));
            }
        }
    }

    public bool IsWheelVolumeModifierAvailable => IsWheelVolumeAdjustAvailable && EnableWheelVolumeModifier;

    private double _wheelVolumeShiftMultiplier;

    public double WheelVolumeShiftMultiplier
    {
        get => _wheelVolumeShiftMultiplier;
        set
        {
            if (SetProperty(ref _wheelVolumeShiftMultiplier, value))
            {
                int multiplier = (int)value;
                ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeShiftMultiplier = multiplier;
                HooksHelper.Instance.WheelVolumeHookHelper.ShiftMultiplier = multiplier;
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

    public AudioEnhancementViewModel()
    {
        _enableWheelVolume = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeEnable;
        _enableWheelVolumeAdjust = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeAdjustEnable;
        _wheelVolumeStep = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeStep;
        _enableWheelVolumeModifier = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeModifierEnable;
        _wheelVolumeShiftMultiplier = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeShiftMultiplier;
        _reverseWheelVolume = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeReverse;
        _enableWheelVolumeMiddleClickMute = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeMiddleClickMuteEnable;
    }
}
