using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System.Text;
using System.Threading.Tasks;

namespace LG.ViewModels
{
    public class Decrease4Converter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            double v = (double)value;
            return v > 4 ? v - 4 : v;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
