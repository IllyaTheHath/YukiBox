using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using Hardcodet.Wpf.TaskbarNotification;

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;

using ModernWpf;
using ModernWpf.Controls;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Services
{
    public class TrayIconService : ITrayIconService
    {
        private readonly IMediatorService _mediatorService;

        private TaskbarIcon _taskbarIcon;

        private ContextMenu _contextMenu;

        private ToolTip _toolTip;

        public TrayIconService(IMediatorService mediatorService)
        {
            this._mediatorService = mediatorService;
        }

        private void OnLocaleChange(Object obj)
        {
            InitContextMenu();
        }

        private void InitContextMenu()
        {
            this._contextMenu.Items.Clear();
            var settingMenuItem = new MenuItem()
            {
                Icon = new SymbolIcon(Symbol.Setting),
                Header = I18NSource.Instance["TrayIcon.Main"],
                ToolTip = I18NSource.Instance["TrayIcon.Main.Tooltip"],
                Command = new RelayCommand(() =>
                {
                    AppStartup.Instance.ShowShellWindow();
                })
            };
            var exitMenuItem = new MenuItem()
            {
                Icon = new SymbolIcon(Symbol.Cancel),
                Header = I18NSource.Instance["TrayIcon.Exit"],
                ToolTip = I18NSource.Instance["TrayIcon.Exit.Tooltip"],
                Command = new RelayCommand(() =>
                {
                    AppStartup.Instance.Exit();
                })
            };
            this._contextMenu.Items.Add(settingMenuItem);
            this._contextMenu.Items.Add(exitMenuItem);
        }

        public void Initialize()
        {
            this._mediatorService.Register(this, "I18N", OnLocaleChange);

            this._contextMenu = new();
            InitContextMenu();

            this._toolTip = new();
            this._toolTip.Content = Program.AppDisplayName;

            this._taskbarIcon = new();
            this._taskbarIcon.ContextMenu = this._contextMenu;
            this._taskbarIcon.TrayToolTip = this._toolTip;
            this._taskbarIcon.DoubleClickCommand = new RelayCommand(() =>
            {
                AppStartup.Instance.ShowShellWindow();
            });

            var iconUri = CommonUtils.GetAbsoluteUri(@"Assets\Images\logo.ico");
            this._taskbarIcon.IconSource = BitmapFrame.Create(iconUri);

            ThemeManager.SetRequestedTheme(this._contextMenu, ElementTheme.Default);
            ThemeManager.SetRequestedTheme(this._toolTip, ElementTheme.Default);
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