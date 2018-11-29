using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class BulletControls : BaseControls
    {
        private BulletM DirectedBullet
        {
            get
            {

                return (BulletM)_block;
            }
        }

        public BulletControls(BulletM bullet)
            : base(0, -SpaceParams.BulletSpeed)
        {
            _block = bullet;
        }

        protected override void Move()
        {
            base.Move();
        }
    }
}
