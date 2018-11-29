using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class AssetControls : BaseControls
    {
        private AssetM DirectedAsset
        {
            get
            {

                return (AssetM)this.Block;
            }
        }

        public AssetControls(AssetM pb) : base(0, SpaceParams.BlockSpeed)
        {
            _block = pb;
            SpaceParams.OnBlockSpeedChanged += SpaceParams_OnBlockSpeedChanged;
        }

        private void SpaceParams_OnBlockSpeedChanged(double ajustedSpeed, double absoluteSpeed, bool isSmallChange)
        {
            this.SpeedY = ajustedSpeed;
        }

        protected override void Move()
        {
            base.Move();
        }

        public override void DetachEvents()
        {
            SpaceParams.OnBlockSpeedChanged -= SpaceParams_OnBlockSpeedChanged;
        }
    }
}
