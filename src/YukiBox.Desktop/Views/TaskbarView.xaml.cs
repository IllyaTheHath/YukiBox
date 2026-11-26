using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop.Views;

/// <summary>
/// Interaction logic for HomeView.xaml
/// </summary>
public partial class TaskbarView : Page
{
    public TaskbarViewModel ViewModel => ViewModelLocator.Current.TaskbarViewModel;

    public TaskbarView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
