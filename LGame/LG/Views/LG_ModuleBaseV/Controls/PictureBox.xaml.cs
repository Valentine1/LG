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
    public partial class PictureBox : UserControl, IUnitV
    {
        public event ShouldBeRemoved OnShouldBeRemoved;

        public PictureBoxVM PictureBoxVModel { get; set; }
        public int WordID { get; set; }
        public PictureBox()
        {
            this.InitializeComponent();
        }
        public PictureBox(PictureBoxVM picVM)
        {
            this.InitializeComponent();
            Initialize(picVM);
        }
        public PictureBox(PictureBoxVM picVM, bool isStatic)
        {
            this.InitializeComponent();
            Initialize(picVM);
            if (isStatic)
            {
                this.DisableMovement();
            }
        }
        public void Initialize(PictureBoxVM picVM)
        {
            WordID = picVM.WordID;
            this.PictureBoxVModel = picVM;
            this.DataContext = picVM;
            imControl.Source = picVM.PictureSource;
            this.PictureBoxVModel.OnItselfDeleted += PictureBoxVModel_OnItselfDeleted;
            this.PictureBoxVModel.OnPictureSourceChanged += PictureBoxVModel_OnPictureSourceChanged;
        }
    
        protected void DisableMovement()
        {
            this.RenderTransform = null;

        }
        private void PictureBoxVModel_OnPictureSourceChanged(BitmapImage im)
        {
            imControl.Source = im;
        }
        public void DeleteItself()
        {
            this.DetachEvents();
            if (this.OnShouldBeRemoved != null)
            {
                this.OnShouldBeRemoved(this);
            }
            this.PictureBoxVModel = null;
        }
        private void PictureBoxVModel_OnItselfDeleted(UnitVM pbvm)
        {
            DeleteItself();
        }
        public void DetachEvents()
        {
            this.PictureBoxVModel.OnItselfDeleted -= PictureBoxVModel_OnItselfDeleted;
            this.PictureBoxVModel.OnPictureSourceChanged -= PictureBoxVModel_OnPictureSourceChanged;
          
            this.DataContext = null;
            imControl.Source = null;
        }

    }
}
