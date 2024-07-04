using System;
using System.Drawing;

using CommunityToolkit.Mvvm.Input;

using H.NotifyIcon;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Services
{
    public class TrayIconService : ITrayIconService
    {
        //private NotifyIcon.Icon _icon;
        //private NotifyIcon.NotifyIcon _notifyIcon;

        private TaskbarIcon _taskbarIcon;

        public void Initialize()
        {
            //var ico = NotifyIcon.Icon.create(@"Assets\Images\logo.ico");
            //if (ico.IsOk)
            //{
            //    this._icon = ico.ResultValue;
            //    this._notifyIcon = NotifyIcon.NotifyIcon.create();
            //    this._notifyIcon.setIcon(this._icon);
            //    this._notifyIcon.setTooltip(App.AppDisplayName);

            //    this._notifyIcon.onMouseLeftButtonDoubleClick += (s, a) =>
            //    {
            //        AppStartup.Instance.ShowShellWindow();
            //    };
            //}

            var settingMenuItem = new MenuFlyoutItem()
            {
                Icon = new SymbolIcon(Symbol.Setting),
                Text = I18NSource.Instance["TrayIcon.Main"],
                Command = new RelayCommand(() =>
                {
                    AppStartup.Instance.ShowShellWindow();
                })
                // ToolTip = I18N["TrayIcon.Main.Tooltip"]
            };
            var exitMenuItem = new MenuFlyoutItem()
            {
                Icon = new SymbolIcon(Symbol.Cancel),
                Text = I18NSource.Instance["TrayIcon.Exit"],
                Command = new RelayCommand(() =>
                {
                    AppStartup.Instance.Exit();
                })
                // ToolTip = I18N["TrayIcon.Exit.Tooltip"]
            };

            var contextMenu = new MenuFlyout();
            contextMenu.Items.Add(settingMenuItem);
            contextMenu.Items.Add(exitMenuItem);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri("ms-appx:///Assets/Images/logo.ico");

            //var iconBitmap = new BitmapImage(new Uri(@"Assets\Images\logo.ico"));

            _taskbarIcon = new();
            _taskbarIcon.ContextFlyout = contextMenu;
            _taskbarIcon.IconSource = bitmapImage;
            _taskbarIcon.ContextMenuMode = ContextMenuMode.SecondWindow;
            _taskbarIcon.DoubleClickCommand = new RelayCommand(() =>
            {
                AppStartup.Instance.ShowShellWindow();
            });

            if (!_taskbarIcon.IsCreated)
            {
                _taskbarIcon.ForceCreate();
            }
        }

        public void Dispose()
        {
            //this._notifyIcon.Dispose();
            //this._icon.Dispose();
            this._taskbarIcon.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}