using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Data;
using LG.XmlData;
using LGservices;

namespace LG.Data
{
    public partial class DataLoaderFactory
    {
        private WebProfileDataLoader _wprofDataLoader;
        private WebProfileDataLoader WprofDataLoader
        {
            get
            {
                if (_wprofDataLoader == null)
                {
                    _wprofDataLoader = new WebProfileDataLoader();
                }
                return _wprofDataLoader;
            }
        }


        public IDataLoader GetDataLoader(string path)
        {
            return this.GetLoader(path);
        }
        public IThemeDataLoader GetThemeDataLoader()
        {
            return new WebThemeDataLoader();
        }

        public IProfileDataLoader GetProfileDataLoader(ProfileType ptype, string path)
        {
            switch (ptype)
            {
                case ProfileType.Local:
                    return this.GetProfileLoader(path);
                case ProfileType.Internet:
                    return WprofDataLoader; 
            }
            return this.GetProfileLoader(path);
        }
        private IProfileDataLoader GetProfileLoader(string path)
        {
            return new DataLoader(path);
        }

        public void ResetServiceConnection()
        {
            ServiceConnection.ResetEndpoint();
        }
    }
}
