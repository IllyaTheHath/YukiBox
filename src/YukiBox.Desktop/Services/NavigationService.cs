using System;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Services
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;
        private object _lastParameterUsed;

        private readonly IPageService _pageService;

        public string CurrentPageKey
        {
            get
            {
                if (this._frame.Content is FrameworkElement element)
                {
                    return element.DataContext.GetType().FullName;
                }

                return string.Empty;
            }
        }

        public NavigationService(IPageService pageService)
        {
            this._pageService = pageService;
        }

        public void Initialize(Frame shellFrame)
        {
            this._frame = shellFrame;
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            if (this._frame is null)
            {
                return;
            }

            var pageType = this._pageService.GetPageType(pageKey);

            if (pageType is null)
            {
                return;
            }

            if (this._frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(this._lastParameterUsed)))
            {
                var page = this._pageService.GetPage(pageKey);
                if (page is null)
                {
                    return;
                }
                var navigated = this._frame.Navigate(page.GetType(), parameter);
                if (navigated)
                {
                    this._lastParameterUsed = parameter;
                }
            }
        }
    }
}