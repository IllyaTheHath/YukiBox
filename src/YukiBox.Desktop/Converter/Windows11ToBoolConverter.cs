using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Data;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Converter
{
    public class Windows11ToBoolConverter : IValueConverter
    {
        public Boolean Reverse { get; set; }

        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            if(value is WindowsVersion version)
            {
                return !Reverse ? version == WindowsVersion.Windows11 : version == WindowsVersion.Windows10;
            }
            return false;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotSupportedException();
        }
    }
}
