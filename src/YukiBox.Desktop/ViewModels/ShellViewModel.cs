using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.DependencyInjection;

using YukiBox.Desktop.Contracts.Services;
using YukiBox.Desktop.Models;

namespace YukiBox.Desktop.ViewModels;

public class ShellViewModel : ViewModelBase
{
    public ObservableCollection<NavMenuItemBase> NavMenuItems { get; set; }
    public ObservableCollection<NavMenuItemBase> FooterNavMenuItems { get; set; }

    private NavMenuItem _selectedItem;

    public NavMenuItem SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }

    private readonly INavigationService _navigationService;

    public ShellViewModel()
    {
        _navigationService = Ioc.Default.GetService<INavigationService>();
        NavMenuItems = [];
        FooterNavMenuItems = [];

        var audioEnhancement = new NavMenuItem("Nav.AudioEnhancement", "Nav.AudioEnhancement.Tooltip", FontIconSymbol.Volume, typeof(AudioEnhancementViewModel));
        NavMenuItems.Add(audioEnhancement);

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
