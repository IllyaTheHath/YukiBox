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

        public SearchboxTask() : this(TimeSpan.FromSeconds(1), 0)
        {
        }

        public SearchboxTask(TimeSpan interval, Int32 runCount = 0) : base(interval, runCount)
        {
        }

        public override void Init()
        {
            if (ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable)
            {
                Run();
            }
        }

        protected override async Task Action()
        {
            if (ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable)
            {
                try
                {
                    var playerName = ConfigHelper.CurrentConfig.Taskbar.MusicPlayer;
                    if (!String.IsNullOrEmpty(playerName) && this._player?.Name != playerName)
                    {
                        var type = Type.GetType(playerName);
                        this._player = Activator.CreateInstance(type) as MusicPlayer;
                    }

                    var musicName = this._player?.GetMusicName();
                    if (String.IsNullOrEmpty(musicName))
                    {
                        musicName = ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextDefault;
                    }

                    var text = SearchboxHelper.GetSearchboxText();
                    if (text != musicName)
                    {
                        SearchboxHelper.SetSearchboxText(musicName);
                        Debug.WriteLine($"set searchbox text to \"{text}\" at {DateTime.Now}");
                    }
                }
                catch
                { }
            }
            else
            {
                Stop();
            }

            await Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            this._player = null;
        }
    }
}