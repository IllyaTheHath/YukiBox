using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Win32.SafeHandles;

using Windows.Win32;
using Windows.Win32.Foundation;

namespace YukiBox.Desktop.Interop
{
    internal static class AppWindowExtensions
    {
        public static AppWindow GetAppWindow(this Window window)
        {
            var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);

            return GetAppWindowFromWindowHandle(windowHandle);
        }

        private static AppWindow GetAppWindowFromWindowHandle(IntPtr windowHandle)
        {
            WindowId windowId;
            windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);

            return AppWindow.GetFromWindowId(windowId);
        }

    }

    public static class WindowExtensions
    {
        public static void ShowWindow(this Window window)
        {
            var appWindow = window.GetAppWindow();
            if(appWindow is not null && !appWindow.IsVisible)
            {
                appWindow.Show();
            }
        }

        public static void HideWindow(this Window window)
        {
            var appWindow = window.GetAppWindow();
            if (appWindow is not null && appWindow.IsVisible)
            {
                appWindow.Hide();
            }
        }

        public static void SetIcon(this Window window, String icon)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var hIcon = PInvoke.LoadImage(null, icon, Windows.Win32.UI.WindowsAndMessaging.GDI_IMAGE_TYPE.IMAGE_ICON,
                16, 16,
                Windows.Win32.UI.WindowsAndMessaging.IMAGE_FLAGS.LR_LOADFROMFILE);
            PInvoke.SendMessage(new HWND(hwnd), PInvoke.WM_SETICON, PInvoke.ICON_SMALL, hIcon.DangerousGetHandle());
        }
    }
}