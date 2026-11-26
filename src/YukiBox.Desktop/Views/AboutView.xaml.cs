using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop.Views;

/// <summary>
/// Interaction logic for AboutView.xaml
/// </summary>
public partial class AboutView : Page
{
    public AboutViewModel ViewModel => ViewModelLocator.Current.AboutViewModel;

    public AboutView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
