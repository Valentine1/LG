using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class LaserControls : SelfHomingBaseControls
    {
        public LaserM Laser
        {
            get
            {
                return (LaserM)_block;
            }
        }

        public LaserControls(BaseBlock block, BaseBlock target) : base(block, target)
        {

        }
    }
}
