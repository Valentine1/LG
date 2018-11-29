using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Data
{
    public class Level
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }

        public Color TopColor { get; set; }
        public Color BottomColor { get; set; }

        public int BestTimeResult { get; set; }
    }
}
