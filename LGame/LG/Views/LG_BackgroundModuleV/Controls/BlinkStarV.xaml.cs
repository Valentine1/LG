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
    public sealed partial class BlinkStarV : UserControl
    {
        public BlinkStarV()
        {
            this.InitializeComponent();
        }

        public void Initialize(BlinkStarVM svm)
        {
            this.SetValue(Canvas.LeftProperty, svm.StartPosition.X);
            this.SetValue(Canvas.TopProperty, svm.StartPosition.Y);

            im.Source = svm.PictureSource;

            imScaleTrans.CenterX = svm.CenterScaleX;
            imScaleTrans.CenterY = svm.CenterScaleY;

            imRotTrans.CenterX = svm.CenterScaleX;
            imRotTrans.CenterY = svm.CenterScaleY;

            imScaleTrans.ScaleX = svm.ScaleX;
            imScaleTrans.ScaleY = svm.ScaleY;
            svm.OnBlinkStartingVM += svm_OnBlinkStartingVM;
        }


        private void svm_OnBlinkStartingVM(double expandTo)
        {
           
            DoubleAnimation daSX = new DoubleAnimation()
           {
               From = imScaleTrans.ScaleX,
               To = expandTo,
               Duration = new TimeSpan(0, 0, 0, 0, 600),
               AutoReverse = true,
              // EasingFunction = new QuadraticEase()
           };
            Storyboard.SetTarget(daSX, imScaleTrans);
            Storyboard.SetTargetProperty(daSX, "ScaleX");


            DoubleAnimation daSY = new DoubleAnimation()
            {
                From = imScaleTrans.ScaleY,
                To = expandTo,
                Duration = new TimeSpan(0, 0, 0, 0, 600),
                AutoReverse = true,
             //   EasingFunction = new QuadraticEase()
            };

            DoubleAnimation daA = new DoubleAnimation()
            {
                From = imRotTrans.Angle,
                To = imRotTrans.Angle + (new Random()).Next(30, 60),
                Duration = new TimeSpan(0, 0, 0, 0, 1200),
                AutoReverse = false,
            };
            Storyboard.SetTarget(daA, imRotTrans);
            Storyboard.SetTargetProperty(daA, "Angle");


            Storyboard.SetTarget(daSY, imScaleTrans);
            Storyboard.SetTargetProperty(daSY, "ScaleY");
            Storyboard st1 = new Storyboard();
            st1.Children.Add(daSX);
            st1.Children.Add(daSY);
            st1.Children.Add(daA);
            st1.Begin();
        }
    }
}
