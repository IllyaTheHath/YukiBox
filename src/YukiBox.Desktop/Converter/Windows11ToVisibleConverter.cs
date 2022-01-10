using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Converter
{
    public class Windows11ToVisibleConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            if(value is WindowsVersion version)
            {
                return version == WindowsVersion.Windows11 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotSupportedException();
        }
    }
}
