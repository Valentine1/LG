using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using LG.ViewModels;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class AvatarV : UserControl
    {
        internal AvatarVM AvatarVModel { get; set; }
        public AvatarV(AvatarVM avVM, bool isTop)
        {
            this.InitializeComponent();
            this.AvatarVModel = avVM;
            if (isTop)
            {
                imgBorder.Visibility = Visibility.Collapsed;
                imgControl.Width = this.AvatarVModel.BlockSize.Width;
                imgControl.Height = this.AvatarVModel.BlockSize.Height;
            }
         
            this.DataContext = this.AvatarVModel;
            backScale.CenterX = this.AvatarVModel.BlockSize.Width / 2;
            backScale.CenterY = this.AvatarVModel.BlockSize.Height / 2;
        }
        Storyboard stor = null;
        public void StartPulsing()
        {
            stor = new Storyboard();
            DoubleAnimation daOpacity = new DoubleAnimation()
            {
                From = 0.4,
                To = 1.0,
                AutoReverse = true,
                Duration = new TimeSpan(0, 0, 0, 0, 650),
                EasingFunction = new ExponentialEase(),
                RepeatBehavior = RepeatBehavior.Forever
            };

            Storyboard.SetTarget(daOpacity, this);
            Storyboard.SetTargetProperty(daOpacity, "Opacity");
            stor.Children.Add(daOpacity);

            DoubleAnimation daExtandX = new DoubleAnimation()
            {
                From = 1.0,
                To = 1.1,
                AutoReverse = true,
                Duration = new TimeSpan(0, 0, 0, 0, 650),
                EasingFunction = new ExponentialEase(),
                RepeatBehavior = RepeatBehavior.Forever
            };

            Storyboard.SetTarget(daExtandX, backScale);
            Storyboard.SetTargetProperty(daExtandX, "ScaleX");
            stor.Children.Add(daExtandX);

            DoubleAnimation daExtandY = new DoubleAnimation()
            {
                From = 1.0,
                To = 1.1,
                AutoReverse = true,
                Duration = new TimeSpan(0, 0, 0, 0, 650),
                EasingFunction = new ExponentialEase(),
                RepeatBehavior = RepeatBehavior.Forever
            };
            Storyboard.SetTarget(daExtandY, backScale);
            Storyboard.SetTargetProperty(daExtandY, "ScaleY");
            stor.Children.Add(daExtandY);
            //TranslateTransform tt = new TranslateTransform();
            //tt.X = 1920;
            //mod.RenderTransform = tt;
            //DoubleAnimation daExpand = new DoubleAnimation()
            //{
            //    From = tt.X,
            //    To = 0,
            //    AutoReverse = false,
            //    Duration = new TimeSpan(0, 0, 0, 0, 500)
            //};
            //Storyboard.SetTarget(daExpand, tt);
            //Storyboard.SetTargetProperty(daExpand, "X");

            //stor.Children.Add(daExpand);

            //gridFrame.Children.Add(mod);
            stor.Begin();
        }

        public void StopPulsing()
        {
            if (stor != null)
            {
                stor.Stop();
                this.Opacity = 1.0;
            }
        }
    }
}
