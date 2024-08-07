﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Windows.Foundation;

using YukiBox.Desktop.Helpers;

using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Windows.Win32.Foundation;


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

        private Boolean IsForegroundWindowFullScreen()
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
            var width = PInvoke.GetSystemMetrics(Windows.Win32.UI.WindowsAndMessaging.SYSTEM_METRICS_INDEX.SM_CXSCREEN);
            var height = PInvoke.GetSystemMetrics(Windows.Win32.UI.WindowsAndMessaging.SYSTEM_METRICS_INDEX.SM_CYSCREEN);
            return new Size(width, height);
        }

        private Boolean IsMouseOverTaskbar(Double x, Double y)
        {
            var taskbarRect = GetTaskbarLocation();
            return taskbarRect.Contains(new Point(x, y));
        }

        private Rect GetTaskbarLocation()
        {
            var hwnd = PInvoke.FindWindow(_taskbarClassName, null);
            return PInvoke.GetWindowRect(hwnd, out var rect) ? new Rect(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top) : new Rect();
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