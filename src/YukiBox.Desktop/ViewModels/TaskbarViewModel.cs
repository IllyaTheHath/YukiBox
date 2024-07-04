using System;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Models;
using YukiBox.Desktop.Tasks;
using System.Windows.Input;
using YukiBox.Desktop.Hooks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace YukiBox.Desktop.ViewModels
{
    public class TaskbarViewModel : ViewModelBase
    {
        private readonly IBackgroundTask _task;

        public ObservableCollection<MusicPlayer> MusicPlayers { get; set; }

        private MusicPlayer _selectedPlayer;

        public MusicPlayer SelectedPlayer
        {
            get => this._selectedPlayer;
            set => SetProperty(ref this._selectedPlayer, value);
        }

        private Boolean _enableMusicUpdate;

        public Boolean EnableMusicUpdate
        {
            get => this._enableMusicUpdate;
            set => SetProperty(ref this._enableMusicUpdate, value);
        }

        private Boolean _compatibleStartIsBack;

        public Boolean CompatibleStartIsBack
        {
            get => this._compatibleStartIsBack;
            set => SetProperty(ref this._compatibleStartIsBack, value);
        }

        public String _searchBoxTextDefault;

        public String SearchBoxTextDefault
        {
            get => this._searchBoxTextDefault;
            set => SetProperty(ref this._searchBoxTextDefault, value);
        }

        private ICommand _saveChangeSearchbox;
        public ICommand SaveChangeSearchbox => this._saveChangeSearchbox ??= new RelayCommand(PerformSaveChangeSearchbox);

        private Boolean _enableWheelVolume;

        public Boolean EnableWheelVolume
        {
            get => this._enableWheelVolume;
            set
            {
                if (SetProperty(ref this._enableWheelVolume, value))
                {
                    HooksHelper.Instance.WheelVolumeHookHelper.IsEnabled = EnableWheelVolume;
                }
            }
        }

        public TaskbarViewModel()
        {
            this._task = Ioc.Default.GetServices<IBackgroundTask>().FirstOrDefault(x => x.Name == typeof(SearchboxTask).FullName);

            MusicPlayers = new ObservableCollection<MusicPlayer>();
            InitMenu();

            SelectedPlayer = MusicPlayers.FirstOrDefault(x => x.Name == ConfigHelper.CurrentConfig.Taskbar.MusicPlayer);
            CompatibleStartIsBack = ConfigHelper.CurrentConfig.Taskbar.CompatibleStartIsBack;
            SearchBoxTextDefault = ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextDefault;
            EnableMusicUpdate = ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable;

            EnableWheelVolume = ConfigHelper.CurrentConfig.Taskbar.WheelVolumeEnable;
        }

        private void InitMenu()
        {
            var qqMusic = new QQMusicPlayer();
            MusicPlayers.Add(qqMusic);

            var cloudMusic = new CloudMusicPlayer();
            MusicPlayers.Add(cloudMusic);
        }

        private void PerformSaveChangeSearchbox()
        {
            ConfigHelper.CurrentConfig.Taskbar.MusicPlayer = SelectedPlayer?.Name;
            ConfigHelper.CurrentConfig.Taskbar.CompatibleStartIsBack = CompatibleStartIsBack;
            ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextDefault = SearchBoxTextDefault;
            ConfigHelper.CurrentConfig.Taskbar.SearchBoxTextUpdateEnable = EnableMusicUpdate;

            if (EnableMusicUpdate)
            {
                this._task?.Run();
            }
            else
            {
                this._task?.Stop();
            }

            if (this._task is SearchboxTask st)
            {
                st.ChangeSettings(SelectedPlayer, SearchBoxTextDefault, CompatibleStartIsBack);
            }
        }
    }
}