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

            var bufferSize = PInvoke.GetWindowTextLength(hWnd) + 1;
            unsafe
            {
                fixed (Char* nameChar = new Char[bufferSize])
                {
                    if (PInvoke.GetWindowText(hWnd, nameChar, bufferSize) > 0)
                    {
                        return new String(nameChar);
                    }
                }
            }

            return String.Empty;
        }
    }
}