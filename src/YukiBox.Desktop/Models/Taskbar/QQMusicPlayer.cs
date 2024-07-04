using System;

using Windows.Win32;

namespace YukiBox.Desktop.Models
{
    public class QQMusicPlayer : MusicPlayer
    {
        //public override String Name => "QQMusic";

        protected override String DisplayNameResourceName => "Taskbar.Player.QQMusic";

        public override String GetMusicName()
        {
            var hWnd = PInvoke.FindWindow("QQMusic_Daemon_Wnd", null);

            if (hWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return String.Empty;
            }

            Windows.Win32.Foundation.PWSTR name = default;
            if (PInvoke.GetWindowText(hWnd, name, 0) > 0)
            {
                return name.ToString();
            }

            return String.Empty;
        }
    }
}