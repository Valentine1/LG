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
    public class ExplosionVM: AssetVM
    {
        internal ExplosionM Explosion
        {
            get
            {
                return (ExplosionM)this.BaseBlockM;
            }
        }

        public ExplosionVM(ExplosionM expl)
            : base(expl)
        {
        }
    }
}
