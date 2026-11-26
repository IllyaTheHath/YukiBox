using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace YukiBox.Desktop.Contracts.Services;

public interface IContentDialogService
{
    void SetMainXamlRoot(XamlRoot mainXamlRoot);
    Task<ContentDialogResult> ShowAsync(string title, string content, string closeButtonText, string primaryButtonText = null, string secondaryButtonText = null);
    Task<ContentDialogResult> ShowAsync(ContentDialog dialog);
}
