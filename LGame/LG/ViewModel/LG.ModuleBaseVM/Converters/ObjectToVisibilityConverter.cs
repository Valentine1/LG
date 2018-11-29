using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System.Text;
using System.Threading.Tasks;


namespace LG.ViewModels
{
    public class ObjectToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
