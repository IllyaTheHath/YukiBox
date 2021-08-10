using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml;

using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.Controls
{
    /// <summary>
    /// Interaction logic for FluentMessageBoxWindow.xaml
    /// </summary>
    public partial class FluentMessageBoxWindow : Window
    {
        public String Glyph { get; private set; }

        public String MessageBoxText { get; private set; }

        public Visibility IconVisibility { get; private set; }

        public Visibility BtnOkVisibility { get; private set; }

        public Visibility BtnYesVisibility { get; private set; }

        public Visibility BtnNoVisibility { get; private set; }

        public Visibility BtnCancelVisibility { get; private set; }

        public MessageBoxResult Result { get; private set; }

        private ICommand _btnCommand;
        public ICommand BtnCommand => this._btnCommand ??= new RelayCommand<String>(BtnClick);

        private FluentMessageBoxWindow()
        {
            InitializeComponent();
            panel.DataContext = this;
        }

        public FluentMessageBoxWindow(
            Window owner,
            String messageBoxText,
            String caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult) : this()
        {
            MessageBoxText = messageBoxText;
            Title = !String.IsNullOrEmpty(caption) ? caption : App.AppDisplayName;

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
                    Result = MessageBoxResult.No;
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