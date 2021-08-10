using System;
using System.Windows.Controls;

using Hardcodet.Wpf.TaskbarNotification;

using Microsoft.Toolkit.Mvvm.Input;

using WinUIEx;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Services
{
    public class TrayIconService : ITrayIconService
    {
        private readonly IMediatorService _mediatorService;

        private TaskbarIcon _taskbarIcon;

        private MenuItem _settingMenuItem;
        private MenuItem _exitMenuItem;

        private ContextMenu _contextMenu;

        private ToolTip _toolTip;

        public TrayIconService(IMediatorService mediatorService)
        {
            this._mediatorService = mediatorService;
        }

        private void OnLocaleChange(Object obj)
        {
            this._settingMenuItem.Header = I18NSource.Instance["TrayIcon.Main"];
            this._settingMenuItem.ToolTip = I18NSource.Instance["TrayIcon.Main.Tooltip"];
            this._exitMenuItem.Header = I18NSource.Instance["TrayIcon.Exit"];
            this._exitMenuItem.ToolTip = I18NSource.Instance["TrayIcon.Exit.Tooltip"];
        }

        public void Initialize()
        {
            this._mediatorService.Register(this, "I18N", OnLocaleChange);

            this._contextMenu = new();
            this._contextMenu.Items.Clear();
            this._settingMenuItem = new MenuItem()
            {
                Icon = new Microsoft.UI.Xaml.Controls.SymbolIcon(Microsoft.UI.Xaml.Controls.Symbol.Setting),
                Header = I18NSource.Instance["TrayIcon.Main"],
                ToolTip = I18NSource.Instance["TrayIcon.Main.Tooltip"],
                Command = new RelayCommand(() =>
                {
                    AppStartup.Instance.ShowShellWindow();
                })
            };
            this._exitMenuItem = new MenuItem()
            {
                Icon = new Microsoft.UI.Xaml.Controls.SymbolIcon(Microsoft.UI.Xaml.Controls.Symbol.Cancel),
                Header = I18NSource.Instance["TrayIcon.Exit"],
                ToolTip = I18NSource.Instance["TrayIcon.Exit.Tooltip"],
                Command = new RelayCommand(() =>
                {
                    AppStartup.Instance.Exit();
                })
            };

            this._contextMenu.Items.Add(this._settingMenuItem);
            this._contextMenu.Items.Add(this._exitMenuItem);

            this._toolTip = new();
            this._toolTip.Content = App.AppDisplayName;

            this._taskbarIcon = new();
            this._taskbarIcon.ContextMenu = this._contextMenu;
            this._taskbarIcon.TrayToolTip = this._toolTip;
            this._taskbarIcon.DoubleClickCommand = new RelayCommand(() =>
            {
                AppStartup.Instance.ShowShellWindow();
            });

            //BitmapImage image = new BitmapImage();
            //image.UriSource = new Uri("ms-appx:///Assets/Images/logo.ico");
            //this._taskbarIcon.IconSource = image;

            //var iconUri = CommonUtils.GetAbsoluteUri(@"Assets/Images/logo.ico");
            //var iconUri = new Uri("ms-appx:///Assets/Images/logo.ico");
            //var names = App.Current.GetType().Assembly.GetManifestResourceNames();
            var stream = App.Current.GetType().Assembly.GetManifestResourceStream("YukiBox.Desktop.Assets.Images.logo.ico");
            //this._taskbarIcon.IconSource = BitmapFrame.Create(iconUri);
            this._taskbarIcon.Icon = new System.Drawing.Icon(stream);

            //ThemeManager.SetRequestedTheme(this._contextMenu, ElementTheme.Default);
            //ThemeManager.SetRequestedTheme(this._toolTip, ElementTheme.Default);
        }

        public void Dispose()
        {
            this._taskbarIcon?.Dispose();
            this._contextMenu = null;
            this._toolTip = null;
            GC.SuppressFinalize(this);
        }
    }
}