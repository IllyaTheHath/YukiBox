using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModernWpf.Controls;

namespace YukiBox.Desktop.Models
{
    public class NavMenuItemBase { }

    public class NavMenuItem: NavMenuItemBase
    {
        public String Name { get; set; }

        public String Tooltip { get; set; }

        public Symbol Glyph { get; set; }

        public Type TargetType { get; set; }

        public NavMenuItem()
        {
        }

        public NavMenuItem(String name,String toolTip,Symbol glyph,Type targetType)
        {
            Name = name;
            Tooltip = toolTip;
            Glyph = glyph;
            TargetType = targetType;
        }
    }

    public class Separator : NavMenuItemBase { }

    public class Header : NavMenuItemBase
    {
        public String Name { get; set; }

        public Header()
        {
        }

        public Header(String name)
        {
            Name = name;
        }
    }
}
