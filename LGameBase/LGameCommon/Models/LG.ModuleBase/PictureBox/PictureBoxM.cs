using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public partial class PictureBoxM : AssetM
    {
        public event PictureBoxColorsChanged OnPictureBoxColorsChanged;  

        public int WordID { get; set; }
        private PictureBoxColors _boxColors;
        public PictureBoxColors BoxColors
        {
            get
            {
                return _boxColors;
            }
            set
            {
                _boxColors = value;
                if (this.OnPictureBoxColorsChanged != null)
                {
                    this.OnPictureBoxColorsChanged(_boxColors);
                }
            }
        }
        public string TextValue { get; set; }
        public bool IsThisPictureSpoken { get; set; }
    }

    public delegate void PictureBoxColorsChanged(PictureBoxColors pbc);
}
