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
using LG.ViewModels;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class ExhaustFireV : UserControl
    {
        public ExhaustFireV()
        {
            this.InitializeComponent();
        }

        public void Initialize(ExhaustFireVM fireVM)
        {
            fireVM.OnPictureSourceChanged += fireVM_OnPictureSourceChanged;
            fireVM.OnItselfDeleted += fireVM_OnItselfDeleted;
        }

        private void fireVM_OnItselfDeleted(UnitVM uvm)
        {
            DetachEvents(uvm as ExhaustFireVM);
        }
        private void fireVM_OnPictureSourceChanged(BitmapImage im)
        {
            imFire.Source = im;
        }

        public void DetachEvents(ExhaustFireVM fvm)
        {
            fvm.OnPictureSourceChanged -= fireVM_OnPictureSourceChanged;
        }
    }
}
