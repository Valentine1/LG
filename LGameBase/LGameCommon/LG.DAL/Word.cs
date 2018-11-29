using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Data
{
    public class Word : Item
    {
        public int ID { get; set; }
        public string PictureUrl { get; set; }
        public string AudioUrl { get; set; }
        public Color BackColor { get; set; }
        public Color BorderColor { get; set; }
        public Color LeftColor { get; set; }
        public Color TopColor { get; set; }
        public Color LeftTopColor { get; set; }
    }
}
