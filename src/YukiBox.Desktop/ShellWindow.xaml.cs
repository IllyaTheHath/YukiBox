using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Contracts.Views;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Interop;
using YukiBox.Desktop.Models;
using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop;

/// <summary>
/// Interaction logic for ShellWindow.xaml
/// </summary>
public partial class ShellWindow : Window, IShellWindow
{
    public ShellViewModel ViewModel => ViewModelLocator.Current.ShellViewModel;

    public ShellWindow()
    {
        InitializeComponent();
        navigationView.DataContext = ViewModel;

        // WinUI 3 doesn't provide API to change window icon, so using Win32 API here
        //SetIcon(@"Assets\Images\logo.ico");

        AppWindow appWindow = this.GetAppWindow();
        appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        SetTitleBar(appTitleBar);

        Closed += (_, args) =>
        {
            this.HideWindow();
            if (!App.Exiting)
            {
                args.Handled = true;
            }
        };
        (Content as FrameworkElement).Loaded += (_, _) =>
        {
            // Set ContentDialog XamlRoot here
            IContentDialogService contentDialogService = Ioc.Default.GetService<IContentDialogService>();
            contentDialogService.SetMainXamlRoot(Content.XamlRoot);
        };
    }

    public Frame GetNavigationFrame()
    {
        return shellFrame;
    }

    public void Show()
    {
        //Activate();
        this.ShowWindow();
    }

    public void CloseWindow()
    {
        Close();
    }

    private void NavigateView(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavMenuItem nav)
        {
            ViewModel.TryNavigate(nav);
        }
    }
}
