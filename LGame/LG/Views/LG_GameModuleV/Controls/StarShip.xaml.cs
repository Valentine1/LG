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
using Windows.UI.Core;
using Windows.System;
using LG.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class StarShip : UserControl
    {
        private ShipControlsVM ShipContrVM;
        private double SpeedX;

        public StarShip()
        {
            this.InitializeComponent();
        }

        public void Initialize(ShipControlsVM shipContrVM)
        {
            this.ShipContrVM = shipContrVM;
            this.DataContext = shipContrVM.ShipVM;
            shipContrVM.ShipVM.OnXChanged += shipVM_OnXChanged;
            shipContrVM.ShipVM.OnYChanged += shipVM_OnYChanged;
            shipContrVM.ShipVM.OnLeftExhaustFireVMCreated += shipVM_OnLeftExhaustFireVMCreated;
            shipContrVM.ShipVM.OnRightExhaustFireVMCreated += shipVM_OnRightExhaustFireVMCreated;
            shipContrVM.ShipVM.OnItselfDeleted += shipVM_OnItselfDeleted;
            shipContrVM.ShipVM.OnSpeedXChangedVM += shipVM_OnSpeedXChangedVM;

            shipContrVM.OnLeftWasDown += shipContrVM_OnLeftWasDown;
            shipContrVM.OnLeftWasUp += shipContrVM_OnLeftWasUp;
            shipContrVM.OnRightWasDown += shipContrVM_OnRightWasDown;
            shipContrVM.OnRightWasUp += shipContrVM_OnRightWasUp;

            tCrossHairView.DataContext = shipContrVM.TargetCrossHairVM;
            imLaserBeam.DataContext = shipContrVM.TargetCrossHairVM.LaserBeamVM;

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
            this.SpeedX = shipContrVM.ShipVM.SpeedXVM;
        }

        private void shipContrVM_OnRightWasUp()
        {
            this.WasRightKeyPressed = false;
            StopAutomatedMove();
        }

        private void shipContrVM_OnRightWasDown()
        {
            this.WasRightKeyPressed = true;
            AutomatedMove(3200);
        }

        private void shipContrVM_OnLeftWasUp()
        {
            this.WasLeftKeyPressed = false;
            StopAutomatedMove();
        }

        private void shipContrVM_OnLeftWasDown()
        {
            this.WasLeftKeyPressed = true;
            AutomatedMove(-3200);
        }


        void CompositionTarget_Rendering(object sender, object e)
        {
            if (WasRightKeyPressed || WasLeftKeyPressed)
            {
                this.ShipContrVM.ShipVM.Xmove = this.shipTranslate.TranslateX;
            }
            double shipLeft =(double)this.GetValue(Canvas.LeftProperty);
            if ( shipLeft + this.shipTranslate.TranslateX < 0)
            {
                StopAutomatedMove();
                this.shipTranslate.TranslateX = -shipLeft;
            }
            if (shipLeft + this.shipTranslate.TranslateX >3200)
            {
                StopAutomatedMove();
                this.shipTranslate.TranslateX = 3200 - shipLeft;
            }
        }

        void shipVM_OnSpeedXChangedVM(double speedX)
        {
            this.SpeedX = speedX;
        }

        private bool WasRightKeyPressed;
        private bool WasLeftKeyPressed;
        private void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Right)
            {
                this.WasRightKeyPressed = false;
                StopAutomatedMove();
            }
            else if (args.VirtualKey == VirtualKey.Left)
            {
                this.WasLeftKeyPressed = false;
                StopAutomatedMove();
            }
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Right)
            {
                if (!this.WasRightKeyPressed)
                {
                    this.WasRightKeyPressed = true;
                    AutomatedMove(3200);
                }
            }
            else if (args.VirtualKey == VirtualKey.Left)
            {
                if (!this.WasLeftKeyPressed)
                {
                    this.WasLeftKeyPressed = true;
                    AutomatedMove(-3200);
                }
            }
        }
        private void shipVM_OnLeftExhaustFireVMCreated(ExhaustFireVM fvm)
        {
            LeftExFireV.Initialize(fvm);
        }
        private void shipVM_OnRightExhaustFireVMCreated(ExhaustFireVM fvm)
        {
            RightExFireV.Initialize(fvm);
        }

        private void shipVM_OnXChanged(double x)
        {
            //shipTranslate.TranslateX = x;
          
        }

        private void shipVM_OnYChanged(double y)
        {
            shipTranslate.TranslateY = y;
            
        }

        Storyboard MovingStoryboard = new Storyboard();
        private void AutomatedMove(double lim)
        {
            if (this.MovingStoryboard.Children.Count == 0)
            {
                DoubleAnimation daExpand = new DoubleAnimation()
                {
                    From = shipTranslate.TranslateX,
                    To = shipTranslate.TranslateX + lim,
                    Duration = new TimeSpan(0, 0, 0, 0, (int)(Math.Abs(lim) / this.SpeedX))
                };
                Storyboard.SetTarget(daExpand, this.shipTranslate);
                Storyboard.SetTargetProperty(daExpand, "TranslateX");
                this.MovingStoryboard.Children.Add(daExpand);
                this.MovingStoryboard.Begin();
                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }
        private void StopAutomatedMove()
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            double ty = this.shipTranslate.TranslateX;
            this.MovingStoryboard.Stop();
            this.MovingStoryboard.Children.Clear();
            this.shipTranslate.TranslateX = ty;
        }
        private void shipVM_OnItselfDeleted(UnitVM uvm)
        {
            this.DetachEvents(uvm as StarShipVM);
            this.DataContext = null;
        }

        public void DetachEvents(StarShipVM ship)
        {
            ship.OnItselfDeleted -= shipVM_OnItselfDeleted;
            ship.OnLeftExhaustFireVMCreated -= shipVM_OnLeftExhaustFireVMCreated;
            ship.OnRightExhaustFireVMCreated -= shipVM_OnRightExhaustFireVMCreated;
            ship.OnXChanged -= shipVM_OnXChanged;
            ship.OnYChanged -= shipVM_OnYChanged;
            ship.OnSpeedXChangedVM -= shipVM_OnSpeedXChangedVM;

            this.ShipContrVM.OnLeftWasDown -= shipContrVM_OnLeftWasDown;
            this.ShipContrVM.OnLeftWasUp -= shipContrVM_OnLeftWasUp;
            this.ShipContrVM.OnRightWasDown -= shipContrVM_OnRightWasDown;
            this.ShipContrVM.OnRightWasUp -= shipContrVM_OnRightWasUp;

            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp -= CoreWindow_KeyUp;
        }
    }
}
