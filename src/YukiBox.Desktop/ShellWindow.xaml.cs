using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using YukiBox.Desktop.Contracts.Views;
using YukiBox.Desktop.Helpers;
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
            DataContext = ViewModel;
        }

        public ModernWpf.Controls.Frame GetNavigationFrame()
        {
            return this.shellFrame;
        }

        public void ShowWindow()
        {
            Show();
        }

        public void CloseWindow()
        {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void NavigateView(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            ViewModel.TryNavigate();
        }
    }
}