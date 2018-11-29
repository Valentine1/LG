using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LG.Common;
using LG.Data;


namespace LG.Models
{
    public class StarShipM : BaseBlock
    {
        public event LeftExhaustCreated OnLeftExhaustCreated;
        public event RightExhaustCreated OnRightExhaustCreated;
        public event SpeedXChanged OnSpeedXChanged;

        private ObservableCollection<BulletM> firedBullets;
        public ObservableCollection<BulletM> FiredBullets
        {
            get
            {
                if (firedBullets == null)
                {
                    firedBullets = new ObservableCollection<BulletM>();
                }
                return firedBullets;
            }
        }
      
        private readonly ExhaustFireM _leftExhaust;
        public ExhaustFireM LeftExhaust
        {
            get
            {
                return _leftExhaust;
            }
        }
        private readonly ExhaustFireM _rightExhaust;
        public ExhaustFireM RightExhaust
        {
            get
            {
                return _rightExhaust;
            }
        }
        
        private ExhaustFireInitializer  _exFireIniter;
        private ExhaustFireInitializer ExFireIniter
        {
            get
            {
                if (_exFireIniter == null)
                {
                    _exFireIniter = new ExhaustFireInitializer();
                }
                return _exFireIniter;
            }
        }
        private double _speedX;
        public double SpeedX
        {
            get
            {
                return _speedX;
            }
            set
            {
                _speedX = value;
                if (this.OnSpeedXChanged != null)
                {
                    this.OnSpeedXChanged(_speedX);
                }
            }
        }//   pixels/ms
        public StarShipM()
        {
            _leftExhaust = new ExhaustFireM(AnimationBehavior.Forever, 100);
            _rightExhaust = new ExhaustFireM(AnimationBehavior.Forever, 100);
        }

        public void TimeElapses(double deltaTime)
        {
            this.DeliverElapseTimeToBullets(deltaTime);
        }
        public void Initialize()
        {
            this.ExFireIniter.OnImageSourcesInitialized += ExFireIniter_OnImageSourcesInitialized;
            this.ExFireIniter.InitializeExhaustFireSource();
        }

        private void ExFireIniter_OnImageSourcesInitialized()
        {
            this.ExFireIniter.AssignPictureBitmaps(this.LeftExhaust, true);
            if (this.OnLeftExhaustCreated != null)
            {
                this.OnLeftExhaustCreated(this.LeftExhaust);
            }
            this.ExFireIniter.AssignPictureBitmaps(this.RightExhaust, true);
            if (this.OnRightExhaustCreated != null)
            {
                this.OnRightExhaustCreated(this.RightExhaust);
            }
        }
        private void DeliverElapseTimeToBullets(double deltaTime)
        {
            foreach (BulletM bul in this.FiredBullets)
            {
                bul.Controls.TimeElapses(deltaTime);
            }
        }
        internal void CreateBulletAndSetInitialPos()
        {
            BulletM bul = new BulletM();
            bul.StartPosition = new Point() { X = this.PositionX + this.StartPosition.X + (this.BlockSize.Width / 2-3), Y = this.StartPosition.Y };
            bul.Controls.IsMovingVertical = true;
            this.FiredBullets.Add(bul);
        }
        public override void MoveHorizontal(double dist)
        {
            double newPos = this.PositionX + dist;
            if ((this.StartPosition.X + newPos) > 0 && (this.StartPosition.X + newPos) < (SpaceParams.SpaceWidth -this.BlockSize.Width))
            {
                this.PositionX = newPos;
            }
            else
            {

            }
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            ExFireIniter.DeleteItself();
            LeftExhaust.DeleteItself();
            RightExhaust.DeleteItself();
        }

        public override void DetachEvents()
        {
            base.DetachEvents();
            this.ExFireIniter.OnImageSourcesInitialized -= ExFireIniter_OnImageSourcesInitialized;
        }
    }

    public delegate void LeftExhaustCreated(ExhaustFireM f);
    public delegate void RightExhaustCreated(ExhaustFireM f);

    public delegate void SpeedXChanged(double speedX);
}



