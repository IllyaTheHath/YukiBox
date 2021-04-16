using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.Mvvm.DependencyInjection;

using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop.Helpers
{
    public class ViewModelLocator
    {
        private static readonly Lazy<ViewModelLocator> _current = new(() => new ViewModelLocator());

        public static ViewModelLocator Current { get { return _current.Value; } }

        //public ShellViewModel ShellViewModel => Ioc.Default.GetService<ShellViewModel>();

        public HomeViewModel HomeViewModel => Ioc.Default.GetService<HomeViewModel>();

        public AboutViewModel AboutViewModel => Ioc.Default.GetService<AboutViewModel>();
        public SettingViewModel SettingViewModel => Ioc.Default.GetService<SettingViewModel>();

        private ViewModelLocator()
        {
        }
    }
}
