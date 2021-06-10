using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using PInvoke;

using YukiBox.Desktop.Helpers;

using static PInvoke.User32;

namespace YukiBox.Desktop.Hooks
{
    public class WheelVolumeHookHelper : HookHelperBase
    {
        private const String _taskbarClassName = "Shell_TrayWnd";

        public override void Initialize()
        {
            var enabled = ConfigHelper.CurrentConfig.Taskbar.WheelVolumeEnable;
            if (enabled != IsEnabled)
            {
                IsEnabled = enabled;
            }
        }

        private void MouseWheel(MOUSEINPUT input, Int16 delta)
        {
            var shouldAction = !IsForegroundWindowFullScreen() && IsMouseOverTaskbar(input.dx, input.dy);
            if (shouldAction)
            {
                var up = delta > 0;
                var step = Math.Abs(delta) / 120;
                for (var i = 0; i < step; i++)
                {
                    AudioDeviceHelper.StepSystemVolume(up, true);
                }
            }
        }

        private Boolean IsWindowsFullScreen(IntPtr hwnd)
        {
            if (hwnd != IntPtr.Zero)
            {
                if (GetWindowRect(hwnd, out var rect))
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

        private Boolean IsForegroundWindowFullScreen()
        {
            var hwnd = GetForegroundWindow();
            var shellHwnd = GetShellWindow();
            var desktopHwnd = GetDesktopWindow();
            if (hwnd == shellHwnd || hwnd == desktopHwnd)
                return false;
            return IsWindowsFullScreen(hwnd);
        }

        private Size GetScreenSize()
        {
            var width = GetSystemMetrics(SystemMetric.SM_CXSCREEN);
            var height = GetSystemMetrics(SystemMetric.SM_CYSCREEN);
            return new Size(width, height);
        }

        private Boolean IsMouseOverTaskbar(Double x, Double y)
        {
            var taskbarRect = GetTaskbarLocation();
            return taskbarRect.Contains(x, y);
        }

        private Rect GetTaskbarLocation()
        {
            var hwnd = FindWindow(_taskbarClassName, null);
            return GetWindowRect(hwnd, out var rect) ? new Rect(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top) : new Rect();
        }

        protected override void OnEnabled()
        {
            ConfigHelper.CurrentConfig.Taskbar.WheelVolumeEnable = IsEnabled;

            if (IsEnabled)
            {
                HooksHelper.Instance.MouseHook.MouseWheel += MouseWheel;
            }
        }

        protected override void OnDisabled()
        {
            ConfigHelper.CurrentConfig.Taskbar.WheelVolumeEnable = IsEnabled;

            if (!IsEnabled)
            {
                HooksHelper.Instance.MouseHook.MouseWheel -= MouseWheel;
            }
        }
    }
}