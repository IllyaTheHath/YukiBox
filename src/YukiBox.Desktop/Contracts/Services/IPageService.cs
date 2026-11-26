using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);

    void Configure<TViewModel, TView>()
        where TViewModel : ViewModelBase
        where TView : Page;
}
