using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public class SpaceGridRow : BaseBlock
    {
        private ObservableCollection<SpaceGridCell> _cells;
        public ObservableCollection<SpaceGridCell> Cells
        {
            get
            {
                if (_cells == null)
                {
                    _cells = new ObservableCollection<SpaceGridCell>();
                }
                return _cells;
            }
        }

        private static int LastNebulaCellIndex { get; set; }

        internal void FillWithCells()
        {
            this.AddStarCells();
            this.AddNebulaCell();
        }

        internal void DisposeCells()
        {
            foreach (SpaceGridCell cell in this.Cells)
            {
                cell.DeActivate();
            }
            this.Cells.Clear();
        }

        private void AddStarCells()
        {
            for (int i = 0; i < BackgroundParams.CellsCount; i++)
            {
                int index = BackSpaceGrid.Rand.Next(BackSpaceGrid.CellsStoreIndexes.Count);
                SpaceGridCell cell = BackSpaceGrid.CellsStore[BackSpaceGrid.CellsStoreIndexes[index]];
                this.Cells.Add(cell);
                cell.PositionY = this.StartPosition.Y + this.PositionY;
                cell.PositionX = i * BackgroundParams.CellWidth;
                BackSpaceGrid.CellsStoreIndexes.RemoveAt(index);
                cell.Activate();
            }
        }

        private void AddNebulaCell()
        {
            int cellNebulaIndex = this.CalculateIndexForNebulaCell();

            int storeNebulaIndex = BackSpaceGrid.Rand.Next(BackSpaceGrid.CellsNebulaStoreIndexes.Count);
            SpaceGridCellNebula cellNebula = BackSpaceGrid.CellsNebulaStore[BackSpaceGrid.CellsNebulaStoreIndexes[storeNebulaIndex]];
            BackSpaceGrid.CellsNebulaStoreIndexes.RemoveAt(storeNebulaIndex);
            SpaceGridRow.LastNebulaCellIndex = cellNebulaIndex;
            this.Cells.Add(cellNebula);

            int ry = (int)Math.Truncate(0.37 * BackgroundParams.CellWidth);
            cellNebula.PositionY = this.StartPosition.Y + this.PositionY + BackSpaceGrid.Rand.Next(-ry, ry);
            int rx = (int)Math.Truncate(0.4 * BackgroundParams.CellWidth);
            cellNebula.PositionX = cellNebulaIndex * BackgroundParams.CellWidth + BackSpaceGrid.Rand.Next(-rx, rx);
        }

        private int CalculateIndexForNebulaCell()
        {
            int cellNebulaIndex = 0;
            if (SpaceGridRow.LastNebulaCellIndex == 0)
            {
                cellNebulaIndex = BackSpaceGrid.Rand.Next(1, BackgroundParams.CellsCount);
            }
            else if (SpaceGridRow.LastNebulaCellIndex == BackgroundParams.CellsCount - 1)
            {
                cellNebulaIndex = BackSpaceGrid.Rand.Next(BackgroundParams.CellsCount - 1);
            }
            else
            {
                int i1 = BackSpaceGrid.Rand.Next(0, SpaceGridRow.LastNebulaCellIndex);
                int i2 = BackSpaceGrid.Rand.Next(SpaceGridRow.LastNebulaCellIndex + 1, BackgroundParams.CellsCount);
                int iSelector = BackSpaceGrid.Rand.Next(0, 2);
                cellNebulaIndex = iSelector == 0 ? i1 : i2;
            }
            return cellNebulaIndex;
        }
    }
}
