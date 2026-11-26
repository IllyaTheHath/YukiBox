using Windows.Win32;
using Windows.Win32.Foundation;

namespace YukiBox.Desktop.Models;

public class CloudMusicPlayer : MusicPlayer
{
    //public override string Name => "CloudMusic";

    protected override string DisplayNameResourceName => "Taskbar.Player.CloudMusic";

    public override string GetMusicName()
    {
        HWND hWnd = PInvoke.FindWindow("OrpheusBrowserHost", null);
        if (hWnd == HWND.Null)
        {
            return string.Empty;
        }

        int bufferSize = PInvoke.GetWindowTextLength(hWnd) + 1;
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
