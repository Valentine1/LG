using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class BulletVM : BaseBlockVM
    {
        #region model
        private PictureBoxM Bullet
        {
            get
            {
                return (PictureBoxM)this.BaseBlockM;
            }
        }
        #endregion

        public BulletVM(BulletM bul)
            : base(bul)
        {

        }


    }
}
