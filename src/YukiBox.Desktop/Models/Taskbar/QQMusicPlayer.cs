using System;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

using Windows.Win32;

namespace YukiBox.Desktop.Models
{
    internal class QQMusicPlayer : MusicPlayer
    {
        //public override string Name => "QQMusic";

        protected override string DisplayNameResourceName => "Taskbar.Player.QQMusic";

        public override string GetMusicName()
        {
            var hWnd = PInvoke.FindWindow("QQMusic_Daemon_Wnd", null);

            if (hWnd == Windows.Win32.Foundation.HWND.Null)
            {
                return string.Empty;
            }

            var bufferSize = PInvoke.GetWindowTextLength(hWnd) + 1;
            unsafe
            {
                fixed (char* nameChar = new char[bufferSize])
                {
                    if (PInvoke.GetWindowText(hWnd, nameChar, bufferSize) > 0)
                    {
                        return new string(nameChar);
                    }
                }
            }

            return string.Empty;
        }
    }
}