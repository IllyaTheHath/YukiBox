using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Models;

public class NavMenuItemBase { }

public class NavMenuItem : NavMenuItemBase
{
    public string Name { get; set; }

    public string Tooltip { get; set; }

    public string Glyph { get; set; }

    public Type TargetType { get; set; }

    private readonly string _nameResourceName;

    private readonly string _tooltipResourceName;

    public NavMenuItem()
    {
    }

    public NavMenuItem(string name, string toolTip, string glyph, Type targetType)
    {
        _nameResourceName = name;
        _tooltipResourceName = toolTip;

        Glyph = glyph;
        TargetType = targetType;
        Name = I18NSource.Instance[_nameResourceName];
        Tooltip = I18NSource.Instance[_tooltipResourceName];
    }
}

public class Separator : NavMenuItemBase { }
