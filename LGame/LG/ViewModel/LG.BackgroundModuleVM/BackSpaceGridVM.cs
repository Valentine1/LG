using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class BackSpaceGridVM:BaseBlockVM
    {
        private Dictionary<int,int> _cellsIDs;
        private Dictionary<int, int> CellsIDs
        {
            get
            {
                if (_cellsIDs == null)
                {
                    _cellsIDs = new Dictionary<int, int>();
                }
                return _cellsIDs;
            }
        }

        private ObservableCollection<SpaceGridCellVM> _cellsVModels;
        public ObservableCollection<SpaceGridCellVM> CellsVModels
        {
            get
            {
                if (_cellsVModels == null)
                {
                    _cellsVModels = new ObservableCollection<SpaceGridCellVM>();
                }
                return _cellsVModels;
            }
        }

        public BackSpaceGridVM(BackSpaceGrid sgrid):base(sgrid)
        {
            foreach (SpaceGridRow row in sgrid.SpaceRows)
            {
                row.Cells.CollectionChanged += Cells_CollectionChanged;
            }
        }

        private void Cells_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                SpaceGridCell cell = (SpaceGridCell)e.NewItems[0];
                if (this.CellsIDs.ContainsKey(cell.ID))
                {
                    SpaceGridCellVM cvm = (from c in this.CellsVModels where c.ID == cell.ID select c).SingleOrDefault();
                    cvm.Opacity = 1.0;
                }
                else
                {
                    SpaceGridCellVM cvm = new SpaceGridCellVM(cell);
                    cvm.Opacity = 1.0;
                    this.CellsIDs.Add(cell.ID, 0);
                    this.CellsVModels.Add(cvm);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                SpaceGridCell cell = (SpaceGridCell)e.OldItems[0];
                SpaceGridCellVM cvm = (from c in this.CellsVModels where c.ID == cell.ID select c).SingleOrDefault();
                cvm.Opacity = 0;
            }
        }

    }
}
