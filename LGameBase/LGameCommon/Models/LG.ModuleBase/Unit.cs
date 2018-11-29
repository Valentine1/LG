using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public abstract class Unit
    {
        public event ItselfDeleted OnItselfDeleted;
        public virtual void DeleteItself()
        {
            this.DetachEvents();
            if (OnItselfDeleted != null)
            {
                OnItselfDeleted(this);
            }
        }

        public virtual void DetachEvents()
        {

        }
    }

    public delegate void ItselfDeleted(Unit m);

}
