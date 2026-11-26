using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop.Views;

/// <summary>
/// Interaction logic for SettingView.xaml
/// </summary>
public partial class SettingView : Page
{
    public SettingViewModel ViewModel => ViewModelLocator.Current.SettingViewModel;

    public SettingView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
