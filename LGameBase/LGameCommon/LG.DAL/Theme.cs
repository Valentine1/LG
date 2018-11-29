using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace LG.Data
{
    public class Theme : Item
    {
        public List<Symbol> NameInLetters { get; set; }
        public List<Word> Words { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }
        public string NativeName { get; set; }
        public License LicenseInfo { get; set; }
        public Level HLevel { get; set; }
        public string PathData { get; set; }
        public bool IsPreviewMode { get; set; }
        public bool IsPreviewsLoaded { get; set; }
        public bool IsResourcesLoaded { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
