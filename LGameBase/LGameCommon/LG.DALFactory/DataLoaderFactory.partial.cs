using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Data;
using LG.XmlData;

namespace LG.Data
{
    public partial class DataLoaderFactory
    {
        private IDataLoader GetLoader(string path)
        {
            return new DataLoader(path);
        }
    }
}
