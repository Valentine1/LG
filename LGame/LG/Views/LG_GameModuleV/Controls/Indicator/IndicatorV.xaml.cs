using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using LG.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public  partial class IndicatorV : UserControl
    {
        private bool IsMovingEnabled { get; set; }
        IndicatorVM IndicatorVModel { get; set; }
        protected CompositeTransform ComposeTrans
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

        public IndicatorV()
        {
            this.InitializeComponent();
        }

        public virtual void Initialize(IndicatorVM ivm)
        {
            IndicatorVModel = ivm;
            this.StripeContainer.DataContext = ivm;
            ivm.OnOpacityChanged += ivm_OnOpacityChanged;
            ivm.Stripes.CollectionChanged += Stripes_CollectionChanged;
            StripeContainer.Opacity = ivm.Opacity;
            foreach (StripeVM svm in ivm.Stripes)
            {
                Rectangle r = new Rectangle() { Height = svm.Rect.Height, Width = svm.Rect.Width, Margin = svm.Margin, Fill = svm.ColorBrush };
                Binding bindClr = new Binding { Source = svm.ColorBrush, Path = new PropertyPath("Color"), Mode = BindingMode.OneWay };
                BindingOperations.SetBinding(r.Fill, SolidColorBrush.ColorProperty, bindClr);
                StripeContainer.Children.Add(r);
            }
            ivm.OnStartDecreaseMoveAnimation += ivm_OnStartDecreaseMoveAnimation;
            ivm.OnItselfDeleted += ivm_OnItselfDeleted;
        }

     
        private void ivm_OnOpacityChanged(double opacity)
        {
            StripeContainer.Opacity = opacity;
        }

        private void Stripes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                StripeVM svm = (StripeVM)e.NewItems[0];
                Rectangle r = new Rectangle() { Height = svm.Rect.Height, Width = svm.Rect.Width, Margin = svm.Margin, Fill = svm.ColorBrush };
                Binding bindClr = new Binding { Source = svm.ColorBrush, Path = new PropertyPath("Color"), Mode = BindingMode.OneWay };
                r.SetBinding(SolidColorBrush.ColorProperty, bindClr);
                StripeContainer.Children.Add(r);

            }


        }
        protected  void EnableMoving()
        {
            IsMovingEnabled = true;
        }
        private void ivm_OnStartDecreaseMoveAnimation()
        {
            if (IsMovingEnabled)
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
                Storyboard.SetTarget(daExpand, this.ComposeTrans);
                Storyboard.SetTargetProperty(daExpand, "TranslateY");
                Storyboard stor = new Storyboard();
                stor.Completed += stor_Completed;
                stor.Children.Add(daExpand);
                stor.Begin();
            }
        }

        private void stor_Completed(object sender, object e)
        {
            this.ComposeTrans.TranslateY = 0;
            this.IndicatorVModel.DecreaseMoveAnimationEnded();
        }
    
     
        private void ivm_OnItselfDeleted(UnitVM uvm)
        {
            this.DetachEvents(uvm as IndicatorVM);
            this.StripeContainer.Children.Clear();
            this.IndicatorVModel = null;
            this.StripeContainer.DataContext = null;
        }

        private void DetachEvents(IndicatorVM ivm)
        {
            ivm.OnOpacityChanged -= ivm_OnOpacityChanged;
            ivm.Stripes.CollectionChanged -= Stripes_CollectionChanged;
            ivm.OnStartDecreaseMoveAnimation -= ivm_OnStartDecreaseMoveAnimation;
            ivm.OnItselfDeleted -= ivm_OnItselfDeleted;
        }
    }
}
