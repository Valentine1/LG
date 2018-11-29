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
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LG.ViewModels;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class SpaceGridCellV : UserControl
    {
        public SpaceGridCellV()
        {
            this.InitializeComponent();
        }

        public void Initialize(SpaceGridCellVM scvm)
        {
            scvm.OnXChanged += sCellvm_OnXChanged;
            scvm.OnYChanged += sCellvm_OnYChanged;

            foreach (AssetVM avm in scvm.Stars)
            {
                Image imStar = new Image(); 
                imStar.Opacity = 1;
                imStar.Source = avm.PictureSource;
                canStars.Children.Add(imStar);
                imStar.SetValue(Canvas.TopProperty, avm.StartPosition.Y);
                imStar.SetValue(Canvas.LeftProperty, avm.StartPosition.X);
                imStar.Width = avm.BlockSize.Width;
                imStar.Height = avm.BlockSize.Height;
                imStar.Stretch = Stretch.Uniform;
            }
            foreach (BlinkStarVM bsvm in scvm.BlinkStarsVMs)
            {
                BlinkStarV bsv = new BlinkStarV();
                bsv.Initialize(bsvm);
                canStars.Children.Add(bsv);
            }
            scvm.Stars.CollectionChanged += Stars_CollectionChanged;
        }

        private void Stars_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AssetVM svm = (AssetVM)e.NewItems[0];
                Image imStar = new Image();
           
                imStar.Source = svm.PictureSource;
                canStars.Children.Add(imStar);
                imStar.SetValue(Canvas.TopProperty, svm.StartPosition.Y);
                imStar.SetValue(Canvas.LeftProperty, svm.StartPosition.X);
            }
        }

        private void sCellvm_OnYChanged(double y)
        {
            trTrans.Y = y;
        }

        private void sCellvm_OnXChanged(double x)
        {
            trTrans.X = x;
        }

    }
}
