using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class UnitVM : BaseNotify
    {
        public event ItselfDeleted OnItselfDeleted;

        protected  Unit _unitM;
        protected Unit UnitM
        {
            get
            {
                return _unitM;
            }
            set
            {
                _unitM = value;
            }
        }

        public UnitVM()
        {

        }

        public UnitVM(Unit unit)
        {
            this._unitM = unit;
            this.UnitM.OnItselfDeleted += UnitM_OnItselfDeleted;
        }

        protected void UnitM_OnItselfDeleted(Unit m)
        {
            this.DeleteItself(m);
        }
        public virtual void DeleteItself(Unit m)
        {
            this.DetachEvents(m);
            if (this.OnItselfDeleted != null)
            {
                this.OnItselfDeleted(this);
            }
            UnitM = null;
        }
        public virtual void DetachEvents(Unit m)
        {
            this.UnitM.OnItselfDeleted -= UnitM_OnItselfDeleted;
        }
    }

    public delegate void ItselfDeleted(UnitVM uvm);
}
