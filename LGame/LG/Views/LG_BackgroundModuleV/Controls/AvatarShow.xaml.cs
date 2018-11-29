using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using LG.ViewModels;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class AvatarShow : UserControl
    {
        public event AvatarShowEnded OnAvatarShowEnded;

        private AssetVM AvaImageVM { get; set; }
        private bool IsBeginShow { get; set; }
        private bool IsStopped { get; set; }


        Storyboard avaImageDescendStor = null;
        Storyboard avaTextSizeStor = null;
        Storyboard avaTextOrangeStor = null;
        Storyboard avaTextRedStor = null;
        Storyboard avaTextOpacityStor = null;
        Storyboard avaImageOpacityStor = null;

        private List<Storyboard> _showStories;
        private List<Storyboard> ShowStories
        {
            get
            {
                if (_showStories == null)
                {
                    _showStories = new List<Storyboard>();
                    _showStories.Add(avaImageDescendStor);
                    _showStories.Add(avaTextSizeStor);
                    _showStories.Add(avaTextOrangeStor);
                    _showStories.Add(avaTextRedStor);
                    _showStories.Add(avaTextOpacityStor);
                    _showStories.Add(avaImageOpacityStor);
                }
                return _showStories;
            }
        }

        public AvatarShow()
        {
            this.InitializeComponent();
        }

        public void StartAvatarBeginShow(AssetVM avaImage, AssetVM avaName, Size spaceSize)
        {
            this.IsBeginShow = true;
            imAvatar.Opacity = 1.0;
            tbAvatar.Opacity = 0;
            this.StartAvatarShow(avaImage, avaName, spaceSize);
        }

        public void StartAvatarFinishShow(AssetVM avaImage, AssetVM avaName, Size spaceSize)
        {
            this.IsBeginShow = false;
            imAvatar.Opacity = 0.5;
          //  tbAvatar.Opacity = 0.5;
            this.StartAvatarShow(avaImage, avaName, spaceSize);
        }

        public void Clear()
        {
            imAvatar.Opacity = 0;
            tbAvatar.Opacity = 0.002;
        }
        public void StopAndClear()
        {
            IsStopped = true;
            if (avaImageDescendStor != null)
            {
                avaImageDescendStor.Completed -= avaImageDescendStor_Completed;
                avaImageDescendStor.SkipToFill();
            }
            if (avaTextSizeStor != null)
            {
                avaTextSizeStor.Completed -= avaTextStor_Completed;
                avaTextSizeStor.SkipToFill();
            }
            if (avaTextOrangeStor != null)
            {
                avaTextOrangeStor.Completed -= avaTextOrangeStor_Completed;
                avaTextOrangeStor.SkipToFill();
            }
            if (avaTextRedStor != null)
            {
                avaTextRedStor.Completed -= avaTextRedStor_Completed;
                avaTextRedStor.SkipToFill();
            }
            if (avaTextOpacityStor != null)
            {
                avaTextOpacityStor.Completed -= avaTextOpacityStor_Completed;
                avaTextOpacityStor.SkipToFill();
            }
            if (avaImageOpacityStor != null)
            {
                avaImageOpacityStor.SkipToFill();
            }
            this.Clear();
        }

        private void StartAvatarShow(AssetVM avaImage, AssetVM avaName, Size spaceSize)
        {
            this.IsStopped = false;
            this.avaImageDescendStor = new Storyboard();
            this.AvaImageVM = avaImage;

            imAvatar.DataContext = this.AvaImageVM;
            imAvatar.Height = this.AvaImageVM.BlockSize.Height;
            imAvatar.Width = this.AvaImageVM.BlockSize.Width;
            imAvatar.SetValue(Canvas.TopProperty, this.AvaImageVM.StartPosition.Y);
            imAvatar.SetValue(Canvas.LeftProperty, this.AvaImageVM.StartPosition.X);

            DoubleAnimation daAdvent = new DoubleAnimation()
            {
                From = 0,
                To = -this.AvaImageVM.StartPosition.Y + this.AvaImageVM.Y,
                AutoReverse = false,
                Duration = new TimeSpan(0, 0, 0, 0, 1200),
                EasingFunction = new QuadraticEase()
            };

            Storyboard.SetTarget(daAdvent, imAvatarTrans);
            Storyboard.SetTargetProperty(daAdvent, "Y");
            avaImageDescendStor.Children.Add(daAdvent);
            avaImageDescendStor.Completed += avaImageDescendStor_Completed;

            canSpace.Width = spaceSize.Width;
            canSpace.Height = spaceSize.Height;
            tbAvatar.DataContext = avaName;
            tbAvatarFore.Color = Colors.Orange;
            avaImageDescendStor.Begin();
        }

        private void avaImageDescendStor_Completed(object sender, object e)
        {
            ((Storyboard)sender).Completed -= avaImageDescendStor_Completed;
            if (this.IsStopped)
            {
                return;
            }
            tbAvatar.Opacity = 1.0;
            avaTextSizeStor = new Storyboard();
            DoubleAnimation daAdventY = new DoubleAnimation()
            {
                From = 0.1,
                To = 1,
                AutoReverse = false,
                Duration = new TimeSpan(0, 0, 0, 0, 1100),
                EasingFunction = new QuadraticEase()
            };
            DoubleAnimation daAdventX = new DoubleAnimation()
            {
                From = 0.1,
                To = 1,
                AutoReverse = false,
                Duration = new TimeSpan(0, 0, 0, 0, 1100),
                EasingFunction = new QuadraticEase()
            };
            Storyboard.SetTarget(daAdventX, tbAvatarScale);
            Storyboard.SetTarget(daAdventY, tbAvatarScale);
            Storyboard.SetTargetProperty(daAdventX, "ScaleX");
            Storyboard.SetTargetProperty(daAdventY, "ScaleY");
            avaTextSizeStor.Children.Add(daAdventX);
            avaTextSizeStor.Children.Add(daAdventY);
            if (this.IsBeginShow)
            {
                avaTextSizeStor.Completed += avaTextStor_Completed;
            }
            avaTextSizeStor.Begin();
        }

        private void avaTextStor_Completed(object sender, object e)
        {
            ((Storyboard)sender).Completed -= avaTextStor_Completed;

            if (this.IsStopped)
            {
                return;
            }
            avaTextOrangeStor = new Storyboard();
            ColorAnimation colAnimation = new ColorAnimation()
            {
                From = Colors.Orange,
                To = Colors.OrangeRed,
                AutoReverse = false,
                Duration = new TimeSpan(0, 0, 0, 0, 1200),
            };
            Storyboard.SetTarget(colAnimation, tbAvatarFore);
            Storyboard.SetTargetProperty(colAnimation, "Color");
            avaTextOrangeStor.Children.Add(colAnimation);
            avaTextOrangeStor.Completed += avaTextOrangeStor_Completed;
            avaTextOrangeStor.Begin();
        }

        private void avaTextOrangeStor_Completed(object sender, object e)
        {

            ((Storyboard)sender).Completed -= avaTextOrangeStor_Completed;
            if (this.IsStopped)
            {
                return;
            }
            avaTextRedStor = new Storyboard();
            ColorAnimation colAnimation = new ColorAnimation()
            {
                From = Colors.OrangeRed,
                To = Colors.OrangeRed,
                AutoReverse = false,
                Duration = new TimeSpan(0, 0, 0, 0, 1500),
            };
            Storyboard.SetTarget(colAnimation, tbAvatarFore);
            Storyboard.SetTargetProperty(colAnimation, "Color");
            avaTextRedStor.Children.Add(colAnimation);
            avaTextRedStor.Completed += avaTextRedStor_Completed;
            avaTextRedStor.Begin();
        }

        private void avaTextRedStor_Completed(object sender, object e)
        {
            ((Storyboard)sender).Completed -= avaTextRedStor_Completed;
            if (this.IsStopped)
            {
                return;
            }
            avaTextOpacityStor = new Storyboard();
            DoubleAnimation daOpacity = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                AutoReverse = false,
                Duration = new TimeSpan(0, 0, 0, 0, 1000),
                EasingFunction = new QuadraticEase()
            };
            Storyboard.SetTarget(daOpacity, tbAvatar);
            Storyboard.SetTargetProperty(daOpacity, "Opacity");
            avaTextOpacityStor.Children.Add(daOpacity);

            avaTextOpacityStor.Completed += avaTextOpacityStor_Completed;
            avaTextOpacityStor.Begin();
        }

        private void avaTextOpacityStor_Completed(object sender, object e)
        {
            ((Storyboard)sender).Completed -= avaTextOpacityStor_Completed;
            if (this.IsStopped)
            {
                return;
            }
            avaImageOpacityStor = new Storyboard();
            DoubleAnimation daOpac = new DoubleAnimation()
            {
                From = 1,
                To = 0.1,
                AutoReverse = false,
                Duration = new TimeSpan(0, 0, 0, 0, 5000)
            };
            Storyboard.SetTarget(daOpac, imAvatar);
            Storyboard.SetTargetProperty(daOpac, "Opacity");
            avaImageOpacityStor.Children.Add(daOpac);
            avaImageOpacityStor.Begin();
            if (this.OnAvatarShowEnded != null)
            {
                this.OnAvatarShowEnded();
            }

        }

        private void tbAvatar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0)
            {
                tbAvatarScale.CenterX = e.NewSize.Width / 2;
                tbAvatarScale.CenterY = e.NewSize.Height;
            }
        }
    }

    public delegate void AvatarShowEnded();
}
