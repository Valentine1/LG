using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using Windows.UI;
using LG.Models;

namespace LG.ViewModels
{
    public class AmmoVM : AssetVM
    {
        internal AmmoM AmmoM
        {
            get
            {
                return (AmmoM)this.BaseBlockM;
            }
        }

        public AmmoVM(AmmoM ammoM)
            : base(ammoM)
        {
        }
    }
}
