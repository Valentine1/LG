using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class LaserM : AssetM
    {
        private BaseBlock TargetBlock;

        private BaseControls _controls;
        public override BaseControls Controls
        {
            get
            {
                if (_controls == null)
                {
                    _controls = new LaserControls(this, this.TargetBlock);
                }
                return _controls;
            }
        }

        public LaserM(BaseBlock target)
        {
            this.TargetBlock = target;
        }
    }
}
