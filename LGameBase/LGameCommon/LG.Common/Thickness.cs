using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Common
{
    public class Thickness
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
        public Thickness()
        {
        }
        public Thickness(double th)
        {
            this.Top = th;
            this.Left = th;
            this.Right = th;
            this.Bottom = th;
        }
        public Thickness(double l, double t, double r, double b)
        {
            this.Top = l;
            this.Left = t;
            this.Right = r;
            this.Bottom = b;
        }
    }
}
