using System;
using System.Diagnostics;
using System.Threading.Tasks;

using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.Tasks
{
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
                var playerName = ConfigHelper.CurrentConfig.Taskbar.MusicPlayer;
                if (!string.IsNullOrEmpty(playerName) && this._player?.Name != playerName)
                {
                    var type = Type.GetType(playerName);
                    this._player = Activator.CreateInstance(type) as MusicPlayer;
                }
                this._defaultText = ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextDefault;
                this._compatibleStartIsBack = ConfigHelper.CurrentConfig.Taskbar.CompatibleStartIsBack;
            }
            catch { }
            if (ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable && CommonUtils.WindowsVersion == WindowsVersion.Windows10)
            {
                Run();
            }
        }

        public void ChangeSettings(MusicPlayer player, string text, bool compatibleStartIsBack)
        {
            this._player = player;
            this._defaultText = text;
            this._compatibleStartIsBack = compatibleStartIsBack;

            // refresh
            if (IsRunning)
            {
                var musicName = this._player?.GetMusicName();
                if (string.IsNullOrEmpty(musicName))
                {
                    musicName = this._defaultText;
                }
                SearchboxHelper.SetSearchboxText(musicName, this._compatibleStartIsBack);
            }
        }

        protected override Task Action()
        {
            if (ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable && CommonUtils.WindowsVersion == WindowsVersion.Windows10)
            {
                var musicName = this._player?.GetMusicName();
                if (string.IsNullOrEmpty(musicName))
                {
                    musicName = this._defaultText;
                }

                var text = SearchboxHelper.GetSearchboxText();
                if (text != musicName)
                {
                    SearchboxHelper.SetSearchboxText(musicName, this._compatibleStartIsBack);
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
            this._player = null;
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}