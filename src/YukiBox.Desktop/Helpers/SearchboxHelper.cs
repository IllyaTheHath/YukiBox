using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Win32;

namespace YukiBox.Desktop.Helpers
{
    public static class SearchboxHelper
    {
        public static String GetSearchboxText()
        {
            var taskbarHWnd = PInvoke.FindWindow("Shell_TrayWnd", null);
            if (taskbarHWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return String.Empty;
            }

            var searchBoxHWnd = PInvoke.FindWindowEx(taskbarHWnd, Windows.Win32.Foundation.HWND.Null , "TrayDummySearchControl", null);
            if (searchBoxHWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return String.Empty;
            }

            var searchBoxTextHWnd = PInvoke.FindWindowEx(searchBoxHWnd, Windows.Win32.Foundation.HWND.Null, "Static", null);
            if (searchBoxTextHWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return String.Empty;
            }

            Windows.Win32.Foundation.PWSTR text = default;
            if (PInvoke.GetWindowText(searchBoxTextHWnd, text, 0) > 0)
            {
                return text.ToString();
            }

            return String.Empty;
        }

        public static Boolean SetSearchboxText(String text, Boolean compatibleStartIsBack)
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