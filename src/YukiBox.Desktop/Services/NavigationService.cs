using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Services
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;
        private Object _lastParameterUsed;

        private readonly IPageService _pageService;

        public String CurrentPageKey
        {
            get
            {
                if (this._frame.Content is FrameworkElement element)
                {
                    return element.DataContext.GetType().FullName;
                }

                return String.Empty;
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

        public void NavigateTo(String pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(String pageKey, Object parameter)
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