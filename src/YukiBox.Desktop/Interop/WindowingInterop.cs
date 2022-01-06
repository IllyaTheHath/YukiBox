using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace YukiBox.Desktop.Interop
{
    public static class AppWindowExtensions
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
            var hIcon = PInvoke.User32.LoadImage(System.IntPtr.Zero, icon,
                      PInvoke.User32.ImageType.IMAGE_ICON, 16, 16, PInvoke.User32.LoadImageFlags.LR_LOADFROMFILE);

            PInvoke.User32.SendMessage(hwnd, PInvoke.User32.WindowMessage.WM_SETICON, (System.IntPtr)0, hIcon);
        }
    }
}