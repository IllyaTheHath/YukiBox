using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages;

    public PageService()
    {
        _pages = [];
    }

    public Type GetPageType(string key)
    {
        Type pageType = null;
        lock (_pages)
        {
            _pages.TryGetValue(key, out pageType);
        }

        return pageType;
    }

    public Page GetPage(string key)
    {
        Type pageType = GetPageType(key);
        if (pageType is null)
        {
            return null;
        }
        Page page = Ioc.Default.GetService(pageType) as Page;
        return page;
    }

    public void Configure<TViewModel, TView>()
        where TViewModel : ViewModelBase
        where TView : Page
    {
        lock (_pages)
        {
            string key = typeof(TViewModel).FullName;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            Type type = typeof(TView);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
