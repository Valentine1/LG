using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LG.Models
{
    public partial class Module 
    {
        public static string LocalStoaragePath
        {
            get
            {
                return ApplicationData.Current.LocalFolder.Path;
            }
        }
    }

}
