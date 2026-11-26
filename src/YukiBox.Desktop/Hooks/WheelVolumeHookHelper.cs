using Windows.Foundation;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Hooks;

public class WheelVolumeHookHelper : HookHelperBase
{
    private const string TaskbarClassName = "Shell_TrayWnd";

    public override void Initialize()
    {
        bool enabled = ConfigHelper.CurrentConfig.Taskbar.WheelVolumeEnable;
        if (enabled != IsEnabled)
        {
            IsEnabled = enabled;
        }
    }

    private void MouseWheel(MOUSEINPUT input, short delta)
    {
        bool shouldAction = !IsForegroundWindowFullScreen() && IsMouseOverTaskbar(input.dx, input.dy);
        if (shouldAction)
        {
            bool up = delta > 0;
            int step = Math.Abs(delta) / 120;
            for (int i = 0; i < step; i++)
            {
                AudioDeviceHelper.StepSystemVolume(up, true);
            }
        }
    }

    private void MouseMiddleButton(MOUSEINPUT input)
    {
        bool shouldAction = !IsForegroundWindowFullScreen() && IsMouseOverTaskbar(input.dx, input.dy);
        if (shouldAction)
        {
            AudioDeviceHelper.Mute(true);
        }
    }

    private bool IsWindowsFullScreen(IntPtr hwnd)
    {
        if (hwnd != IntPtr.Zero)
        {
            if (PInvoke.GetWindowRect(new HWND(hwnd), out var rect))
            {
                var screenSize = GetScreenSize();
                if (rect.top == 0 &&
                    rect.left == 0 &&
                    rect.bottom == screenSize.Height &&
                    rect.right == screenSize.Width)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsForegroundWindowFullScreen()
    {
        var hwnd = PInvoke.GetForegroundWindow();
        var shellHwnd = PInvoke.GetShellWindow();
        var desktopHwnd = PInvoke.GetDesktopWindow();
        if (hwnd == shellHwnd || hwnd == desktopHwnd)
        {
            return false;
        }
        return IsWindowsFullScreen(hwnd);
    }

    private Size GetScreenSize()
    {
        int width = PInvoke.GetSystemMetrics(Windows.Win32.UI.WindowsAndMessaging.SYSTEM_METRICS_INDEX.SM_CXSCREEN);
        int height = PInvoke.GetSystemMetrics(Windows.Win32.UI.WindowsAndMessaging.SYSTEM_METRICS_INDEX.SM_CYSCREEN);
        return new Size(width, height);
    }

    private bool IsMouseOverTaskbar(double x, double y)
    {
        var taskbarRect = GetTaskbarLocation();
        return taskbarRect.Contains(new Point(x, y));
    }

    private Rect GetTaskbarLocation()
    {
        var hwnd = PInvoke.FindWindow(TaskbarClassName, null);
        return PInvoke.GetWindowRect(hwnd, out var rect) ? new Rect(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top) : new Rect();
    }

    protected override void OnEnabled()
    {
        ConfigHelper.CurrentConfig.Taskbar.WheelVolumeEnable = IsEnabled;

        if (IsEnabled)
        {
            HooksHelper.Instance.MouseHook.MouseWheel += MouseWheel;
            HooksHelper.Instance.MouseHook.MiddleButtonDown += MouseMiddleButton;
        }
    }

    protected override void OnDisabled()
    {
        ConfigHelper.CurrentConfig.Taskbar.WheelVolumeEnable = IsEnabled;

        if (!IsEnabled)
        {
            HooksHelper.Instance.MouseHook.MouseWheel -= MouseWheel;
            HooksHelper.Instance.MouseHook.MiddleButtonDown -= MouseMiddleButton;
        }
    }
}
