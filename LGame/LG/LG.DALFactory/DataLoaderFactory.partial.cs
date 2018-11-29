using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Data;
using LG.XmlData;

namespace LG.Data
{
    public partial class DataLoaderFactory
    {
        private DataLoader _profDataLoader;
        private IDataLoader GetLoader(string path)
        {
            if (_profDataLoader == null)
            {
                return new DataLoader(path);
            }
            return _profDataLoader;
        }

     
    }
}
