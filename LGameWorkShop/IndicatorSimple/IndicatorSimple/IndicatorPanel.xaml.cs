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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace IndicatorSimple
{
    public sealed partial class IndicatorPanel : UserControl
    {
        public CompositeTransform StripeContainerTransition
        {
            get
            {
                return this.stripeContainerTrans;
            }
        }
 
        public StackPanel StripeContainer
        {
            get
            {
                return this.stripeContainer;
            }
        }

        public IndicatorPanel(int stripeNumber)
        {
            this.InitializeComponent();

        }

    
    }

  
}
