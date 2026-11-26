using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop.Views;

/// <summary>
/// Interaction logic for AudioEnhancementView.xaml
/// </summary>
public partial class AudioEnhancementView : Page
{
    public AudioEnhancementViewModel ViewModel => ViewModelLocator.Current.AudioEnhancementViewModel;

    public AudioEnhancementView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
