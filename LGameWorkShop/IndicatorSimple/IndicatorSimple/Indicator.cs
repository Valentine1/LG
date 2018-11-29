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
    public abstract class Indicator
    {
        private int _emptyammo;
        protected int Emptyammo
        {
            get
            {
                return _emptyammo;
            }
            set
            {
                _emptyammo = value;
                if (this.Ammount < 3 && this.IsFlashing)
                {
                    this.Flash();
                }
            }
        }

        private readonly int _capacity;
        public int Capacity
        {
            get
            {
                return _capacity;
            }
        }

        public int Ammount
        {
            get
            {
                return this.Capacity - this.Emptyammo;
            }
        }

        private readonly Color _stripeFullClr;
        protected Color StripeFullClr
        {
            get
            {
                return _stripeFullClr;
            }
        }

        private readonly Color _stripeEmptyClr;
        protected Color StripeEmptyClr
        {
            get
            {
                return _stripeEmptyClr;
            }
        }

        protected bool IsFlashing
        {
            get;
            set;
        }

        private IndicatorPanel _panel;
        public IndicatorPanel Panel
        {
            get
            {
                return _panel;
            }
        }

        public Indicator(int stripeNumber, Color clr, IndicatorType itype, bool isFull)
        {
            _capacity = stripeNumber;
            _panel = new IndicatorPanel(this.Capacity);
            _stripeFullClr = clr;
            if (!isFull)
            {
                this.Emptyammo = this.Capacity;
            }
            _stripeEmptyClr = Color.FromArgb(255, 103, 105, 104);
            Rectangle recf = new Rectangle();
            recf.Height = 3;
            recf.Width = 16;
            recf.Margin = new Thickness(1);
            recf.Fill = new SolidColorBrush(this.StripeEmptyClr);
            this.Panel.StripeContainer.Children.Add(recf);
            for (int i = 1; i <= this.Capacity; i++)
            {
                #region draw indicator stripes
                Rectangle rec = null;
                switch (itype)
                {
                    case IndicatorType.Thin:
                        rec = new Rectangle() { Height = 3, Width = 16, Margin = new Thickness(1) };
                        break;
                    case IndicatorType.Middle:
                        rec = new Rectangle() { Height = 5, Width = 20, Margin = new Thickness(1, 1.5, 1, 1.5) };
                        if (i == 1)
                        {
                            rec.Margin = new Thickness(1, 1, 1, 1);
                        }
                        if (i == stripeNumber)
                        {
                            rec.Margin = new Thickness(1, 1.5, 1, 1);
                        }
                        break;
                    case IndicatorType.Thick:
                        rec = new Rectangle() { Height = 8, Width = 24, Margin = new Thickness(1, 1.5, 1, 1.5) };
                        if (i == 1)
                        {
                            rec.Margin = new Thickness(1, 1, 1, 1);
                        }
                        if (i == stripeNumber)
                        {
                            rec.Margin = new Thickness(1, 1.5, 1, 1);
                        }
                        break;
                }
                rec.Fill = isFull ? new SolidColorBrush(this.StripeFullClr) : new SolidColorBrush(this.StripeEmptyClr);
                this.Panel.StripeContainer.Children.Add(rec);
                #endregion
            }
        }

        private void Flash()
        {
            try
            {
                if (this.Ammount == 2)
                {
                    Storyboard stor = new Storyboard();

                    ObjectAnimationUsingKeyFrames animV = MakeVisFlashAnimation();
                    Storyboard.SetTarget(animV, this.Panel.StripeContainer);
                    Storyboard.SetTargetProperty(animV, "Opacity");
                    stor.Children.Add(animV);

                    ObjectAnimationUsingKeyFrames anim = MakeFlashAnimation(Colors.Yellow);
                    Storyboard.SetTarget(anim, (Rectangle)this.Panel.StripeContainer.Children[Emptyammo + 1]);
                    Storyboard.SetTargetProperty(anim, "Fill");
                    stor.Children.Add(anim);

                    ObjectAnimationUsingKeyFrames anim1 = MakeFlashAnimation(Colors.Yellow);
                    Storyboard.SetTarget(anim1, (Rectangle)this.Panel.StripeContainer.Children[Emptyammo + 2]);
                    Storyboard.SetTargetProperty(anim1, "Fill");
                    stor.Children.Add(anim1);

                    stor.Begin();
                }
                else if (this.Ammount == 1)
                {
                    Storyboard stor = new Storyboard();

                    ObjectAnimationUsingKeyFrames animV = MakeVisFlashAnimation();
                    Storyboard.SetTarget(animV, this.Panel.StripeContainer);
                    Storyboard.SetTargetProperty(animV, "Opacity");
                    stor.Children.Add(animV);

                    ObjectAnimationUsingKeyFrames anim = MakeFlashAnimation(Colors.Red);
                    Storyboard.SetTarget(anim, (Rectangle)this.Panel.StripeContainer.Children[Emptyammo + 1]);
                    Storyboard.SetTargetProperty(anim, "Fill");
                    stor.Children.Add(anim);

                    stor.Begin();
                }
                else
                {

                    Storyboard stor = new Storyboard();

                    ObjectAnimationUsingKeyFrames animV = MakeVisFlashAnimation();
                    Storyboard.SetTarget(animV, this.Panel.StripeContainer);
                    Storyboard.SetTargetProperty(animV, "Opacity");
                    stor.Children.Add(animV);
                    stor.Begin();
                }
            }
            catch (Exception ex)
            {

            }

        }

        private ObjectAnimationUsingKeyFrames MakeVisFlashAnimation()
        {
            ObjectAnimationUsingKeyFrames anim = new ObjectAnimationUsingKeyFrames();

            DiscreteObjectKeyFrame f1 = new DiscreteObjectKeyFrame();
            f1.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 80));
            f1.Value = 0.1;

            anim.KeyFrames.Add(f1);

            DiscreteObjectKeyFrame f2 = new DiscreteObjectKeyFrame();
            f2.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 160));
            f2.Value = 1;

            anim.KeyFrames.Add(f2);

            DiscreteObjectKeyFrame f3 = new DiscreteObjectKeyFrame();
            f3.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 240));
            f3.Value = 0;

            anim.KeyFrames.Add(f3);

            DiscreteObjectKeyFrame f4 = new DiscreteObjectKeyFrame();
            f4.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 320));
            f4.Value = 1;

            anim.KeyFrames.Add(f4);

            DiscreteObjectKeyFrame f5 = new DiscreteObjectKeyFrame();
            f5.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 400));
            f5.Value = 0;

            anim.KeyFrames.Add(f5);

            DiscreteObjectKeyFrame f6 = new DiscreteObjectKeyFrame();
            f6.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 480));
            f6.Value = 1;

            anim.KeyFrames.Add(f6);
            return anim;
        }
        private ObjectAnimationUsingKeyFrames MakeFlashAnimation(Color col)
        {
            ObjectAnimationUsingKeyFrames anim = new ObjectAnimationUsingKeyFrames();

            DiscreteObjectKeyFrame f1 = new DiscreteObjectKeyFrame();
            f1.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 80));
            f1.Value = new SolidColorBrush(this.StripeEmptyClr);

            anim.KeyFrames.Add(f1);

            DiscreteObjectKeyFrame f2 = new DiscreteObjectKeyFrame();
            f2.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 160));
            f2.Value = new SolidColorBrush(col);

            anim.KeyFrames.Add(f2);

            DiscreteObjectKeyFrame f3 = new DiscreteObjectKeyFrame();
            f3.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 240));
            f3.Value = new SolidColorBrush(this.StripeEmptyClr);

            anim.KeyFrames.Add(f3);

            DiscreteObjectKeyFrame f4 = new DiscreteObjectKeyFrame();
            f4.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 320));
            f4.Value = new SolidColorBrush(col);

            anim.KeyFrames.Add(f4);

            DiscreteObjectKeyFrame f5 = new DiscreteObjectKeyFrame();
            f5.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 400));
            f5.Value = new SolidColorBrush(this.StripeEmptyClr);

            anim.KeyFrames.Add(f5);

            DiscreteObjectKeyFrame f6 = new DiscreteObjectKeyFrame();
            f6.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 480));
            f6.Value = new SolidColorBrush(col);

            anim.KeyFrames.Add(f6);
            return anim;
        }




    }
    public enum IndicatorType { Thin, Middle, Thick };
}
