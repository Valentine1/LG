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
    public sealed partial class ExplosionV : UserControl
    {
        public event ShouldBeRemoved OnShouldBeRemoved;

        private ExplosionVM ExplosionVModel { get; set; }

        public ExplosionV()
        {
            this.InitializeComponent();
        }
        public ExplosionV(ExplosionVM explVM)
        {
            this.InitializeComponent();
            this.ExplosionVModel = explVM;
            this.DataContext = explVM;
            this.ExplosionVModel.OnItselfDeleted += ExplosionVModel_OnItselfDeleted;
        }


        private void ExplosionVModel_OnItselfDeleted(UnitVM pbvm)
        {
            this.ExplosionVModel.OnItselfDeleted -= ExplosionVModel_OnItselfDeleted;
            if (this.OnShouldBeRemoved != null)
            {
                this.OnShouldBeRemoved(this);
            }
            this.ExplosionVModel = null;
        }
    }
}
