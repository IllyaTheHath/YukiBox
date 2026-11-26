using System.Diagnostics;

using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.Tasks;

public class SearchboxTask : BackgroundTaskBase
{
    private MusicPlayer _player;
    private string _defaultText;
    private bool _compatibleStartIsBack;

    public SearchboxTask() : this(TimeSpan.FromSeconds(1), 0)
    {
    }

    public SearchboxTask(TimeSpan interval, int runCount = 0) : base(interval, runCount)
    {
    }

    public override void Init()
    {
        try
        {
            string playerName = ConfigHelper.CurrentConfig.Taskbar.MusicPlayer;
            if (!string.IsNullOrEmpty(playerName) && _player?.Name != playerName)
            {
                var type = Type.GetType(playerName);
                _player = Activator.CreateInstance(type) as MusicPlayer;
            }
            _defaultText = ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextDefault;
            _compatibleStartIsBack = ConfigHelper.CurrentConfig.Taskbar.CompatibleStartIsBack;
        }
        catch { }
        if (ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable && CommonUtils.WindowsVersion == WindowsVersion.Windows10)
        {
            Run();
        }
    }

    public void ChangeSettings(MusicPlayer player, string text, bool compatibleStartIsBack)
    {
        _player = player;
        _defaultText = text;
        _compatibleStartIsBack = compatibleStartIsBack;

        // refresh
        if (IsRunning)
        {
            string musicName = _player?.GetMusicName();
            if (string.IsNullOrEmpty(musicName))
            {
                musicName = _defaultText;
            }
            SearchboxHelper.SetSearchboxText(musicName, _compatibleStartIsBack);
        }
    }

    protected override Task ActionAsync()
    {
        if (ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable && CommonUtils.WindowsVersion == WindowsVersion.Windows10)
        {
            string musicName = _player?.GetMusicName();
            if (string.IsNullOrEmpty(musicName))
            {
                musicName = _defaultText;
            }

            string text = SearchboxHelper.GetSearchboxText();
            if (text != musicName)
            {
                SearchboxHelper.SetSearchboxText(musicName, _compatibleStartIsBack);
                Debug.WriteLine($"set searchbox text to \"{text}\" at {DateTime.Now}");
            }
        }
        else
        {
            Stop();
            ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable = false;
        }

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _player = null;
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
