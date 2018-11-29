using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class StarShipVM : BaseBlockVM
    {
        public event LeftExhaustFireVMCreated OnLeftExhaustFireVMCreated;
        public event RightExhaustFireVMCreated OnRightExhaustFireVMCreated;
        public event SpeedXChangedVM OnSpeedXChangedVM;

        #region model
        private StarShipM StarShip
        {
            get
            {
                return (StarShipM)this.BaseBlockM;
            }
            set
            {
                this.BaseBlockM = value;
            }
        }
        #endregion

        #region view-model

        private ObservableCollection<BulletVM> firedBulletVMs;
        public ObservableCollection<BulletVM> FiredBulletVMs
        {
            get
            {
                if (firedBulletVMs == null)
                {
                    firedBulletVMs = new ObservableCollection<BulletVM>();
                }
                return firedBulletVMs;
            }
        }

        private ExhaustFireVM _leftExhaustFireVM;
        public ExhaustFireVM LeftExhaustFireVM
        {
            get
            {
                return _leftExhaustFireVM;
            }
            set
            {
                _leftExhaustFireVM = value;
                if (this.OnLeftExhaustFireVMCreated != null)
                {
                    this.OnLeftExhaustFireVMCreated(_leftExhaustFireVM);
                }
            }
        }

        private ExhaustFireVM _rightExhaustFireVM;
        public ExhaustFireVM RightExhaustFireVM
        {
            get
            {
                return _rightExhaustFireVM;
            }
            set
            {
                _rightExhaustFireVM = value;
                if (this.OnRightExhaustFireVMCreated != null)
                {
                    this.OnRightExhaustFireVMCreated(_rightExhaustFireVM);
                }
            }
        }

        private double _speedXVM;
        public double SpeedXVM
        {
            get
            {
                return _speedXVM;
            }
            set
            {
                _speedXVM = value;
                if (OnSpeedXChangedVM != null)
                {
                    this.OnSpeedXChangedVM(_speedXVM);
                }
            }
        }
        private double _xmove;
        public  double Xmove
        {
            get
            {
                return _xmove;
            }
            set
            {
                _xmove = value;
                this.StarShip.PositionX = _xmove;
            }
        }
        #endregion

        public StarShipVM(StarShipM ship) : base(ship)
        {
            this.StarShip.OnMovedX -= this.BaseBlockM_OnMovedX;
            this.StarShip.FiredBullets.CollectionChanged += FiredBullets_CollectionChanged;
            this.StarShip.OnLeftExhaustCreated += StarShip_OnLeftExhaustCreated;
            this.StarShip.OnRightExhaustCreated += StarShip_OnRightExhaustCreated;
            this.StarShip.OnSpeedXChanged += StarShip_OnSpeedXChanged;

            this.SpeedXVM = ship.SpeedX;
        }

        void StarShip_OnSpeedXChanged(double speedX)
        {
            this.SpeedXVM = speedX;
        }

        private void StarShip_OnLeftExhaustCreated(ExhaustFireM f)
        {
            this.LeftExhaustFireVM = new ExhaustFireVM(f);
        }

        private void StarShip_OnRightExhaustCreated(ExhaustFireM f)
        {
            this.RightExhaustFireVM = new ExhaustFireVM(f);
        }
        private void FiredBullets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                BulletM bul = (BulletM)e.NewItems[0];
                BulletVM bulvm = new BulletVM(bul);
                bulvm.OnItselfDeleted += bulvm_OnItselfDeleted;
                this.FiredBulletVMs.Add(bulvm);
            }
        }

        private void bulvm_OnItselfDeleted(UnitVM bbvm)
        {
            (bbvm as BulletVM).OnItselfDeleted -= bulvm_OnItselfDeleted;
            this.FiredBulletVMs.Remove((BulletVM)bbvm);
        }

        #region utilities

        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);
            if (FiredBulletVMs != null)
            {
                FiredBulletVMs.Clear();
                firedBulletVMs = null;
            }
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            (m as StarShipM).FiredBullets.CollectionChanged -= FiredBullets_CollectionChanged;
            (m as StarShipM).OnLeftExhaustCreated -= StarShip_OnLeftExhaustCreated;
            (m as StarShipM).OnRightExhaustCreated -= StarShip_OnRightExhaustCreated;
            (m as StarShipM).OnSpeedXChanged -= StarShip_OnSpeedXChanged;
        }
        #endregion

    }

    public delegate void LeftExhaustFireVMCreated(ExhaustFireVM fvm);
    public delegate void RightExhaustFireVMCreated(ExhaustFireVM fvm);
    public delegate void SpeedXChangedVM(double speedX);
}
