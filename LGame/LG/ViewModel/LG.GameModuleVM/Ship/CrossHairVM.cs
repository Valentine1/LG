using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using LG.Models;

namespace LG.ViewModels
{
    public class CrossHairVM : AssetVM
    {
         private AssetVM _laserBeamVM;
         public AssetVM LaserBeamVM
         {
             get
             {
                 return _laserBeamVM;
             }
             set
             {
                 _laserBeamVM = value;
                 NotifyPropertyChanged("LaserBeamVM");
             }

         }

        public CrossHairVM(CrossHair ch): base(ch)
        {
            this._laserBeamVM = new AssetVM(ch.LaserBeam);
            ch.OnStartPositionChanged -= this.BaseBlockM_OnStartPositionChanged;
        }

    }
}
