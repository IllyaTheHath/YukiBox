using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YukiBox.Desktop.Controls;

public sealed partial class SettingsPageControl : UserControl
{
    public static readonly DependencyProperty ModuleContentProperty = DependencyProperty.Register("ModuleContent", typeof(object), typeof(SettingsPageControl), new PropertyMetadata(new Grid()));

    public object ModuleContent
    {
        get { return GetValue(ModuleContentProperty); }
        set { SetValue(ModuleContentProperty, value); }
    }

    public SettingsPageControl()
    {
        InitializeComponent();
    }
}
