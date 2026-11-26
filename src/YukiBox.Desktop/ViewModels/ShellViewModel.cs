using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.DependencyInjection;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.ViewModels;

public class ShellViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    public ObservableCollection<NavMenuItemBase> NavMenuItems { get; set; }
    public ObservableCollection<NavMenuItemBase> FooterNavMenuItems { get; set; }

    private NavMenuItem _selectedItem;

    public NavMenuItem SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }

    public ShellViewModel()
    {
        _navigationService = Ioc.Default.GetService<INavigationService>();
        NavMenuItems = [];
        FooterNavMenuItems = [];

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
        string pageKey = nav.TargetType.FullName;
        _navigationService.NavigateTo(pageKey);
    }
}
