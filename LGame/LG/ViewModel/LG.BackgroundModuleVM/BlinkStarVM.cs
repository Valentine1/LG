using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class BlinkStarVM : AssetVM
    {
        public event BlinkStartingVM OnBlinkStartingVM;

        public BlinkStarVM(BlinkStar bstar) : base(bstar)
        {
            bstar.OnBlinkStarting += bstar_OnBlinkStarting;
        }

        private void bstar_OnBlinkStarting(double expandTo)
        {
            if (this.OnBlinkStartingVM != null)
            {
                this.OnBlinkStartingVM(expandTo);
            }
        }
    }

    public delegate void BlinkStartingVM(double expandTo);
}
