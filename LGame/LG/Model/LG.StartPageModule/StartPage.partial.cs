using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Data;

namespace LG.Models
{
   public partial class StartPage: Module
    {
        public async override Task Initialize()
        {
            base.Initialize();
            StartPageParams.Initialize();
            this.InitSizesAndPositions();
            this.ProfilesMgr.OnProgressShowStart += ProfilesMgr_OnProgressShowStart;
            this.ProfilesMgr.OnProgressShowEnd += ProfilesMgr_OnProgressShowEnd;
            await this.ProfilesMgr.Initialize();

        }

    }
}
