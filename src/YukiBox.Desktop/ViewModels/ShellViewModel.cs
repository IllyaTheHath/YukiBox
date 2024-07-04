using System;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.DependencyInjection;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ObservableCollection<NavMenuItemBase> NavMenuItems { get; set; }
        public ObservableCollection<NavMenuItemBase> FooterNavMenuItems { get; set; }

        private NavMenuItem _selectedItem;

        public NavMenuItem SelectedItem
        {
            get => this._selectedItem;
            set => SetProperty(ref this._selectedItem, value);
        }

        public ShellViewModel()
        {
            this._navigationService = Ioc.Default.GetService<INavigationService>();
            NavMenuItems = new ObservableCollection<NavMenuItemBase>();
            FooterNavMenuItems = new ObservableCollection<NavMenuItemBase>();

            var taskbar = new NavMenuItem("Nav.Taskbar", "Nav.Taskbar.Tooltip", FontIconSymbol.SIPRedock, typeof(TaskbarViewModel));
            NavMenuItems.Add(taskbar);

            var about = new NavMenuItem("Nav.About", "Nav.About.Tooltip", FontIconSymbol.Info, typeof(AboutViewModel));
            var setting = new NavMenuItem("Nav.Setting", "Nav.Setting.Tooltip", FontIconSymbol.Setting, typeof(SettingViewModel));
            FooterNavMenuItems.Add(about);
            FooterNavMenuItems.Add(setting);

            SelectedItem = about;
        }

        public void TryNavigate(NavMenuItem nav)
        {
            var pageKey = nav.TargetType.FullName;
            this._navigationService.NavigateTo(pageKey);
        }
    }
}