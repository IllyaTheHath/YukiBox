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
        public bool Reverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is WindowsVersion version)
            {
                return !Reverse ? version == WindowsVersion.Windows11 : version == WindowsVersion.Windows10;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
