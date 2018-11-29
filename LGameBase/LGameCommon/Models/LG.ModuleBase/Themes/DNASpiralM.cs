using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class DNASpiralM : SpiralM
    {
        //double d = 1.0;
        //double dy = 1.0;
        //double limit = 6.0;
        //double limity = 12.0;
        public DNASpiralM(double w, double h)
            : base(w, h)
        {
            this.Dx = 1.0;
            this.Dy = 1.0;
            this.LimitxBottom = 3.0;
            this.LimitxTop = 6.0;

            this.LimityBottom = 9.0;
            this.LimityTop = 12.0;

            this.Limitx = LimitxTop;
            this.Limity = LimityTop;
        }
    }
}
