using System.Diagnostics;
using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

namespace YukiBox.Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public const string AppName = "YukiBox.Desktop";
    public const string AppDisplayName = "YukiBox";
    public const string AppUuid = "B19A8370-3BD2-452F-851D-7A0058EC35AC";

    public static bool Exiting { get; private set; }

    public static string AppVersion
    {
        get => Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }

    public App()
    {
        AppActivationArguments appArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
        AppInstance instance = AppInstance.FindOrRegisterForKey(AppUuid);
        if (!instance.IsCurrent)
        {
            instance.RedirectActivationToAsync(appArgs).GetResults();
            Process.GetCurrentProcess().Kill();
            return;
        }

        InitializeComponent();

        AppStartup.Instance.Initialize();
    }

    /// <summary>
    /// Override App Exit
    /// </summary>
    public static new void Exit()
    {
        Exiting = true;

        AppStartup.Instance.OnExit();

        App.Current.Exit();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        AppStartup.Instance.OnStartup();
    }
}
