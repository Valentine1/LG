using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public sealed partial class BackSpaceGridV : UserControl
    {
        public BackSpaceGridV()
        {
            this.InitializeComponent();
        }

        public void Initialize(BackSpaceGridVM gridVM)
        {
            gridVM.CellsVModels.CollectionChanged += CellsVModels_CollectionChanged;
            gridVM.OnYChanged += gridVM_OnYChanged;
        }

        private void gridVM_OnYChanged(double y)
        {
            sgTrans.Y = y;
        }

        private void CellsVModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                SpaceGridCellVM cvm = (SpaceGridCellVM)e.NewItems[0];
                
                SpaceGridCellV cview= new SpaceGridCellV();
                cview.Initialize(cvm);
                cnvBackSpace.Children.Add(cview);

            }
        }

    }
}
