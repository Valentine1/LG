using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

namespace IndicatorSimple
{
    public class IndicatorOrdinaryFlashing : IndicatorOrdinary
    {
        public IndicatorOrdinaryFlashing(int stripeNumber, Color clr, IndicatorType itype)
            : base(stripeNumber, clr, itype)
        {
            this.IsFlashing = true;
        }

    
    }
}
