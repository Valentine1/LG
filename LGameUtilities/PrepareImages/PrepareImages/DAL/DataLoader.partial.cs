using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using LG.Common;
using LG.Data;

namespace LG.XmlData
{
    public partial class DataLoader
    {
      
        private async Task<XDocument> LoadAsync(string path)
        {
            return XDocument.Load(path);

        }
        async private Task Save(XDocument xdoc, string path)
        {
            xdoc.Save(path);
        }


        private XDocument Load(string path)
        {
            return XDocument.Load(path);
        }
      
    }
}
