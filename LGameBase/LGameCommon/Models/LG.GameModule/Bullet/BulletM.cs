using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public class BulletM : BaseBlock
    {
        private BulletControls _controls;
        public BulletControls Controls
        {
            get
            {
                if (_controls == null)
                {
                    _controls = new BulletControls(this);

                }
                return _controls;
            }
        }

      
    }

}
