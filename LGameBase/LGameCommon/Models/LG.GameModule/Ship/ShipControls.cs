using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public class ShipControls : BaseControls
    {
        public event TriedFireWhenChargerEmpty OnTriedFireWhenChargerEmpty;
        public event ShipJittered OnShipJittered;
        public event LaserCollidedWithShip OnLaserCollidedWithShip;
        public event AsteroidCollidedWithShip OnAsteroidCollidedWithShip;
        public event PanelHeightChanged OnPanelHeightChanged;

        public bool IsMovingLeft
        {
            set
            {
                this.IsMovingHorizontal = value;
                this.SpeedX = value ? -Math.Abs(this.SpeedX) : Math.Abs(this.SpeedX);
            }
        }
        public bool IsMovingRight
        {
            set
            {
                this.IsMovingHorizontal = value;
                this.SpeedX = value ? Math.Abs(this.SpeedX) : -Math.Abs(this.SpeedX);
            }
        }
        public StarShipM Ship
        {
            get
            {
                return (StarShipM)_block;
            }
        }

        public CrossHair TargetCrossHair { get; set; }

        private int _panelHeight;
        public int PanelHeight
        {
            get
            {
                return _panelHeight;
            }
            set
            {
                _panelHeight = value;
                if (this.OnPanelHeightChanged != null)
                {
                    this.OnPanelHeightChanged(_panelHeight);
                }
            }
        }
        private IndicatorMoving _ammoCountIndicator;
        public IndicatorMoving AmmoCountIndicator
        {
            get
            {
                return _ammoCountIndicator;
            }
        }

        public bool IsFireEnabled { get; set; }
        private bool IsCatchingAssetEnabled { get; set; }

        private AssetM CatchedAsset { get; set; }
        private double TimeLeftForAssetCapture { get; set; }

        private Timer _jitterTimer;
        private Timer JitterTimer
        {
            get
            {
                if (_jitterTimer == null)
                {
                    _jitterTimer = new Timer(100);
                    _jitterTimer.OnTicked += JitterTimer_OnTicked;
                }
                return _jitterTimer;
            }
        }

        private int JitterCount;
        private bool IsDisplaced;
        private void JitterTimer_OnTicked()
        {
            JitterCount++;
            if (JitterCount % 2 == 0)
            {
                IsDisplaced = false;
                this.Ship.PositionX = this.Ship.PositionX - 4;
                this.Ship.PositionY = this.Ship.PositionY - 4;
            }
            else
            {
                IsDisplaced = true;
                this.Ship.PositionX = this.Ship.PositionX + 4;
                this.Ship.PositionY = this.Ship.PositionY + 4;
            }
            if (JitterTimer.TotalTimeElapsed > 1000)
            {
                JitterTimer.Stop();
                if (IsDisplaced)
                {
                    this.Ship.PositionX = this.Ship.PositionX - 4;
                    this.Ship.PositionY = this.Ship.PositionY - 4;
                }
            }
        }

        public ShipControls() : base(SpaceParams.ShipSpeed, 0)
        {
            _block = new StarShipM();
            this.TargetCrossHair = new CrossHair();
            this._ammoCountIndicator = new IndicatorMoving(10, new Color() { A = 255, R = 144, G = 238, B = 144 }, IndicatorType.Thin);
            SpaceParams.OnShipSpeedChanged += SpaceParams_OnShipSpeedChanged;
            this.Ship.OnMovedX += TargetCrossHair.Ship_OnMovedX;
        }

        private void SpaceParams_OnShipSpeedChanged(double speed)
        {
            this.SpeedX = speed;
            this.Ship.SpeedX = speed;
        }

        public void Initialize()
        {
            this.Ship.BlockSize = new Size() { Width = SpaceParams.PictureBlockWidth / 2.5, Height = SpaceParams.PictureBlockHeight / 1.25 };
            this.Ship.StartPosition = new Point()
            {
                X = (GlobalGameParams.WindowWidth - SpaceParams.InfoBoardWidth) / 2 - this.Ship.BlockSize.Width / 2,
                Y = (GlobalGameParams.WindowHeight - SpaceParams.BottomSpace)
            };
            this.TargetCrossHair.BlockSize = new Size() { Width = this.Ship.BlockSize.Width / 2 + 1, Height = this.Ship.BlockSize.Width / 2 };
            this.TargetCrossHair.StartPosition = new Point()
            {
                X = this.Ship.StartPosition.X + (this.Ship.BlockSize.Width - this.TargetCrossHair.BlockSize.Width) * 0.5,
                Y = 200
            };
            this.TargetCrossHair.DisplayStartPosition = new Point()
            {
                X = (this.Ship.BlockSize.Width - this.TargetCrossHair.BlockSize.Width) * 0.5,
                Y = 200 - this.Ship.StartPosition.Y
            };
            this.TargetCrossHair.LaserBeam.StartPosition = new Point() { X = this.TargetCrossHair.DisplayCenter.X - 1.5, Y = this.TargetCrossHair.DisplayCenter.Y };
            this.TargetCrossHair.LaserBeam.BlockSize = new Size() { Height = this.Ship.StartPosition.Y - this.TargetCrossHair.Center.Y, Width = 3 };
            this.IsFireEnabled = true;
            this.Ship.Initialize();
        }

        public override void TimeElapses(double deltaTime)
        {
            base.TimeElapses(deltaTime);
            this.Ship.TimeElapses(deltaTime);
            this.TargetCrossHair.TimeElapses();
            if (this.IsCatchingAssetEnabled)
            {
                this.MoveAssetToTheShip(deltaTime);
            }
        }
        public void Fire()
        {
            if (this.Ship.FiredBullets.Count < 1 && this.IsFireEnabled)
            {
                if (this.AmmoCountIndicator.Ammount > 0)
                {
                    this.Ship.CreateBulletAndSetInitialPos();
                    this.AmmoCountIndicator.Decrease();
                }
                else
                {
                    this.AmmoCountIndicator.Flash();
                    if (this.OnTriedFireWhenChargerEmpty != null)
                    {
                        this.OnTriedFireWhenChargerEmpty();
                    }
                }
            }
        }
        public void JitterShip()
        {
            if (this.OnShipJittered != null)
            {
                this.OnShipJittered();
            }
            this.JitterTimer.Start();
        }

        internal void DisableFire()
        {
            this.IsFireEnabled = false;
        }
        internal void ShipCollidedWithAmmo(AmmoM ammo)
        {
            this.CatchedAsset = ammo;
            this.AmmoCountIndicator.Recharge();
            this.IsCatchingAssetEnabled = true;

            this.CatchedAsset.ScaleCenterX = this.Ship.Center.X - this.CatchedAsset.Center.X;
            this.CatchedAsset.ScaleCenterY = this.Ship.Center.Y - this.CatchedAsset.Center.Y;

            this.TimeLeftForAssetCapture = SpaceParams.TimeForAssetСapture;
        }
        internal void BulletCollidedWithPicBox(BulletM bul)
        {
            this.Ship.FiredBullets.Remove(bul);
            bul.DeleteItself();
        }
        internal void BulletCollidedWithAmmo(BulletM bul)
        {
            this.Ship.FiredBullets.Remove(bul);
            bul.DeleteItself();
        }
        internal bool DetectCollisionWithLaser(AssetM ass)
        {
            if (this.Ship.IntersectsNotRotatedWithRotated(ass))
            {
                if (this.OnLaserCollidedWithShip != null)
                {
                    this.OnLaserCollidedWithShip();
                }
                return true;
            }
            return false;
        }
        internal bool DetectCollisionWithAsteroid(AssetM ass)
        {
            if (this.Ship.IntersectsNotRotatedWithNotRotated(ass))
            {
                if (this.OnAsteroidCollidedWithShip != null)
                {
                    this.OnAsteroidCollidedWithShip();
                }
                return true;
            }
            return false;
        }
        protected override void Move()
        {
            base.Move();
            if (this.CatchedAsset != null)
            {
                this.CatchedAsset.ScaleCenterX = this.Ship.Center.X - this.CatchedAsset.StartPosition.X;
            }
        }

        private void MoveAssetToTheShip(double deltaTime)
        {
            if (!ShipCenterConcidesWithAsset() && this.TimeLeftForAssetCapture > 0)
            {
                this.ScaleDownAssetBeingCaptured(deltaTime);
                this.TimeLeftForAssetCapture -= deltaTime;
            }
            else
            {
                this.AssetHasGotCaptured();
            }
        }
        private bool ShipCenterConcidesWithAsset()
        {
            return Math.Abs(this.Ship.Center.X - this.CatchedAsset.Center.X) < 10 && Math.Abs(this.Ship.Center.Y - this.CatchedAsset.Center.Y) < 10;
        }
        private void AssetHasGotCaptured()
        {
            this.IsCatchingAssetEnabled = false;
            this.CatchedAsset.DeleteItself();
            this.CatchedAsset = null;
        }
        private void ScaleDownAssetBeingCaptured(double deltaTime)
        {
            double dscale = (1 - 0.1) / this.TimeLeftForAssetCapture * deltaTime;
            double scale = this.CatchedAsset.ScaleX - dscale;
            if (scale > 0)
            {
                this.CatchedAsset.Scale(scale, scale);
            }
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            this.Ship.DeleteItself();
            this.AmmoCountIndicator.DeleteItself();
            this.TargetCrossHair.DeleteItself();
        }
        public override void DetachEvents()
        {
            SpaceParams.OnShipSpeedChanged -= SpaceParams_OnShipSpeedChanged;
            this.JitterTimer.OnTicked -= JitterTimer_OnTicked;
        }
    }

    public delegate void TriedFireWhenChargerEmpty();
    public delegate void ShipJittered();
    public delegate void LaserCollidedWithShip();
    public delegate void AsteroidCollidedWithShip();
    public delegate void PanelHeightChanged(int panelHeight);
}


