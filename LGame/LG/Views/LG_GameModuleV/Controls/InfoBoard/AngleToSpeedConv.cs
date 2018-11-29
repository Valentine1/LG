using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace LG.Views
{
    class AngleToSpeedConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //223.65 - 300 000
            return ((int)Math.Ceiling((double)value * 300000 / 223.65)).ToString("000 000");
        }



        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
