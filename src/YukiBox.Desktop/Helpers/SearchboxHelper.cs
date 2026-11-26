using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace YukiBox.Desktop.Helpers;

public static class SearchboxHelper
{
    public static string GetSearchboxText()
    {
        HWND taskbarHWnd = PInvoke.FindWindow("Shell_TrayWnd", null);
        if (taskbarHWnd == HWND.Null)
        {
            return string.Empty;
        }

        HWND searchBoxHWnd = PInvoke.FindWindowEx(taskbarHWnd, HWND.Null, "TrayDummySearchControl", null);
        if (searchBoxHWnd == HWND.Null)
        {
            return string.Empty;
        }

        HWND searchBoxTextHWnd = PInvoke.FindWindowEx(searchBoxHWnd, HWND.Null, "Static", null);
        if (searchBoxTextHWnd == HWND.Null)
        {
            return string.Empty;
        }

        int bufferSize = PInvoke.GetWindowTextLength(searchBoxTextHWnd) + 1;
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
        HWND taskbarHWnd = PInvoke.FindWindow("Shell_TrayWnd", null);
        if (taskbarHWnd == HWND.Null)
        {
            return false;
        }
        HWND searchBoxHWnd = PInvoke.FindWindowEx(taskbarHWnd, HWND.Null, "TrayDummySearchControl", null);
        if (searchBoxHWnd == HWND.Null)
        {
            return false;
        }
        HWND searchBoxTextHWnd = PInvoke.FindWindowEx(searchBoxHWnd, HWND.Null, "Static", null);
        if (searchBoxTextHWnd == HWND.Null)
        {
            return false;
        }
        PInvoke.SetWindowText(searchBoxTextHWnd, text);

        // force refresh searchbox for the changes to take effect
        if (compatibleStartIsBack)
        {
            PInvoke.ShowWindow(searchBoxTextHWnd, SHOW_WINDOW_CMD.SW_SHOWNOACTIVATE);
            PInvoke.ShowWindow(searchBoxTextHWnd, SHOW_WINDOW_CMD.SW_HIDE);
        }
        else
        {
            PInvoke.ShowWindow(searchBoxTextHWnd, SHOW_WINDOW_CMD.SW_HIDE);
            PInvoke.ShowWindow(searchBoxTextHWnd, SHOW_WINDOW_CMD.SW_SHOWNOACTIVATE);
        }
        return true;
    }
}
