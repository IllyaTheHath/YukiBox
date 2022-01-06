using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

using Windows.ApplicationModel;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Controls;
using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        private readonly IContentDialogService _contentDialogService;

        private Boolean _enableStartUp;

        public Boolean EnableStartUp
        {
            get => this._enableStartUp;
            set
            {
                if (SetProperty(ref this._enableStartUp, value))
                {
                    SetToggleEnableStartUp(this._enableStartUp);
                }
                async void SetToggleEnableStartUp(Boolean flag)
                {
                    var success = await ToggleEnableStartUp(value);
                    SetProperty(ref this._enableStartUp, await GetEnableStartUp());
                }
            }
        }

        public SettingViewModel()
        {
            this._contentDialogService = Ioc.Default.GetService<IContentDialogService>();

            InitGetEnableStartUp();

            async void InitGetEnableStartUp() => EnableStartUp = await GetEnableStartUp();
        }

        private ICommand _resetSettingCommand;
        public ICommand ResetSettingCommand => this._resetSettingCommand ??= new AsyncRelayCommand(ResetSetting);

        private ICommand _appExitCommand;
        public ICommand AppExitCommand => this._appExitCommand ??= new RelayCommand(ExitApp);

        private async Task ResetSetting()
        {
            await ConfigHelper.Clear();
            await this._contentDialogService.ShowAsync(App.AppDisplayName, I18NSource.Instance["System.Restart"], I18NSource.Instance["MsgBox.Ok"]);
        }

        private void ExitApp()
        {
            App.Exit();
        }

        private async Task<Boolean> GetEnableStartUp()
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

        private async Task<Boolean> ToggleEnableStartUp(Boolean enableStartUp)
        {
            try
            {
                var startupTask = await StartupTask.GetAsync(App.AppName);
                if (enableStartUp)
                {
                    if (startupTask.State == StartupTaskState.DisabledByUser)
                    {
                        await this._contentDialogService.ShowAsync(
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
                await this._contentDialogService.ShowAsync(
                            App.AppDisplayName,
                            I18NSource.Instance["System.RunAtStartUp.Error"],
                            I18NSource.Instance["MsgBox.Ok"]);
                return false;
            }
        }
    }
}