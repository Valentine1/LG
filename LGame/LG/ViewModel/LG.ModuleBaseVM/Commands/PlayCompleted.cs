using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRtBehaviors;

namespace LG.ViewModels.Commands
{
    public class PlayCompleted : Behavior<MediaElement>
    {
        public RoutedEventHandler PlayCompletedCommand;

        protected override void OnAttached()
        {
            AssociatedObject.MediaEnded += PlayCompletedCommand;
            base.OnAttached(); 
        }

   
        protected override void OnDetaching()
        {
            AssociatedObject.MediaEnded -= PlayCompletedCommand;
            base.OnDetaching();
        }

    }
     
}
