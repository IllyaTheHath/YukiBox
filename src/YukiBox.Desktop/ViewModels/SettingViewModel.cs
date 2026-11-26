using System.Windows.Input;

using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

using Windows.ApplicationModel;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.ViewModels;

public class SettingViewModel : ViewModelBase
{
    private readonly IContentDialogService _contentDialogService;

    private bool _enableStartUp;

    public bool EnableStartUp
    {
        get => _enableStartUp;
        set
        {
            if (SetProperty(ref _enableStartUp, value))
            {
                SetToggleEnableStartUp(_enableStartUp);
            }
            async void SetToggleEnableStartUp(bool flag)
            {
                bool success = await ToggleEnableStartUpAsync(value);
                SetProperty(ref _enableStartUp, await GetEnableStartUpAsync());
            }
        }
    }

    public SettingViewModel()
    {
        _contentDialogService = Ioc.Default.GetService<IContentDialogService>();

        InitGetEnableStartUp();

        async void InitGetEnableStartUp() => EnableStartUp = await GetEnableStartUpAsync();
    }

    private ICommand _resetSettingCommand;

    public ICommand ResetSettingCommand => _resetSettingCommand ??= new AsyncRelayCommand(ResetSettingAsync);

    private ICommand _appExitCommand;

    public ICommand AppExitCommand => _appExitCommand ??= new RelayCommand(ExitApp);

    private async Task ResetSettingAsync()
    {
        await ConfigHelper.ClearAsync();
        await _contentDialogService.ShowAsync(App.AppDisplayName, I18NSource.Instance["System.Restart"], I18NSource.Instance["MsgBox.Ok"]);
    }

    private void ExitApp()
    {
        App.Exit();
    }

    private async Task<bool> GetEnableStartUpAsync()
    {
        try
        {
            var startupTask = await StartupTask.GetAsync(App.AppName);
            var state = startupTask.State;
            return state == StartupTaskState.Enabled;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> ToggleEnableStartUpAsync(bool enableStartUp)
    {
        try
        {
            var startupTask = await StartupTask.GetAsync(App.AppName);
            if (enableStartUp)
            {
                if (startupTask.State == StartupTaskState.DisabledByUser)
                {
                    await _contentDialogService.ShowAsync(
                        App.AppDisplayName,
                        I18NSource.Instance["System.RunAtStartUp.Disabled"],
                        I18NSource.Instance["MsgBox.Ok"]);
                }
                await startupTask.RequestEnableAsync();
            }
            else
            {
                startupTask.Disable();
            }
            return true;
        }
        catch
        {
            await _contentDialogService.ShowAsync(
                        App.AppDisplayName,
                        I18NSource.Instance["System.RunAtStartUp.Error"],
                        I18NSource.Instance["MsgBox.Ok"]);
            return false;
        }
    }
}
