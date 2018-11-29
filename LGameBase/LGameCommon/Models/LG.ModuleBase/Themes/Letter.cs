using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Data;
using LG.Common;

namespace LG.Models
{
    public class Letter : AssetM
    {

        public Letter(Symbol s)
        {
            this.TextValue = s.Value;
            this.StartPosition = new Point() { X = s.Left, Y = s.Top };
            this.Rotation = s.Rotation;
        }
    }
}
