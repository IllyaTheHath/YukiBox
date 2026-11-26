using CommunityToolkit.Mvvm.DependencyInjection;

using YukiBox.Desktop.ViewModels;

namespace YukiBox.Desktop.Helpers;

public class ViewModelLocator
{
    private static readonly Lazy<ViewModelLocator> s_current = new(() => new ViewModelLocator());

    public static ViewModelLocator Current => s_current.Value;

    public ShellViewModel ShellViewModel => Ioc.Default.GetService<ShellViewModel>();
    public TaskbarViewModel TaskbarViewModel => Ioc.Default.GetService<TaskbarViewModel>();
    public AboutViewModel AboutViewModel => Ioc.Default.GetService<AboutViewModel>();
    public SettingViewModel SettingViewModel => Ioc.Default.GetService<SettingViewModel>();

    private ViewModelLocator()
    {
    }
}
