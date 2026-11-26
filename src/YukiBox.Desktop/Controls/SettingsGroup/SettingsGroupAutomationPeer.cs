using Microsoft.UI.Xaml.Automation.Peers;

namespace YukiBox.Desktop.Controls;

public class SettingsGroupAutomationPeer : FrameworkElementAutomationPeer
{
    public SettingsGroupAutomationPeer(SettingsGroup owner)
        : base(owner)
    {
    }

    protected override string GetNameCore()
    {
        var selectedSettingsGroup = (SettingsGroup)Owner;
        return selectedSettingsGroup.Header;
    }
}
