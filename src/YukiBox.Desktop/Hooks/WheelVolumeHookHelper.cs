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

    private bool _isMiddleButtonDownHandled;

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
        HooksHelper.Instance.MouseHook.MiddleButtonDown += MouseMiddleButtonDown;
        HooksHelper.Instance.MouseHook.MiddleButtonUp += MouseMiddleButtonUp;
    }

    protected override void OnDisabled()
    {
        HooksHelper.Instance.MouseHook.MouseWheel -= MouseWheel;
        HooksHelper.Instance.MouseHook.MiddleButtonDown -= MouseMiddleButtonDown;
        HooksHelper.Instance.MouseHook.MiddleButtonUp -= MouseMiddleButtonUp;
        _isMiddleButtonDownHandled = false;
    }

    private void MouseWheel(object sender, MouseHookEventArgs e)
    {
        bool shouldAction = IsAdjustEnabled && IsPointOverTaskbar(e.MouseInfo.pt);
        if (shouldAction)
        {
            bool up = (e.WheelDelta > 0) != IsReversed;
            int step = Math.Abs(e.WheelDelta) / 120;
            for (int i = 0; i < step; i++)
            {
                AudioDeviceHelper.StepSystemVolume(up, true);
            }

            e.Handled = true;
        }
    }

    private void MouseMiddleButtonDown(object sender, MouseHookEventArgs e)
    {
        bool shouldAction = IsMiddleClickMuteEnabled && IsPointOverTaskbar(e.MouseInfo.pt);
        if (shouldAction)
        {
            AudioDeviceHelper.Mute(true);
            e.Handled = true;
        }

        _isMiddleButtonDownHandled = shouldAction;
    }

    private void MouseMiddleButtonUp(object sender, MouseHookEventArgs e)
    {
        // Swallow the matching button up as well, otherwise the taskbar may still treat it as a middle click
        if (_isMiddleButtonDownHandled)
        {
            _isMiddleButtonDownHandled = false;
            e.Handled = true;
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
