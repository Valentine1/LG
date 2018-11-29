using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class SaluteVM : AssetVM
    {
        internal SaluteM Salute
        {
            get
            {
                return (SaluteM)this.BaseBlockM;
            }
        }

        public SaluteVM(SaluteM sal)
            : base(sal)
        {
        }
    }
}
