using System;

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Services
{
    public class TrayIconService : ITrayIconService
    {
        private NotifyIcon.Icon _icon;
        private NotifyIcon.NotifyIcon _notifyIcon;

        public void Initialize()
        {
            var ico = NotifyIcon.Icon.create(@"Assets\Images\logo.ico");
            if (ico.IsOk)
            {
                this._icon = ico.ResultValue;
                this._notifyIcon = NotifyIcon.NotifyIcon.create();
                this._notifyIcon.setIcon(this._icon);
                this._notifyIcon.setTooltip(App.AppDisplayName);

                this._notifyIcon.onMouseLeftButtonDoubleClick += (s, a) =>
                {
                    AppStartup.Instance.ShowShellWindow();
                };
            }
        }

        public void Dispose()
        {
            this._notifyIcon.Dispose();
            this._icon.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}