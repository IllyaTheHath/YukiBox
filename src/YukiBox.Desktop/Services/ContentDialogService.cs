using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Services;

public class ContentDialogService : IContentDialogService
{
    private XamlRoot _mainXamlRoot;

    public void SetMainXamlRoot(XamlRoot mainXamlRoot)
    {
        _mainXamlRoot = mainXamlRoot;
    }

    public async Task<ContentDialogResult> ShowAsync(string title, string content, string closeButtonText, string primaryButtonText = null, string secondaryButtonText = null)
    {
        var dialog = new ContentDialog()
        {
            Title = title,
            Content = content,
            CloseButtonText = closeButtonText,
            PrimaryButtonText = primaryButtonText,
            SecondaryButtonText = secondaryButtonText,
            XamlRoot = _mainXamlRoot
        };
        return await dialog.ShowAsync();
    }

    public async Task<ContentDialogResult> ShowAsync(ContentDialog dialog)
    {
        dialog.XamlRoot = _mainXamlRoot;
        return await dialog.ShowAsync();
    }
}
