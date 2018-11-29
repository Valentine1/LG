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
using Windows.Storage;
using LG.ViewModels;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class AssetV : UserControl
    {
       public event ShouldBeRemoved OnShouldBeRemoved;

       private AssetVM AssetVModel { get; set; }

        public AssetV()
        {
            this.InitializeComponent();
          
        }
        public AssetV(AssetVM assetVM)
        {
            this.InitializeComponent();
            this.AssetVModel = assetVM;
            this.DataContext = assetVM;
            this.AssetVModel.OnItselfDeleted += AssetVModel_OnItselfDeleted;
        }

      
        protected void DisableMovement()
        {
            this.RenderTransform = null;
           
        }
        private void AssetVModel_OnItselfDeleted(UnitVM pbvm)
        {
            this.AssetVModel.OnItselfDeleted -= AssetVModel_OnItselfDeleted;
            if (this.OnShouldBeRemoved != null)
            {
                this.OnShouldBeRemoved(this);
            }
            this.AssetVModel = null;
            this.DataContext = null;
        }

    }
}
