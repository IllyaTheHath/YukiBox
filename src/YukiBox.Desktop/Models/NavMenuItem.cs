using System;

using Microsoft.Toolkit.Mvvm.ComponentModel;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Models
{
    public class NavMenuItemBase { }

    public class NavMenuItem : NavMenuItemBase
    {
        private readonly String _nameResourceName;

        public String Name { get; set; }

        private readonly String _tooltipResourceName;

        public String Tooltip { get; set; }

        public String Glyph { get; set; }

        public Type TargetType { get; set; }

        public NavMenuItem()
        {
        }

        public NavMenuItem(String name, String toolTip, String glyph, Type targetType)
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