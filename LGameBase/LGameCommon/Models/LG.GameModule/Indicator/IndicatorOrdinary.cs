using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LG.Common;

namespace LG.Models
{
    public class IndicatorOrdinary : Indicator
    {
        public IndicatorOrdinary(int stripeNumber, Color clr, IndicatorType itype)
            : base(stripeNumber, clr, itype, false)
        {
            this.FlashingEnabled = false;
        }

        public virtual void Encrease()
        {
            if (Emptyammo > 0)
            {
                this.Panel[Emptyammo].Clr = this.StripeFullClr;
                --Emptyammo;
            }
        }

        public void Clear()
        {
            for (int i = 1; i <= this.Capacity; i++)
            {
                this.Panel[Emptyammo].Clr = this.StripeEmptyClr;
            }
            this.Emptyammo = this.Capacity;
        }
    }
}
