using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LG.Common;

namespace LG.Models
{
    public partial class IndicatorMoving : Indicator
    {
        public event DecreaseBegin OnDecreaseBegin;

        public IndicatorMoving(int stripeNumber, Color clr, IndicatorType itype)
            : base(stripeNumber, clr, itype, true)
        {
            this.FlashingEnabled = true;
        }

        public void Decrease()
        {
            if (Emptyammo < this.Capacity)
            {
                if (this.OnDecreaseBegin != null)
                {
                    this.OnDecreaseBegin();
                }
            }
        }

        public void EndDecrease()
        {
            ++Emptyammo;
            this.Panel[Emptyammo].Clr = this.StripeEmptyClr;
            if (this.Ammount == 3)
            {
                this.ChangeColorOfUnderChargedStripes(new Color() { A = 255, R = 255, G = 255, B = 0 });
            }
            if (this.Ammount == 1)
            {
                this.ChangeColorOfUnderChargedStripes(new Color() { A = 255, R = 255, G = 0, B = 0 });
            }
        }

        public void Recharge()
        {
            for (int i = 1; i <= this.Capacity; i++)
            {
                 this.Panel[i].Clr = this.StripeFullClr;
            }
            this.Emptyammo = 0;
        }
    }


    public delegate void DecreaseBegin();
}
