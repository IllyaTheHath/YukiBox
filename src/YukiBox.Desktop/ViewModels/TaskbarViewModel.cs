using System;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Models;
using YukiBox.Desktop.Tasks;

namespace YukiBox.Desktop.ViewModels
{
    public class TaskbarViewModel : ViewModelBase, IDisposable
    {
        private readonly IMediatorService _mediatorService;
        private readonly IBackgroundTask _task;

        public ObservableCollection<MusicPlayer> MusicPlayers { get; set; }

        public MusicPlayer SelectedPlayer
        {
            get => MusicPlayers.FirstOrDefault(x => x.Name == ConfigHelper.CurrentConfig.Taskbar.MusicPlayer);
            set => ConfigHelper.CurrentConfig.Taskbar.MusicPlayer = value?.Name;
        }

        public Boolean EnableMusicUpdate
        {
            get => ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable;
            set
            {
                ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable = value;
                if (EnableMusicUpdate)
                {
                    this._task?.Run();
                }
                else
                {
                    this._task?.Stop();
                }
            }
        }

        public String SearchBoxTextDefault
        {
            get => ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextDefault;
            set => ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextDefault = value;
        }

        public TaskbarViewModel()
        {
            this._mediatorService = Ioc.Default.GetService<IMediatorService>();
            this._task = Ioc.Default.GetServices<IBackgroundTask>().FirstOrDefault(x => x.Name == typeof(SearchboxTask).FullName);

            this._mediatorService.Register(this, "I18N", OnLocaleChange);

            MusicPlayers = new ObservableCollection<MusicPlayer>();

            InitMenu();
        }

        private void InitMenu()
        {
            MusicPlayers.Clear();

            var qqMusic = new QQMusicPlayer();
            MusicPlayers.Add(qqMusic);

            var cloudMusic = new CloudMusicPlayer();
            MusicPlayers.Add(cloudMusic);
        }

        private void OnLocaleChange(Object obj)
        {
            foreach (var item in MusicPlayers)
            {
                item.UpdateDisplayName();
            }
        }

        public void Dispose()
        {
            this._mediatorService.UnRegister(this, "I18N");
            GC.SuppressFinalize(this);
        }
    }
}