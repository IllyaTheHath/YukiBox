using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Helpers;
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

            InitMenu();
        }

        private void InitMenu(Type current = null)
        {
            NavMenuItems.Clear();
            FooterNavMenuItems.Clear();

            NavMenuItem home = new(I18NSource.Instance["Nav.Home"], I18NSource.Instance["Nav.Home.Tooltip"], FontIconSymbol.Home, typeof(HomeViewModel));
            NavMenuItems.Add(home);

            NavMenuItem about = new(I18NSource.Instance["Nav.About"], I18NSource.Instance["Nav.Home.Tooltip"], FontIconSymbol.Info, typeof(AboutViewModel));
            NavMenuItem setting = new(I18NSource.Instance["Nav.Setting"], I18NSource.Instance["Nav.Setting.Tooltip"], FontIconSymbol.Setting, typeof(SettingViewModel));
            FooterNavMenuItems.Add(about);
            FooterNavMenuItems.Add(setting);

            if (current is not null)
            {
                var item = NavMenuItems.Union(FooterNavMenuItems).FirstOrDefault(x =>
                {
                    if (x is NavMenuItem nav)
                    {
                        if (nav.TargetType == current)
                        {
                            return true;
                        }
                    }
                    return false;
                });
                SelectedItem = item as NavMenuItem;
            }
            else
            {
                SelectedItem = home;
            }
        }

        private void OnLocaleChange(Object obj)
        {
            InitMenu(SelectedItem.TargetType);
        }

        public void TryNavigate()
        {
            var pageKey = SelectedItem.TargetType.FullName;
            this._navigationService.NavigateTo(pageKey);
        }

        public void Dispose()
        {
            this._mediatorService.UnRegister(this, "I18N");
            GC.SuppressFinalize(this);
        }
    }
}