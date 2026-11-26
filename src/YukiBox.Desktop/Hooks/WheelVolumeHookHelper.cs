using System.Drawing;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Hooks;

public class WheelVolumeHookHelper : HookHelperBase
{
    private const string TaskbarClassName = "Shell_TrayWnd";
    private const string SecondaryTaskbarClassName = "Shell_SecondaryTrayWnd";

    public bool IsAdjustEnabled { get; set; }

    public bool IsMiddleClickMuteEnabled { get; set; }

    public bool IsReversed { get; set; }

    public override void Initialize()
    {
        IsAdjustEnabled = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeAdjustEnable;
        IsMiddleClickMuteEnabled = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeMiddleClickMuteEnable;
        IsReversed = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeReverse;
        IsEnabled = ConfigHelper.CurrentConfig.AudioEnhancement.WheelVolumeEnable;
    }

    protected override void OnEnabled()
    {
        HooksHelper.Instance.MouseHook.MouseWheel += MouseWheel;
        HooksHelper.Instance.MouseHook.MiddleButtonDown += MouseMiddleButton;
    }

    protected override void OnDisabled()
    {
        HooksHelper.Instance.MouseHook.MouseWheel -= MouseWheel;
        HooksHelper.Instance.MouseHook.MiddleButtonDown -= MouseMiddleButton;
    }

    private void MouseWheel(MSLLHOOKSTRUCT mouseInfo, short delta)
    {
        bool shouldAction = IsAdjustEnabled && IsPointOverTaskbar(mouseInfo.pt);
        if (shouldAction)
        {
            bool up = (delta > 0) != IsReversed;
            int step = Math.Abs(delta) / 120;
            for (int i = 0; i < step; i++)
            {
                AudioDeviceHelper.StepSystemVolume(up, true);
            }
        }
    }

    private void MouseMiddleButton(MSLLHOOKSTRUCT mouseInfo)
    {
        bool shouldAction = IsMiddleClickMuteEnabled && IsPointOverTaskbar(mouseInfo.pt);
        if (shouldAction)
        {
            AudioDeviceHelper.Mute(true);
        }
    }

    private bool IsPointOverTaskbar(Point point)
    {
        HWND hwnd = PInvoke.WindowFromPoint(point);
        if (hwnd == HWND.Null)
        {
            return false;
        }

        HWND rootHwnd = PInvoke.GetAncestor(hwnd, GET_ANCESTOR_FLAGS.GA_ROOT);
        Span<char> buffer = stackalloc char[256];
        int length = PInvoke.GetClassName(rootHwnd, buffer);
        string className = new(buffer[..length]);
        bool isTaskbar = className is TaskbarClassName or SecondaryTaskbarClassName;
        return isTaskbar;
    }
}
