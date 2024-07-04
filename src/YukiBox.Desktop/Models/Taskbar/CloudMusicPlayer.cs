using System;

using Windows.Win32;

namespace YukiBox.Desktop.Models
{
    public class CloudMusicPlayer : MusicPlayer
    {
        //public override String Name => "CloudMusic";

        protected override String DisplayNameResourceName => "Taskbar.Player.CloudMusic";

        public override String GetMusicName()
        {
            var hWnd = PInvoke.FindWindow("OrpheusBrowserHost", null);
            if (hWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return String.Empty;
            }


            Windows.Win32.Foundation.PWSTR name = default;
            if(PInvoke.GetWindowText(hWnd, name, 0) > 0)
            {
                return name.ToString();
            }

            return String.Empty;
        }
    }
}