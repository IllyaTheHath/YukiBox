﻿using System;
using System.Diagnostics;
using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Contracts.Views;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Hooks;
using YukiBox.Desktop.Interop;
using YukiBox.Desktop.Services;
using YukiBox.Desktop.Tasks;
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
            services.AddSingleton<IFileStoreService, FileStoreService>();
            services.AddSingleton<IContentDialogService, ContentDialogService>();
            //services.AddSingleton<IConfigService, ConfigService>();

            // Configure Shell Window
            services.AddSingleton<IShellWindow, ShellWindow>();
            services.AddSingleton<ShellViewModel>();

            // Configure View & ViewModel
            services.AddSingleton<TaskbarView>();
            services.AddSingleton<TaskbarViewModel>();

            services.AddSingleton<AboutView>();
            services.AddSingleton<AboutViewModel>();

            services.AddSingleton<SettingView>();
            services.AddSingleton<SettingViewModel>();

            // Configure Background Task
            services.AddSingleton<IBackgroundTask, SearchboxTask>();

            return services.BuildServiceProvider();
        }

        public void OnStartup()
        {
            ConfigurePages();

            I18NSource.Instance.Initialize();

            this._trayIconService = Ioc.Default.GetService<ITrayIconService>();
            this._trayIconService.Initialize();

            var tasks = Ioc.Default.GetServices<IBackgroundTask>();
            foreach (var task in tasks)
            {
                task.Init();
            }

            HooksHelper.Instance.Initialize();
        }

        public void OnExit()
        {
            this._trayIconService?.Dispose();

            var tasks = Ioc.Default.GetServices<IBackgroundTask>();
            foreach (var task in tasks)
            {
                task.Dispose();
            }

            HooksHelper.Instance.Dispose();
        }

        private void ConfigurePages()
        {
            this._pageService = Ioc.Default.GetService<IPageService>();

            this._pageService.Configure<TaskbarViewModel, TaskbarView>();

            this._pageService.Configure<AboutViewModel, AboutView>();
            this._pageService.Configure<SettingViewModel, SettingView>();
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
                this._shellWindow.Show();
                this._navigationService.NavigateTo(typeof(AboutViewModel).FullName);
            }
            else
            {
                this._shellWindow.Show();
            }
        }

        public void Exit()
        {
            App.Exit();
        }
    }
}