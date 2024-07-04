using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Contracts.Views;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Interop;
using YukiBox.Desktop.Models;
using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window, IShellWindow
    {
        public ShellViewModel ViewModel => ViewModelLocator.Current.ShellViewModel;

        public ShellWindow()
        {
            InitializeComponent();
            this.navigationView.DataContext = ViewModel;

            // WinUI 3 doesn't provide API to change window icon, so using Win32 API here
            //this.SetIcon(@"Assets\Images\logo.ico");

            var appWindow = this.GetAppWindow();
            appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            SetTitleBar(this.appTitleBar);

            Closed += ShellWindow_Closed;
            (Content as FrameworkElement).Loaded += ShellWindow_Loaded;
        }

        private void ShellWindow_Loaded(Object sender, RoutedEventArgs e)
        {
            // Set ContentDialog XamlRoot here
            var contentDialogService = Ioc.Default.GetService<IContentDialogService>();
            contentDialogService.SetMainXamlRoot(Content.XamlRoot);
        }

        private void ShellWindow_Closed(Object sender, WindowEventArgs args)
        {
            this.HideWindow();
            if (!App.Exiting)
            {
                args.Handled = true;
            }
        }

        public Frame GetNavigationFrame()
        {
            return this.shellFrame;
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
}