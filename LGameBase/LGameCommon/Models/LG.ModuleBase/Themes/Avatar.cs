using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public partial class Avatar : AssetM
    {
        static public int MaxLevel = 13;
        public int LevelNo { get; set; }
        public Color TopBorderClr { get; set; }
        public Color BottomBorderClr { get; set; }

        public int BestTimeResult { get; set; }
        public double TimeThreshold { get; set; }

        public string PictureUrl { get; set; }
        public Avatar()
        {
        }

        public Avatar(Level lev)
        {
            this.TextValue = lev.Name;
            this.LevelNo = lev.Number;
            this.PictureUrl = lev.PictureUrl;
            this.TopBorderClr = lev.TopColor;
            this.BottomBorderClr = lev.BottomColor;
        }
    }
}
