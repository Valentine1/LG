using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class BaseControls : Unit
    {
     
        protected BaseBlock _block;
        protected BaseBlock Block
        {
            get
            {
                return _block;
            }
        }

        protected double SpeedY;  //   pixels/ms
    
        protected double SpeedX;//   pixels/ms
        protected double DeltaTimeFromPreviousFrame { get; set; }

        public bool IsMovingVertical { get; set; }
        public bool IsMovingHorizontal { get; set; }

        public BaseControls(double speedx, double speedy)
        {
            this.SpeedX = speedx;
            this.SpeedY = speedy;
        }
        public BaseControls()
        {
        }
        public virtual void TimeElapses(double deltaTime)
        {
            this.DeltaTimeFromPreviousFrame = deltaTime;
            this.Move();
        }

        double rest;
        protected virtual void Move()
        {
            if (IsMovingHorizontal)
            {
                this.Block.MoveHorizontal(this.DeltaTimeFromPreviousFrame * this.SpeedX);
            }
            if (this.IsMovingVertical)
            {
                this.Block.MoveVertical(this.DeltaTimeFromPreviousFrame * this.SpeedY);
                //double dist = this.DeltaTimeFromPreviousFrame * this.SpeedY + rest;
                //double f = Math.Floor(dist);
                //if (f > 0)
                //{
                //    this.Block.MoveVertical(f);
                //    rest = dist % f;
                //}
                //else
                //{
                //    rest += dist;
                //}
            }
        }
    }

}
