using System;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

using Windows.Win32;

namespace YukiBox.Desktop.Models
{
    internal class QQMusicPlayer : MusicPlayer
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