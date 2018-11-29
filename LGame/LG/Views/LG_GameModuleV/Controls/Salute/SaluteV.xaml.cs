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
    public sealed partial class SaluteV : UserControl
    {
        public event ShouldBeRemoved OnShouldBeRemoved;

        public SaluteV()
        {
            this.InitializeComponent();
        }
        public SaluteV(SaluteVM salVM)
        {
            this.InitializeComponent();
            this.DataContext = salVM;
            salVM.OnItselfDeleted += SaluteVModel_OnItselfDeleted;

        }

        private void SaluteVModel_OnItselfDeleted(UnitVM pbvm)
        {
            pbvm.OnItselfDeleted -= SaluteVModel_OnItselfDeleted;
            if (this.OnShouldBeRemoved != null)
            {
                this.OnShouldBeRemoved(this);
            }
        }
    }
}
