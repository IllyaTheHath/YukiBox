using System;

using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);

        Page GetPage(string key);

        void Configure<VM, V>()
            where VM : ViewModelBase
            where V : Page;
    }
}