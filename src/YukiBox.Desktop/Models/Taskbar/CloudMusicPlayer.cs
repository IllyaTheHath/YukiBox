using Windows.Win32;

namespace YukiBox.Desktop.Models;

public class CloudMusicPlayer : MusicPlayer
{
    //public override string Name => "CloudMusic";

    protected override string DisplayNameResourceName => "Taskbar.Player.CloudMusic";

    public override string GetMusicName()
    {
        var hWnd = PInvoke.FindWindow("OrpheusBrowserHost", null);
        if (hWnd == Windows.Win32.Foundation.HWND.Null)
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
