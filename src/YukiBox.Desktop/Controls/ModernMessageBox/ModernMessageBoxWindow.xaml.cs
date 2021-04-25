using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

using Microsoft.Toolkit.Mvvm.Input;

using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.Controls
{
    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class ModernMessageBoxWindow : Window
    {
        public String Glyph { get; set; }

        public String MessageBoxText { get; init; }

        public Visibility IconVisibility { get; init; }

        public Visibility BtnOkVisibility { get; init; }

        public Visibility BtnYesVisibility { get; init; }

        public Visibility BtnNoVisibility { get; init; }

        public Visibility BtnCancelVisibility { get; init; }

        public MessageBoxResult Result { get; private set; }

        private ICommand btnCommand;

        public ICommand BtnCommand
        {
            get
            {
                if (this.btnCommand is null)
                {
                    this.btnCommand = new RelayCommand<String>(BtnClick);
                }
                return this.btnCommand;
            }
        }

        private ModernMessageBoxWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ModernMessageBoxWindow(
            Window owner,
            String messageBoxText,
            String caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult) : this()
        {
            if (owner is not null)
            {
                Owner = owner;
            }
            WindowStartupLocation = Owner is null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
            MessageBoxText = messageBoxText;
            Title = !String.IsNullOrEmpty(caption) ? caption : Program.AppDisplayName;

            IconVisibility = icon is MessageBoxImage.None ? Visibility.Collapsed : Visibility.Visible;
            Glyph = icon switch
            {
                MessageBoxImage.None => default,
                MessageBoxImage.Error => FontIconSymbol.Error,
                MessageBoxImage.Question => FontIconSymbol.Help,
                MessageBoxImage.Exclamation => FontIconSymbol.Warning,
                MessageBoxImage.Asterisk => FontIconSymbol.Info,
                _ => default
            };

            BtnOkVisibility = button is MessageBoxButton.OK or MessageBoxButton.OKCancel ? Visibility.Visible : Visibility.Collapsed;
            BtnYesVisibility = button is MessageBoxButton.YesNo or MessageBoxButton.YesNoCancel ? Visibility.Visible : Visibility.Collapsed;
            BtnNoVisibility = button is MessageBoxButton.YesNo or MessageBoxButton.YesNoCancel ? Visibility.Visible : Visibility.Collapsed;
            BtnCancelVisibility = button is MessageBoxButton.OKCancel or MessageBoxButton.YesNoCancel ? Visibility.Visible : Visibility.Collapsed;

            Result = defaultResult;
        }

        private void BtnClick(String btn)
        {
            switch (btn)
            {
                case "OK":
                    Result = MessageBoxResult.OK;
                    Close();
                    break;

                case "YES":
                    Result = MessageBoxResult.Yes;
                    Close();
                    break;

                case "NO":
                    Result = MessageBoxResult.None;
                    Close();
                    break;

                case "CANCEL":
                    Result = MessageBoxResult.Cancel;
                    Close();
                    break;
            }
        }
    }
}