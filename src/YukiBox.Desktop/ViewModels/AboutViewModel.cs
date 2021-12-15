using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IMediatorService _mediatorService;

        public ObservableCollection<AboutListItem> AboutLists { get; set; }
        public ObservableCollection<AboutThirdPartyItem> ThirdParties { get; set; }

        public AboutViewModel()
        {
            this._navigationService = Ioc.Default.GetService<INavigationService>();
            this._mediatorService = Ioc.Default.GetService<IMediatorService>();

            this._mediatorService.Register(this, "I18N", OnLocaleChange);

            AboutLists = new ObservableCollection<AboutListItem>();
            ThirdParties = new ObservableCollection<AboutThirdPartyItem>();

            InitMenu();
        }

        private void InitMenu()
        {
            AboutLists.Clear();
            ThirdParties.Clear();

            var github = new AboutListItem("Github", I18NSource.Instance["System.About.Github.Des"], FontIconSymbol.FavoriteStar, true, "https://github.com/IllyaTheHath/YukiBox");
            var feedback = new AboutListItem(I18NSource.Instance["System.About.Feedback"], I18NSource.Instance["System.About.Feedback.Des"], FontIconSymbol.Feedback, true, "https://github.com/IllyaTheHath/YukiBox/issues");
            AboutLists.Add(github);
            AboutLists.Add(feedback);

            var notifyIcon = new AboutThirdPartyItem("Hardcodet.NotifyIcon.Wpf", "https://github.com/hardcodet/wpf-notifyicon", "https://github.com/hardcodet/wpf-notifyicon/blob/master/LICENSE");
            var mvvm = new AboutThirdPartyItem("Microsoft.Toolkit.Mvvm", "https://github.com/windows-toolkit/WindowsCommunityToolkit", "https://github.com/windows-toolkit/WindowsCommunityToolkit/blob/master/LICENSE");
            var behavior = new AboutThirdPartyItem("Microsoft.Xaml.Behaviors.Wpf", "https://github.com/microsoft/XamlBehaviorsWpf", "https://github.com/microsoft/XamlBehaviorsWpf/blob/master/LICENSE");
            var modernWpf = new AboutThirdPartyItem("ModernWpf", "https://github.com/Kinnara/ModernWpf", "https://github.com/Kinnara/ModernWpf/blob/master/LICENSE");
            var pinvoke = new AboutThirdPartyItem("PInvoke.User32", "https://github.com/dotnet/pinvoke", "https://github.com/dotnet/pinvoke/blob/master/LICENSE");
            var naudio = new AboutThirdPartyItem("NAudio", "https://github.com/naudio/NAudio", "https://github.com/naudio/NAudio/blob/master/license.txt");
            ThirdParties.Add(notifyIcon);
            ThirdParties.Add(mvvm);
            ThirdParties.Add(behavior);
            ThirdParties.Add(modernWpf);
            ThirdParties.Add(pinvoke);
            ThirdParties.Add(naudio);
        }

        private void OnLocaleChange(Object obj)
        {
            InitMenu();
        }



        public void Dispose()
        {
            this._mediatorService.UnRegister(this, "I18N");
            GC.SuppressFinalize(this);
        }
    }
}