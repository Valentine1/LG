using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class BlinkStar : AssetM
    {
        public event BlinkStarting OnBlinkStarting;

        internal void Blink(double expandTo)
        {
            if (this.OnBlinkStarting != null)
            {
                this.OnBlinkStarting(expandTo);
            }
        }
    }

    public delegate void BlinkStarting(double expandTo);
}
