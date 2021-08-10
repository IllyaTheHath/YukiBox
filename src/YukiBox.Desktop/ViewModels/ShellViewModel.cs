using System;
using System.Collections.ObjectModel;

using Microsoft.Toolkit.Mvvm.DependencyInjection;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.ViewModels
{
    public class ShellViewModel : ViewModelBase, IDisposable
    {
        private readonly INavigationService _navigationService;
        private readonly IMediatorService _mediatorService;

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
            this._mediatorService = Ioc.Default.GetService<IMediatorService>();

            this._mediatorService.Register(this, "I18N", OnLocaleChange);

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

        private void OnLocaleChange(Object obj)
        {
            foreach (NavMenuItem item in NavMenuItems)
            {
                if (item is NavMenuItem nav)
                {
                    nav.UpdateNameAndTooltip();
                }
            }
            foreach (NavMenuItem item in FooterNavMenuItems)
            {
                if (item is NavMenuItem nav)
                {
                    nav.UpdateNameAndTooltip();
                }
            }
        }

        public void TryNavigate(NavMenuItem nav)
        {
            var pageKey = nav.TargetType.FullName;
            this._navigationService.NavigateTo(pageKey);
        }

        public void Dispose()
        {
            this._mediatorService.UnRegister(this, "I18N");
            GC.SuppressFinalize(this);
        }
    }
}