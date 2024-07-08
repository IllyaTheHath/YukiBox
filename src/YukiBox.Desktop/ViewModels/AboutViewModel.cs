using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public ObservableCollection<AboutListItem> AboutLists { get; set; }
        public ObservableCollection<AboutThirdPartyItem> ThirdParties { get; set; }

        public AboutViewModel()
        {
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

            var mvvm = new AboutThirdPartyItem("CommunityToolkit.Mvvm", "https://github.com/CommunityToolkit/dotnet", "https://github.com/CommunityToolkit/dotnet/blob/main/License.md");
            var behavior = new AboutThirdPartyItem("Microsoft.Xaml.Behaviors.WinUI", "https://github.com/Microsoft/XamlBehaviors", "https://github.com/microsoft/XamlBehaviors/blob/master/LICENSE");
            var pinvoke = new AboutThirdPartyItem("Microsoft.Windows.CsWin32", "https://github.com/microsoft/CsWin32", "https://github.com/microsoft/CsWin32/blob/main/LICENSE");
            var naudio = new AboutThirdPartyItem("NAudio", "https://github.com/naudio/NAudio", "https://github.com/naudio/NAudio/blob/master/license.txt");
            var hNotifyIcon = new AboutThirdPartyItem("H.NotifyIcon.WinUI", "https://github.com/HavenDV/H.NotifyIcon", "https://github.com/HavenDV/H.NotifyIcon/blob/master/LICENSE.md");
            var powertoys = new AboutThirdPartyItem("PowerToys", "https://github.com/microsoft/PowerToys", "https://github.com/microsoft/PowerToys/blob/main/LICENSE");
            ThirdParties.Add(mvvm);
            ThirdParties.Add(behavior);
            ThirdParties.Add(pinvoke);
            ThirdParties.Add(naudio);
            ThirdParties.Add(hNotifyIcon);
            ThirdParties.Add(powertoys);
        }
    }
}