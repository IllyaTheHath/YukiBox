using System;

using PInvoke;

namespace YukiBox.Desktop.Models
{
    public class QQMusicPlayer : MusicPlayer
    {
        //public override String Name => "QQMusic";

        protected override String DisplayNameResourceName => "Taskbar.Player.QQMusic";

        public override String GetMusicName()
        {
            var hWnd = User32.FindWindow("QQMusic_Daemon_Wnd", null);

            if (hWnd == IntPtr.Zero)
            {
                return String.Empty;
            }

            return User32.GetWindowText(hWnd);
        }
    }
}