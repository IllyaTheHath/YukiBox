using System;

using Windows.Win32;

namespace YukiBox.Desktop.Helpers
{
    public static class SearchboxHelper
    {
        public static string GetSearchboxText()
        {
            var taskbarHWnd = PInvoke.FindWindow("Shell_TrayWnd", null);
            if (taskbarHWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return string.Empty;
            }

            var searchBoxHWnd = PInvoke.FindWindowEx(taskbarHWnd, Windows.Win32.Foundation.HWND.Null , "TrayDummySearchControl", null);
            if (searchBoxHWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return string.Empty;
            }

            var searchBoxTextHWnd = PInvoke.FindWindowEx(searchBoxHWnd, Windows.Win32.Foundation.HWND.Null, "Static", null);
            if (searchBoxTextHWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return string.Empty;
            }

            var bufferSize = PInvoke.GetWindowTextLength(searchBoxTextHWnd) + 1;
            unsafe
            {
                fixed (char* nameChar = new char[bufferSize])
                {
                    if (PInvoke.GetWindowText(searchBoxTextHWnd, nameChar, bufferSize) > 0)
                    {
                        return new string(nameChar);
                    }
                }
            }

            return string.Empty;
        }

        public static bool SetSearchboxText(string text, bool compatibleStartIsBack)
        {
            var taskbarHWnd = PInvoke.FindWindow("Shell_TrayWnd", null);
            if (taskbarHWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return false;
            }
            var searchBoxHWnd = PInvoke.FindWindowEx(taskbarHWnd, Windows.Win32.Foundation.HWND.Null, "TrayDummySearchControl", null);
            if (searchBoxHWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return false;
            }
            var searchBoxTextHWnd = PInvoke.FindWindowEx(searchBoxHWnd, Windows.Win32.Foundation.HWND.Null, "Static", null);
            if (searchBoxTextHWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return false;
            }
            PInvoke.SetWindowText(searchBoxTextHWnd, text);

            // force refresh searchbox for the changes to take effect
            if (compatibleStartIsBack)
            {
                PInvoke.ShowWindow(searchBoxTextHWnd, Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD.SW_SHOWNOACTIVATE);
                PInvoke.ShowWindow(searchBoxTextHWnd, Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD.SW_HIDE);
            }
            else
            {
                PInvoke.ShowWindow(searchBoxTextHWnd, Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD.SW_HIDE);
                PInvoke.ShowWindow(searchBoxTextHWnd, Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD.SW_SHOWNOACTIVATE);
            }
            return true;
        }
    }
}