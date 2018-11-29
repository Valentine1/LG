using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class SpaceGridCellNebula: SpaceGridCell
    {
        internal override void DeActivate()
        {
            this.BlinkStarTimer.Stop();
            BackSpaceGrid.CellsNebulaStoreIndexes.Add(this.ID);
        }
    }
}
