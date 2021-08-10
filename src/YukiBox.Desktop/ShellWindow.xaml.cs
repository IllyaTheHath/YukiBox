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

using Windows.Foundation;
using Windows.Foundation.Collections;

using WinUIEx;

using YukiBox.Desktop.Contracts.Views;
using YukiBox.Desktop.Helpers;
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
        //public ShellViewModel ViewModel => new ();

        public ShellWindow()
        {
            InitializeComponent();
            navigationView.DataContext = ViewModel;

            this.Closed += ShellWindow_Closed;
        }

        private void ShellWindow_Closed(object sender, WindowEventArgs args)
        {
            this.HideWindow();
            args.Handled = true;
        }

        public Frame GetNavigationFrame()
        {
            return this.shellFrame;
        }

        public void Show()
        {
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