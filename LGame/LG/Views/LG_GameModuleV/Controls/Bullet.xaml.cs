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
using LG.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class Bullet : UserControl, IUnitV
    {
        public event ShouldBeRemoved OnShouldBeRemoved;

        public Bullet(BulletVM bm)
        {
            this.InitializeComponent();
            this.DataContext = bm;

            bm.OnItselfDeleted += BulletModel_OnItselfDeleted;
            bm.OnYChanged += BulletVModel_OnYChanged;
        }

        void BulletVModel_OnYChanged(double y)
        {
            bulletMovement.Y = y;
        }

        private void BulletModel_OnItselfDeleted(UnitVM bul)
        {
            this.DetachEvents(bul as BulletVM);
            if (this.OnShouldBeRemoved != null)
            {
                this.OnShouldBeRemoved(this);
            }
        }


        private void DetachEvents(BulletVM bm)
        {
            bm.OnItselfDeleted -= BulletModel_OnItselfDeleted;
            bm.OnItselfDeleted -= BulletModel_OnItselfDeleted;
        }


      
    }


}
