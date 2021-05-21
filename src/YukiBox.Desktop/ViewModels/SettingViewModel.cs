using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using ModernWpf.Controls;

using Windows.ApplicationModel;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Controls;
using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        private readonly IConfigService _configService;

        public LanguageInfo CurrentLanguage
        {
            get => I18NSource.Instance.CurrentLanguage;
            set => I18NSource.Instance.CurrentLanguage = value;
        }

        public ObservableCollection<LanguageInfo> SupportedLanguages
        {
            get => I18NSource.Instance.SupportedLanguages;
        }

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

        public SettingViewModel(IConfigService configService)
        {
            this._configService = configService;
            InitGetEnableStartUp();

            async void InitGetEnableStartUp() => EnableStartUp = await GetEnableStartUp();
        }

        private ICommand _showMessageCommand;
        public ICommand ShowMessageCommand => this._showMessageCommand ??= new RelayCommand(ShowMessageBox);

        private ICommand _resetSettingCommand;
        public ICommand ResetSettingCommand => this._resetSettingCommand ??= new AsyncRelayCommand(ResetSetting);

        public void ShowMessageBox()
        {
            var result = FluentMessageBox.Show("test text", null, MessageBoxButton.YesNoCancel, MessageBoxImage.Error);
            FluentMessageBox.Show($"You clicked {result}", null, MessageBoxButton.YesNoCancel);
        }

        public async Task ResetSetting()
        {
            await this._configService.Clear();
            var dialog = new ContentDialog
            {
                Title = Program.AppDisplayName,
                Content = I18NSource.Instance["System.Restart"],
                CloseButtonText = I18NSource.Instance["MsgBox.Ok"]
            };
            await dialog.ShowAsync();
        }

        private async Task<Boolean> GetEnableStartUp()
        {
            try
            {
                var startupTask = await StartupTask.GetAsync(Program.AppName);
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
                var startupTask = await StartupTask.GetAsync(Program.AppName);
                if (enableStartUp)
                {
                    if (startupTask.State == StartupTaskState.DisabledByUser)
                    {
                        var dialog = new ContentDialog
                        {
                            Title = Program.AppDisplayName,
                            Content = I18NSource.Instance["System.RunAtStartUp.Disabled"],
                            CloseButtonText = I18NSource.Instance["MsgBox.Ok"]
                        };
                        await dialog.ShowAsync();
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
                var dialog = new ContentDialog
                {
                    Title = Program.AppDisplayName,
                    Content = I18NSource.Instance["System.RunAtStartUp.Error"],
                    CloseButtonText = I18NSource.Instance["MsgBox.Ok"]
                };
                await dialog.ShowAsync();
                return false;
            }
        }
    }
}