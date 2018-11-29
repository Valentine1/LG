using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class ExhaustFireVM : AssetVM
    {

        internal ExhaustFireM ExhaustFire
        {
            get
            {
                return (ExhaustFireM)this.BaseBlockM;
            }
        }

        public ExhaustFireVM(ExhaustFireM ef)
            : base(ef)
        {
        }

        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);
        }
    }
}
