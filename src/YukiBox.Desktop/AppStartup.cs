using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Contracts.Views;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Hooks;
using YukiBox.Desktop.Services;
using YukiBox.Desktop.ViewModels;
using YukiBox.Desktop.Views;

namespace YukiBox.Desktop;

public class AppStartup
{
    private static readonly Lazy<AppStartup> s_lazy = new(() => new AppStartup());

    public static AppStartup Instance => s_lazy.Value;

    private INavigationService _navigationService;
    private IPageService _pageService;
    private ITrayIconService _trayIconService;

    private IShellWindow _shellWindow;

    private IServiceProvider _services;

    private AppStartup()
    {
    }

    public void Initialize()
    {
        _services = ConfigureServices();
        Ioc.Default.ConfigureServices(_services);
    }

    public void OnStartup()
    {
        ConfigurePages();

        I18NSource.Instance.Initialize();

        _trayIconService = Ioc.Default.GetService<ITrayIconService>();
        _trayIconService.Initialize();

        HooksHelper.Instance.Initialize();
    }

    public void OnExit()
    {
        _trayIconService?.Dispose();

        HooksHelper.Instance.Dispose();
    }

    public void ShowShellWindow()
    {
        if (_shellWindow is null)
        //if (!App.Current.Windows.OfType<IShellWindow>().Any())
        {
            _navigationService = Ioc.Default.GetService<INavigationService>();
            _shellWindow = Ioc.Default.GetService<IShellWindow>();
            //_shellWindow = new ShellWindow();
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
            _shellWindow.Show();
            _navigationService.NavigateTo(typeof(AboutViewModel).FullName);
        }
        else
        {
            _shellWindow.Show();
        }
    }

    public void Exit()
    {
        App.Exit();
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

        ServiceProvider serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }

    private void ConfigurePages()
    {
        _pageService = Ioc.Default.GetService<IPageService>();

        _pageService.Configure<TaskbarViewModel, TaskbarView>();

        _pageService.Configure<AboutViewModel, AboutView>();
        _pageService.Configure<SettingViewModel, SettingView>();
    }
}
