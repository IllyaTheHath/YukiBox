using System;

using Microsoft.Toolkit.Mvvm.ComponentModel;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Models
{
    public class NavMenuItemBase : ObservableObject { }

    public class NavMenuItem : NavMenuItemBase
    {
        private readonly String _nameResourceName;
        private String _name;

        public String Name
        {
            get => this._name;
            set => SetProperty(ref this._name, value);
        }

        private readonly String _tooltipResourceName;
        private String _tooltip;

        public String Tooltip
        {
            get => this._tooltip;
            set => SetProperty(ref this._tooltip, value);
        }

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
            UpdateNameAndTooltip();
        }

        public void UpdateNameAndTooltip()
        {
            Name = I18NSource.Instance[this._nameResourceName];
            Tooltip = I18NSource.Instance[this._tooltipResourceName];
        }
    }

    public class Separator : NavMenuItemBase { }
}