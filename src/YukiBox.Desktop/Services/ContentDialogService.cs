using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Services
{
    public class ContentDialogService : IContentDialogService
    {
        private XamlRoot _mainXamlRoot;

        public void SetMainXamlRoot(XamlRoot mainXamlRoot)
        {
            this._mainXamlRoot = mainXamlRoot;
        }

        public async Task<ContentDialogResult> ShowAsync(String title, String content, String closeButtonText, String primaryButtonText = null, String secondaryButtonText = null)
        {
            var dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                CloseButtonText = closeButtonText,
                PrimaryButtonText = primaryButtonText,
                SecondaryButtonText = secondaryButtonText,
                XamlRoot = this._mainXamlRoot
            };
            return await dialog.ShowAsync();
        }

        public async Task<ContentDialogResult> ShowAsync(ContentDialog dialog)
        {
            dialog.XamlRoot = this._mainXamlRoot;
            return await dialog.ShowAsync();
        }
    }
}
