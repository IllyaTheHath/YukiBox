using System;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Models
{
    public class NavMenuItemBase { }

    public class NavMenuItem : NavMenuItemBase
    {
        private readonly string _nameResourceName;

        public string Name { get; set; }

        private readonly string _tooltipResourceName;

        public string Tooltip { get; set; }

        public string Glyph { get; set; }

        public Type TargetType { get; set; }

        public NavMenuItem()
        {
        }

        public NavMenuItem(string name, string toolTip, string glyph, Type targetType)
        {
            this._nameResourceName = name;
            this._tooltipResourceName = toolTip;

            Glyph = glyph;
            TargetType = targetType;
            Name = I18NSource.Instance[this._nameResourceName];
            Tooltip = I18NSource.Instance[this._tooltipResourceName];
        }
    }

    public class Separator : NavMenuItemBase { }
}