using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Services;

public class NavigationService : INavigationService
{
    private Frame _frame;
    private object _lastParameterUsed;

    private readonly IPageService _pageService;

    public string CurrentPageKey
    {
        get
        {
            if (_frame.Content is FrameworkElement element)
            {
                return element.DataContext.GetType().FullName;
            }

            return string.Empty;
        }
    }

    public NavigationService(IPageService pageService)
    {
        _pageService = pageService;
    }

    public void Initialize(Frame shellFrame)
    {
        _frame = shellFrame;
    }

    public void NavigateTo(string pageKey)
    {
        NavigateTo(pageKey, null);
    }

    public void NavigateTo(string pageKey, object parameter)
    {
        if (_frame is null)
        {
            return;
        }

        var pageType = _pageService.GetPageType(pageKey);

        if (pageType is null)
        {
            return;
        }

        if (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed)))
        {
            var page = _pageService.GetPage(pageKey);
            if (page is null)
            {
                return;
            }
            bool navigated = _frame.Navigate(page.GetType(), parameter);
            if (navigated)
            {
                _lastParameterUsed = parameter;
            }
        }
    }
}
