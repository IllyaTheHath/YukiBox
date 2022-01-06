using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace YukiBox.Desktop.Contracts.Services
{
    public interface IContentDialogService
    {
        void SetMainXamlRoot(XamlRoot mainXamlRoot);
        Task<ContentDialogResult> ShowAsync(String title, String content, String closeButtonText, String primaryButtonText = null, String secondaryButtonText = null);
        Task<ContentDialogResult> ShowAsync(ContentDialog dialog);
    }
}
