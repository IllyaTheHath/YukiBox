using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YukiBox.Desktop.Controls;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.Tasks
{
    public class SearchboxTask : BackgroundTaskBase
    {
        private MusicPlayer _player;
        private String _defaultText;
        private Boolean _compatibleStartIsBack;

        public SearchboxTask() : this(TimeSpan.FromSeconds(1), 0)
        {
        }

        public SearchboxTask(TimeSpan interval, Int32 runCount = 0) : base(interval, runCount)
        {
        }

        public override void Init()
        {
            try
            {
                var playerName = ConfigHelper.CurrentConfig.Taskbar.MusicPlayer;
                if (!String.IsNullOrEmpty(playerName) && this._player?.Name != playerName)
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

        public void ChangeSettings(MusicPlayer player, String text, Boolean compatibleStartIsBack)
        {
            this._player = player;
            this._defaultText = text;
            this._compatibleStartIsBack = compatibleStartIsBack;

            // refresh
            if (IsRunning)
            {
                var musicName = this._player?.GetMusicName();
                if (String.IsNullOrEmpty(musicName))
                {
                    musicName = this._defaultText;
                }
                SearchboxHelper.SetSearchboxText(musicName, this._compatibleStartIsBack);
            }
        }

        protected override async Task Action()
        {
            if (ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable && CommonUtils.WindowsVersion == WindowsVersion.Windows10)
            {
                var musicName = this._player?.GetMusicName();
                if (String.IsNullOrEmpty(musicName))
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

            await Task.CompletedTask;
        }

        public override void Dispose()
        {
            this._player = null;
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}