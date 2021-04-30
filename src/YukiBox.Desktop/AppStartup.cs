using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Contracts.Views;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Services;
using YukiBox.Desktop.ViewModels;
using YukiBox.Desktop.Views;

namespace YukiBox.Desktop
{
    public class AppStartup
    {
        private static readonly Lazy<AppStartup> lazy = new(() => new AppStartup());

        public static AppStartup Instance { get { return lazy.Value; } }

        private IServiceProvider _services;

        private INavigationService _navigationService;
        private IPageService _pageService;
        private ITrayIconService _trayIconService;

        private IShellWindow _shellWindow;

        private AppStartup()
        {
        }

        public void Initialize()
        {
            this._services = ConfigureServices();
            Ioc.Default.ConfigureServices(this._services);
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Configure Services
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ITrayIconService, TrayIconService>();
            services.AddSingleton<IMediatorService, MediatorService>();
            services.AddSingleton<ISearchboxService, SearchboxService>();
            services.AddSingleton<IFileStoreService, FileStoreService>();
            services.AddSingleton<IConfigService, ConfigService>();

            // Configure Shell Window
            services.AddSingleton<IShellWindow, ShellWindow>();
            services.AddSingleton<ShellViewModel>();

            // Configure View & ViewModel
            services.AddSingleton<HomeView>();
            services.AddSingleton<HomeViewModel>();

            services.AddSingleton<AboutView>();
            services.AddSingleton<AboutViewModel>();

            services.AddSingleton<SettingView>();
            services.AddSingleton<SettingViewModel>();

            return services.BuildServiceProvider();
        }

        public void OnStartup(StartupEventArgs e)
        {
            ConfigurePages();

            I18NSource.Instance.Initialize();

            HandleActivation();
        }

        public void OnExit(ExitEventArgs e)
        {
        }

        private void ConfigurePages()
        {
            this._pageService = Ioc.Default.GetService<IPageService>();

            this._pageService.Configure<HomeViewModel, HomeView>();

            this._pageService.Configure<AboutViewModel, AboutView>();
            this._pageService.Configure<SettingViewModel, SettingView>();
        }

        private void HandleActivation()
        {
            this._trayIconService = Ioc.Default.GetService<ITrayIconService>();
            this._trayIconService.Initialize();
        }

        public void ShowShellWindow()
        {
            if (this._shellWindow is null)
            //if (!App.Current.Windows.OfType<IShellWindow>().Any())
            {
                this._navigationService = Ioc.Default.GetService<INavigationService>();
                this._shellWindow = Ioc.Default.GetService<IShellWindow>();
                //this._shellWindow = new ShellWindow();
                this._navigationService.Initialize(this._shellWindow.GetNavigationFrame());
                this._shellWindow.ShowWindow();
                this._navigationService.NavigateTo(typeof(HomeViewModel).FullName);
            }
            else
            {
                this._shellWindow.ShowWindow();
            }
        }

        public void Exit()
        {
            App.Current.Shutdown();
        }
    }
}