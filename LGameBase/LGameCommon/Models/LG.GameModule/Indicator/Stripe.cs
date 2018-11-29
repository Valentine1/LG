using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public partial class Stripe: Unit
    {
        public event ColorChanged OnColorChanged;

        private Color _clr;
        public Color Clr
        {
            get
            {
                return _clr;
            }
            set
            {
                _clr = value;
                if (this.OnColorChanged != null)
                {
                    this.OnColorChanged(_clr);
                }
            }
        }
        public Rect Area { get; set; }
        public Thickness Margin { get; set; }
    }

    public delegate void ColorChanged(Color Clr);
}
