using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop.Services
{
    public class PageService : IPageService
    {
        private readonly Dictionary<String, Type> _pages;

        public PageService()
        {
            this._pages = new();
        }

        public Type GetPageType(String key)
        {
            Type pageType = null;
            lock (this._pages)
            {
                this._pages.TryGetValue(key, out pageType);
            }

            return pageType;
        }

        public Page GetPage(String key)
        {
            var pageType = GetPageType(key);
            if (pageType is null)
            {
                return null;
            }
            return Ioc.Default.GetService(pageType) as Page;
        }

        public void Configure<ViewModel, View>()
            where ViewModel : ViewModelBase
            where View : Page
        {
            lock (this._pages)
            {
                var key = typeof(ViewModel).FullName;
                if (this._pages.ContainsKey(key))
                {
                    throw new ArgumentException($"The key {key} is already configured in PageService");
                }

                var type = typeof(View);
                if (this._pages.ContainsValue(type))
                {
                    throw new ArgumentException($"This type is already configured with key {this._pages.First(p => p.Value == type).Key}");
                }

                this._pages.Add(key, type);
            }
        }
    }
}