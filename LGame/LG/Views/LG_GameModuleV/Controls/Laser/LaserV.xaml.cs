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
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using LG.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class LaserV : UserControl
    {
        public event ShouldBeRemoved OnShouldBeRemoved;

        public LaserV()
        {
            this.InitializeComponent();
        }
        public LaserV(AssetVM lasVM)
        {
            this.InitializeComponent();
            this.DataContext = lasVM;
            imControl.Source = lasVM.PictureSource;
            lasVM.OnPictureSourceChanged += lasVM_OnPictureSourceChanged;
            lasVM.OnItselfDeleted += LaserVModel_OnItselfDeleted;
        }

        private void lasVM_OnPictureSourceChanged(BitmapImage im)
        {
            imControl.Source = im;
        }

        private void LaserVModel_OnItselfDeleted(UnitVM pbvm)
        {
            (pbvm as AssetVM).OnItselfDeleted -= LaserVModel_OnItselfDeleted;
            (pbvm as AssetVM).OnPictureSourceChanged -= lasVM_OnPictureSourceChanged;
            if (this.OnShouldBeRemoved != null)
            {
                this.OnShouldBeRemoved(this);
            }
            imControl.Source = null;
        }
    }
}
