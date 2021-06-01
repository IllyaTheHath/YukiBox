using System;

using PInvoke;

namespace YukiBox.Desktop.Models
{
    public class CloudMusicPlayer : MusicPlayer
    {
        //public override String Name => "CloudMusic";

        protected override String DisplayNameResourceName => "Taskbar.Player.CloudMusic";

        public override String GetMusicName()
        {
            var hWnd = User32.FindWindow("OrpheusBrowserHost", null);
            if (hWnd == IntPtr.Zero)
            {
                return String.Empty;
            }

            return User32.GetWindowText(hWnd);
        }
    }
}