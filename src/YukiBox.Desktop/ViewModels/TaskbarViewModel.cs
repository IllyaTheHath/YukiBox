using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Hooks;

namespace YukiBox.Desktop.ViewModels;

public class TaskbarViewModel : ViewModelBase
{
    private bool _enableWheelVolume;

    public bool EnableWheelVolume
    {
        get => _enableWheelVolume;
        set
        {
            if (SetProperty(ref _enableWheelVolume, value))
            {
                HooksHelper.Instance.WheelVolumeHookHelper.IsEnabled = EnableWheelVolume;
            }
        }
    }

    public TaskbarViewModel()
    {
        EnableWheelVolume = ConfigHelper.CurrentConfig.Taskbar.WheelVolumeEnable;
    }
}
