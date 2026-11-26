using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;

using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Hooks;
using YukiBox.Desktop.Models;
using YukiBox.Desktop.Tasks;

namespace YukiBox.Desktop.ViewModels;

public class TaskbarViewModel : ViewModelBase
{
    private readonly IBackgroundTask _task;

    public ObservableCollection<MusicPlayer> MusicPlayers { get; set; }

    private MusicPlayer _selectedPlayer;

    public MusicPlayer SelectedPlayer
    {
        get => _selectedPlayer;
        set => SetProperty(ref _selectedPlayer, value);
    }

    private bool _enableMusicUpdate;

    public bool EnableMusicUpdate
    {
        get => _enableMusicUpdate;
        set => SetProperty(ref _enableMusicUpdate, value);
    }

    private bool _compatibleStartIsBack;

    public bool CompatibleStartIsBack
    {
        get => _compatibleStartIsBack;
        set => SetProperty(ref _compatibleStartIsBack, value);
    }

    private string _searchBoxTextDefault;

    public string SearchBoxTextDefault
    {
        get => _searchBoxTextDefault;
        set => SetProperty(ref _searchBoxTextDefault, value);
    }

    private ICommand _saveChangeSearchbox;

    public ICommand SaveChangeSearchbox => _saveChangeSearchbox ??= new RelayCommand(PerformSaveChangeSearchbox);

    private bool _enableWheelVolume;

    public bool EnableWheelVolume
    {
        get => _enableWheelVolume;
        set
        {
            if (SetProperty(ref _enableWheelVolume, value))
            {
                HooksHelper.Instance.WheelVolumeHookHelper.IsEnabled = EnableWheelVolume;
            }
        }
    }

    public TaskbarViewModel()
    {
        _task = Ioc.Default.GetServices<IBackgroundTask>().FirstOrDefault(x => x.Name == typeof(SearchboxTask).FullName);

        MusicPlayers = [];
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
            _task?.Run();
        }
        else
        {
            _task?.Stop();
        }

        if (_task is SearchboxTask st)
        {
            st.ChangeSettings(SelectedPlayer, SearchBoxTextDefault, CompatibleStartIsBack);
        }
    }
}
