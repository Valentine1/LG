using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Common
{
    public partial class Settings
    {
        static private string _themesUpdaterWebServiceUrl;
        static public string ThemesUpdaterWebServiceUrl
        {
            get
            {
                return _themesUpdaterWebServiceUrl;
            }
        }

    }
}
