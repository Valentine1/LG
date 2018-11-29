using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class SpeedToAngleConv : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (double)value * 1500 / SpaceParams.SpaceHeightRatioTo900;
            //0.1491  -  300 000

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
