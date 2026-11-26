using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Converter;

public class Windows11ToVisibleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is WindowsVersion version)
        {
            return version == WindowsVersion.Windows11 ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
