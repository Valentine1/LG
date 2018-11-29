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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Streams;
using LG.ViewModels;
using Windows.UI.Xaml.Media.Imaging;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class InfoBoardIntroV : UserControl
    {
        private InfoBoardVM InfoBoardVModel { get; set; }

        #region IsSmallChangeProperty
        public static readonly DependencyProperty IsSmallChangeProperty = DependencyProperty.Register("IsSmallChange",
                                                                                                typeof(bool),
                                                                                                typeof(InfoBoardIntroV),
                                                                                                null);
        public bool IsSmallChange
        {
            get
            {
                return (bool)GetValue(IsSmallChangeProperty);
            }
            set
            {
                SetValue(IsSmallChangeProperty, value);
            }
        }
        #endregion

        #region ShipSpeedProperty
        public static readonly DependencyProperty ShipSpeedProperty = DependencyProperty.Register("ShipSpeed",
                                                                                                typeof(double),
                                                                                                typeof(InfoBoardIntroV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(OnShipSpeedChanged))
                                                                                                );
        public double ShipSpeed
        {
            get
            {
                return (double)GetValue(ShipSpeedProperty);
            }
            set
            {
                SetValue(ShipSpeedProperty, value);
            }
        }
        private static void OnShipSpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InfoBoardIntroV ibView = (InfoBoardIntroV)d;
            ibView.SetShipSpeedometer((double)e.OldValue);
        }
        #endregion

        private double BigWayDistance { get; set; }
        private const double BigWayDistanceKilometers = 44388000;
        private Storyboard IdleProgazovkaStory = new Storyboard();

        public InfoBoardIntroV()
        {
            this.InitializeComponent();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            //bigCircle.StrokeDashArray.Add(2.5);
            //bigCircle.StrokeDashArray.Add(0.5);
            //bigCircle.StrokeDashOffset = 1;
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            if (accelerateLongPlayer.Position.Milliseconds > 450)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                this.IdleProgazovkaBegin();
            }
        }

        public void Initialize(GameVM gameVM)
        {
            this.DataContext = gameVM;
            InfoBoardVModel = gameVM.InfoBoardVModel;
            selPicBoxBorder.Height = gameVM.InfoBoardVModel.PictureHeight;
            vbSpeedometer.Margin = new Thickness(5, gameVM.InfoBoardVModel.SpeedometerTopMargin, 0, gameVM.InfoBoardVModel.SpeedometerBottomMargin);
            vbSpeedometer.Height = gameVM.InfoBoardVModel.SpeedometerHeight;
            vbIndicators.Width = gameVM.InfoBoardVModel.IndicatorsWidth;
            vbIndicators.Height = gameVM.InfoBoardVModel.IndicatorsHeight;
            vbIndicators.Margin = new Thickness(0, 0, 0, gameVM.InfoBoardVModel.IndicatorsBottomMargin);
            tbDist.Text = BigWayDistanceKilometers.ToString("00 000 000");
            this.BigWayDistance = gameVM.SpaceVModel.BigWayDistance;
            gameVM.InfoBoardVModel.OnSelectedWordChanged += InfoBoardVModel_OnSelectedWordChanged;
            gameVM.InfoBoardVModel.OnDelayEndedForPictureToSpeakShowVM += InfoBoardVModel_OnDelayEndedForPictureToSpeakShowVM;
            gameVM.OnCoveredDistanceVMChanged += gameVM_OnCoveredDistanceVMChanged;
            gameVM.InfoBoardVModel.OnClearShowWordVM += InfoBoardVModel_OnClearShowWordVM;

            SpeedToAngleConv conv = new SpeedToAngleConv();
            Binding bindSpeed = new Binding
            {
                Source = gameVM.InfoBoardVModel.ShipSpeed,
                Path = new PropertyPath("Speed"),
                Converter = conv
            };
            this.SetBinding(InfoBoardIntroV.ShipSpeedProperty, bindSpeed);

            Binding bindIsSmallChange = new Binding
            {
                Source = gameVM.InfoBoardVModel.ShipSpeed,
                Path = new PropertyPath("IsSmallChange")
            };
            this.SetBinding(InfoBoardIntroV.IsSmallChangeProperty, bindIsSmallChange);

            ammoIndicator.Initialize(gameVM.SpaceVModel.StarShipControlsVM.AmmoCountIndicatorVM);

            accelerateLongPlayer.SetSource(gameVM.InfoBoardVModel.AcceleratingLongAudio, "audio/mpeg");
            breakLongPlayer.SetSource(gameVM.InfoBoardVModel.BreakingLongAudio, "audio/mpeg");
            InfoBoardVModel.OnIdleProgazovkaBegin += InfoBoardVModel_OnIdleProgazovkaBegin;
            gameVM.OnItselfDeleted += gameVM_OnItselfDeleted;
        }

        private void gameVM_OnCoveredDistanceVMChanged(double dist)
        {
          tbCovDist.Text =((int)dist * BigWayDistanceKilometers / this.BigWayDistance).ToString("00 000 000");
        }

        private void InfoBoardVModel_OnIdleProgazovkaBegin()
        {
            PlayAccelerateLong();
        }

        private void InfoBoardVModel_OnSelectedWordChanged(PictureBoxVM pbox)
        {
            if (pbox != null)
            {
                if (selPicBoxBorder.Child != null)
                {
                    (selPicBoxBorder.Child as PictureBox).DeleteItself();
                }
                PictureBox spic = new PictureBox(pbox, true);
                spic.OnShouldBeRemoved += spic_OnShouldBeRemoved;
                selPicBoxBorder.Child = spic;
                selPicBoxBorder.Opacity = InfoBoardVModel.IsPictureInitiallyVisible ? 1 : 0;
                tbSelWord.Text = pbox.Word;
            }
            else
            {
                selPicBoxBorder.Child = null;
                tbSelWord.Text = string.Empty;
            }
        }

        private void InfoBoardVModel_OnClearShowWordVM()
        {
            if (selPicBoxBorder.Child != null)
            {
                (selPicBoxBorder.Child as PictureBox).DeleteItself();
            }
        }
        private void InfoBoardVModel_OnDelayEndedForPictureToSpeakShowVM()
        {
            selPicBoxBorder.Opacity = 1;
        }

        private void spic_OnShouldBeRemoved(UIElement sender)
         {
            (sender as PictureBox).OnShouldBeRemoved -= spic_OnShouldBeRemoved;
            selPicBoxBorder.Child = null;
            tbSelWord.Text = string.Empty;
        }

        #region Progazovka
        private void IdleProgazovkaBegin()
        {
            this.SpeedLightBlink(imSpeedUp, 1200, 1);

            this.IdleProgazovkaStory.Completed += IdleProgazovkaAccelerate_Completed;
           
            DoubleAnimation daSpeed = new DoubleAnimation()
            {
                From = 0,
                To = 230,
                Duration = new TimeSpan(0, 0, 0, 0, 2400)
            };
            daSpeed.EasingFunction = new QuadraticEase();

            DoubleAnimation dnSpeed = new DoubleAnimation()
            {
                From = 0,
                To = 230,
                Duration = new TimeSpan(0, 0, 0, 0, 2400)
            };
            dnSpeed.EasingFunction = new QuadraticEase();

            Storyboard.SetTarget(daSpeed, Trans);
            Storyboard.SetTargetProperty(daSpeed, "Angle");
            Storyboard.SetTarget(dnSpeed, speedRect);
            Storyboard.SetTargetProperty(dnSpeed, "Opacity");

            IdleProgazovkaStory.Children.Add(daSpeed);
            IdleProgazovkaStory.Children.Add(dnSpeed);
            IdleProgazovkaStory.Begin();
        }
        private void IdleProgazovkaAccelerate_Completed(object sender, object e)
        {
            IdleProgazovkaStory.Stop();
            IdleProgazovkaStory.Children.Clear();
            this.IdleProgazovkaStory.Completed -= IdleProgazovkaAccelerate_Completed;
            this.IdleProgazovkaStory.Completed += IdleProgazovkaBreak_Completed;
            this.SpeedLightBlink(imSpeedDown, 1100, 1);
            this.PlayBreakLong();
            DoubleAnimation daSpeed = new DoubleAnimation()
            {
                From = 230,
                To = 0,
                Duration = new TimeSpan(0, 0, 0, 0, 2200),
            };
            daSpeed.EasingFunction = new QuadraticEase();
            DoubleAnimation dnSpeed = new DoubleAnimation()
            {
                From = 230,
                To = 0,
                Duration = new TimeSpan(0, 0, 0, 0, 2200),
            };
            dnSpeed.EasingFunction = new QuadraticEase();

            Storyboard.SetTarget(daSpeed, Trans);
            Storyboard.SetTargetProperty(daSpeed, "Angle");
            Storyboard.SetTarget(dnSpeed, speedRect);
            Storyboard.SetTargetProperty(dnSpeed, "Opacity");

            IdleProgazovkaStory.Children.Add(daSpeed);
            IdleProgazovkaStory.Children.Add(dnSpeed);
            IdleProgazovkaStory.Begin();
        }
        private void IdleProgazovkaBreak_Completed(object sender, object e)
        {
            this.IdleProgazovkaStory.Completed -= IdleProgazovkaBreak_Completed;
            this.InfoBoardVModel.IdleProgazovkaCompleted();
            this.SetLimitArc(this.InfoBoardVModel.MaxSpeed * 1500);
        }
        #endregion

        public void SetShipSpeedometer(double oldSpeed)
        {
            if (this.ShipSpeed == 0) 
            {
                this.SpeedLightBlink(imSpeedDown, 2200, 1);
                this.PlayBreakLong();
                this.AnimateSpeedArrow(oldSpeed, this.ShipSpeed, 2200);
                return;
            }
            if (this.ShipSpeed > oldSpeed)
            {
                this.SpeedLightBlink(imSpeedUp, 550, 1);
                this.PlayAccelerateShort();
                this.AnimateSpeedArrow(oldSpeed, this.ShipSpeed, 1100);
            }
            else
            {
                if (this.IsSmallChange)
                {
                    this.SpeedLightBlink(imSpeedDown, 250, 0.3);
                    this.breakShortPlayer.Play();
                    this.AnimateSpeedArrow(oldSpeed, this.ShipSpeed, 500);
                }
                else
                {
                    this.SpeedLightBlink(imSpeedDown, 550, 1);
                    this.PlayBreakShort();
                    this.AnimateSpeedArrow(oldSpeed, this.ShipSpeed, 1100);
                }
            }
        }
        private void SetLimitArc(double angle)
        {
            if (angle > 180)
            {
                arcSeg.IsLargeArc = false;
            }
            angle = angle - 44.99;
            arcLimit.Visibility = Visibility.Visible;
            if (angle > 0 && angle <= 90)
            {
                arcStart.StartPoint = new Point(211 - 211 * Math.Cos(angle / 180d * 3.14), 211 - 211 * Math.Sin(angle / 180d * 3.14));
                arcSeg.Point = arcStart.StartPoint;
            }
            else if (angle > 90 && angle <= 180)
            {
                arcStart.StartPoint = new Point(211 + 211 * Math.Abs(Math.Cos(angle / 180d * 3.14)), 211 - 211 * Math.Abs(Math.Sin(angle / 180d * 3.14)));
                arcSeg.Point = arcStart.StartPoint;
            }
            else if (angle > 180)
            {
                arcStart.StartPoint = new Point(211 + 211 * Math.Abs(Math.Cos(angle / 180d * 3.14)), 211 + 211 * Math.Abs(Math.Sin(angle / 180d * 3.14)));
                arcSeg.Point = arcStart.StartPoint;
            }


        }
        public void SpeedLightBlink(Image im, int timeMs, double opIntensity)
        {
            DoubleAnimation daExpand = new DoubleAnimation()
            {
                From = 0.1,
                To = opIntensity,
                AutoReverse = true,
                Duration = new TimeSpan(0, 0, 0, 0, timeMs),
                //RepeatBehavior = RepeatBehavior.Forever
            };
            daExpand.EasingFunction = new QuarticEase();
            Storyboard.SetTarget(daExpand, im);
            Storyboard.SetTargetProperty(daExpand, "Opacity");
            Storyboard stor = new Storyboard();
            stor.Children.Add(daExpand);
            stor.Begin();
        }
        public void AnimateSpeedArrow(double oldSpeed, double newSpeed, int timeMs)
        {
            DoubleAnimation daSpeed = new DoubleAnimation()
            {
                From = oldSpeed,
                To = newSpeed,
                Duration = new TimeSpan(0, 0, 0, 0, timeMs),
            };
            daSpeed.EasingFunction = new QuadraticEase();
            DoubleAnimation dnSpeed = new DoubleAnimation()
            {
                From = oldSpeed,
                To = newSpeed,
                Duration = new TimeSpan(0, 0, 0, 0, timeMs),
            };
            dnSpeed.EasingFunction = new QuadraticEase();

            Storyboard.SetTarget(daSpeed, Trans);
            Storyboard.SetTargetProperty(daSpeed, "Angle");
            Storyboard.SetTarget(dnSpeed, speedRect);
            Storyboard.SetTargetProperty(dnSpeed, "Opacity");
            Storyboard stor = new Storyboard();
            stor.Children.Add(daSpeed);
            stor.Children.Add(dnSpeed);
            stor.Begin();
        }

        public void PlayAccelerateShort()
        {
            this.acceleratePlayer.Play();
        }
        public void PlayBreakShort()
        {
            this.breakPlayer.Play();
        }
        public void PlayAccelerateLong()
        {
            accelerateLongPlayer.Play();
        }
        public void PlayBreakLong()
        {
            breakLongPlayer.Play();
        }
       

        private void gameVM_OnItselfDeleted(UnitVM uvm)
        {
            uvm.OnItselfDeleted -= gameVM_OnItselfDeleted;
            this.DataContext = null;
            selPicBoxBorder.Child = null;
            this.DetachEvents(uvm as GameVM);
        }

        private void DetachEvents(GameVM gameVM)
        {
            InfoBoardVModel.OnIdleProgazovkaBegin -= InfoBoardVModel_OnIdleProgazovkaBegin;
            gameVM.InfoBoardVModel.OnSelectedWordChanged -= InfoBoardVModel_OnSelectedWordChanged;
            gameVM.InfoBoardVModel.OnDelayEndedForPictureToSpeakShowVM -= InfoBoardVModel_OnDelayEndedForPictureToSpeakShowVM;
            gameVM.InfoBoardVModel.OnClearShowWordVM -= InfoBoardVModel_OnClearShowWordVM;

        }
    }


}
