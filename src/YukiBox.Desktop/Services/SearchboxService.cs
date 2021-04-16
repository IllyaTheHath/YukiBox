using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PInvoke;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Services
{
    public class SearchboxService : ISearchboxService
    {
        public String GetSearchboxText()
        {
            var taskbarHWnd = User32.FindWindow("Shell_TrayWnd", null);
            if (taskbarHWnd == IntPtr.Zero)
            {
                return String.Empty;
            }

            var searchBoxHWnd = User32.FindWindowEx(taskbarHWnd, IntPtr.Zero, "TrayDummySearchControl", null);
            if (searchBoxHWnd == IntPtr.Zero)
            {
                return String.Empty;
            }

            var searchBoxTextHWnd = User32.FindWindowEx(searchBoxHWnd, IntPtr.Zero, "Static", null);
            if (searchBoxTextHWnd == IntPtr.Zero)
            {
                return String.Empty;
            }

            var searchboxText = User32.GetWindowText(searchBoxTextHWnd);
            return searchboxText;
        }

        public Boolean SetSearchboxText(String text)
        {
            var taskbarHWnd = User32.FindWindow("Shell_TrayWnd", null);
            if (taskbarHWnd == IntPtr.Zero)
            {
                return false;
            }
            var searchBoxHWnd = User32.FindWindowEx(taskbarHWnd, IntPtr.Zero, "TrayDummySearchControl", null);
            if (searchBoxHWnd == IntPtr.Zero)
            {
                return false;
            }
            var searchBoxTextHWnd = User32.FindWindowEx(searchBoxHWnd, IntPtr.Zero, "Static", null);
            if (searchBoxTextHWnd == IntPtr.Zero)
            {
                return false;
            }
            User32.SetWindowText(searchBoxTextHWnd, text);

            // force refresh searchbox for the changes to take effect
            User32.ShowWindow(searchBoxTextHWnd, User32.WindowShowStyle.SW_HIDE);
            User32.ShowWindow(searchBoxTextHWnd, User32.WindowShowStyle.SW_SHOWNOACTIVATE);
            return true;
        }
    }
}
