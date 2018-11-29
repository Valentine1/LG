using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Common
{
    public class Color
    {
        // Summary:
        //     Gets or sets the sRGB alpha channel value of the color.
        //
        // Returns:
        //     The sRGB alpha channel value of the color.
        public byte A { get; set; }

        public byte B { get; set; }

        public byte G { get; set; }

        public byte R { get; set; }

        public string HexString
        {
            set
            {
                char[] hn = value.ToCharArray();
                this.A = 255;

                string r = hn[0].ToString() + hn[1].ToString();
                this.R = byte.Parse(r, System.Globalization.NumberStyles.HexNumber);

                string g = hn[2].ToString() + hn[3].ToString();
                this.G = byte.Parse(g, System.Globalization.NumberStyles.HexNumber);

                string b = hn[4].ToString() + hn[5].ToString();
                this.B = byte.Parse(b, System.Globalization.NumberStyles.HexNumber);
            }
        }

        public Color(string clr)
        {
            char[] hn = clr.ToCharArray();
            string a = hn[1].ToString() + hn[2].ToString();
            this.A = byte.Parse(a, System.Globalization.NumberStyles.HexNumber);

            string r = hn[3].ToString() + hn[4].ToString();
            this.R = byte.Parse(r, System.Globalization.NumberStyles.HexNumber);

            string g = hn[5].ToString() + hn[6].ToString();
            this.G = byte.Parse(g, System.Globalization.NumberStyles.HexNumber);

            string b = hn[7].ToString() + hn[8].ToString();
            this.B = byte.Parse(b, System.Globalization.NumberStyles.HexNumber);
        }
        public Color()
        {
        }

        public string ToHexString()
        {
            string r = R.ToString("X");
            r = r.Length == 1 ? "0" + r : r;

            string g = G.ToString("X");
            g = g.Length == 1 ? "0" + g : g;

            string b = B.ToString("X");
            b = b.Length == 1 ? "0" + b : b;

            return "#FF" + r + g + b;
        }
    }
}
