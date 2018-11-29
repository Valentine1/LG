using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Data
{
    public class ProfileDAL
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ContactInfo { get; set; }
        public string PictureUrl { get; set; }
        public byte[] Picture { get; set; }
        public int Type { get; set; }
        public int  LastThemeID { get; set; }
        public int TimePlayed { get; set; }
        public int BestTime { get; set; }
        public bool HasPhoto { get; set; }
        public List<Theme> Themes { get; set; }
    }

    public enum ProfileType { Local = 0, Internet = 1 }
}
