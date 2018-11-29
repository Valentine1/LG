using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class SpaceMapVM : BaseBlockVM
    {
        #region model
        private SpaceMap MapVodel
        {
            get
            {
                return (SpaceMap)this.BaseBlockM;
            }
        }
        #endregion

        private readonly SpaceMapAdvancerVM _advancerVM;
        public SpaceMapAdvancerVM AdvancerVM
        {
            get
            {
                return _advancerVM;
            }
        }

        public SpaceMapVM(SpaceMap map):base(map)
        {
            _advancerVM = new SpaceMapAdvancerVM(map.AdvancerControls.Advancer);
        }
    }
}
