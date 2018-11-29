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
    public partial class IndicatorMoving : Indicator
    {
        public IndicatorMoving(int stripeNumber, Color clr, IndicatorType itype)
            : base(stripeNumber, clr, itype, true)
        {
            this.IsFlashing = true;
        }

        public  void Decrease()
        {
            if (Emptyammo < this.Capacity)
            {
                DoubleAnimation daExpand = new DoubleAnimation()
                {
                    From = 0,
                    To = 5,
                    // AutoReverse = true,
                    Duration = new TimeSpan(0, 0, 0, 0, 300),
                    // RepeatBehavior = RepeatBehavior.Forever
                };
                daExpand.EasingFunction = new QuadraticEase();
                Storyboard.SetTarget(daExpand, this.Panel.StripeContainerTransition);
                Storyboard.SetTargetProperty(daExpand, "TranslateY");
                Storyboard stor = new Storyboard();
                stor.Completed += stor_Completed;
                stor.Children.Add(daExpand);
                stor.Begin();
            }
        }

        private void stor_Completed(object sender, object e)
        {
                ++Emptyammo;
                ((Rectangle)this.Panel.StripeContainer.Children[Emptyammo]).Fill = new SolidColorBrush(this.StripeEmptyClr);
                this.Panel.StripeContainerTransition.TranslateY = 0;
         
        }
        public void Recharge()
        {
            for (int i = 1; i <= this.Capacity; i++)
            {
                ((Rectangle)this.Panel.StripeContainer.Children[i]).Fill = new SolidColorBrush(this.StripeFullClr);
            }
            Emptyammo = 0;
        }

    }
}
