using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public class SpaceControllerBase : Unit
    {
        private int LaserShotCount = 0;

        public event ShipExploded OnShipExploded;
        public event IdleProgazovkaBegin OnIdleProgazovkaBegin;
        public event IdleProgazovkaEnd OnIdleProgazovkaEnd;
        public event MaxSpeedLimitChanged OnMaxSpeedLimitChanged;

        #region properties

        private readonly Space _outerSpace;
        public Space OuterSpace
        {
            get
            {
                return _outerSpace;
            }
        }

        private SpaceMap _spaceMapM;
        public SpaceMap SpaceMapM
        {
            get
            {
                if (_spaceMapM == null)
                {
                    _spaceMapM = new SpaceMap();
                }
                return _spaceMapM;
            }
        }

        private double _maxSpeed;
        public double MaxSpeed
        {
            get
            {
                return _maxSpeed;
            }
            set
            {
                _maxSpeed = value;
                if (this.OnMaxSpeedLimitChanged != null)
                {
                    this.OnMaxSpeedLimitChanged(_maxSpeed);
                }
            }
        }

        private double _speedChangeStep;
        protected double SpeedChangeStep
        {
            get
            {
                return _speedChangeStep * SpaceParams.SpaceHeightRatioTo900;
            }
        }

        private double _speedSlowDownStep;
        protected double SpeedSlowDownStep
        {
            get
            {
                return _speedSlowDownStep * SpaceParams.SpaceHeightRatioTo900;
            }
        }

        protected InfoBoard _board;
        public InfoBoard Board
        {
            get
            {
                return _board;
            }
        }

        private Timer _slowDownCountDownTimer;
        private Timer SlowDownCountDownTimer
        {
            get
            {
                return _slowDownCountDownTimer;
            }
        }

        private Timer _slowDownUndergoTimer;
        private Timer SlowDownUndergoTimer
        {
            get
            {
                if (_slowDownUndergoTimer == null)
                {
                    _slowDownUndergoTimer = new Timer(1000);
                }
                return _slowDownUndergoTimer;
            }
        }

        private bool IsFirstWordForSpeakPrepared { get; set; }
        internal bool IsProducingWordToSpeakEnabled { get; set; }
        internal bool IsWordPreparationSucceded { get; set; }
        internal bool IsProducingNewRowsEnabled { get; set; }
        internal int HitSuccessionCount { get; set; }
        internal int MissSuccessionCount { get; set; }
        #endregion

        public SpaceControllerBase()
        {
            this._outerSpace = new Space();
            this._board = new InfoBoardIntro(this.OuterSpace);
            this.IsProducingWordToSpeakEnabled = true;
            this.IsProducingNewRowsEnabled = true;
            this.IsWordPreparationSucceded = true;
            _speedChangeStep = 0.004;
            _speedSlowDownStep = 0.0005;

            this.OuterSpace.OnPictureBoxHitted += space_OnPictureBoxHitted;
            this.OuterSpace.OnPictureBoxHitted += this.Board.Space_OnPictureBoxHitted;
            this.OuterSpace.OnBulletRunOutOfSpace += space_OnBulletRunOutOfSpace;
            this.OuterSpace.OnTheWordPreparedForSpeak += OuterSpace_OnTheWordPreparedForSpeak;
            this.OuterSpace.StarShipControls.OnLaserCollidedWithShip += StarShipControls_OnLaserCollidedWithShip;
            this.OuterSpace.StarShipControls.OnAsteroidCollidedWithShip += StarShipControls_OnAsteroidCollidedWithShip;
            this.OuterSpace.OnShipHitted += OuterSpace_OnShipHitted;

            this.SlowDownUndergoTimer.OnTicked += SlowDownUndergoTimer_OnTicked;

        }

        private void OuterSpace_OnShipHitted()
        {
            if (SpaceParams.BlockSpeed > SpaceParams.MinBlockSpeed)
            {
                SpaceParams.ChangeBlockSpeed(-this.SpeedChangeStep, false);
            }
        }

        internal void BeginIdleProgazovka()
        {
            if (this.OnIdleProgazovkaBegin != null)
            {
                this.OnIdleProgazovkaBegin();
            }
        }
        public void ShootRoundAtShip()
        {
            this.LaserShotCount = 0;
            this.OuterSpace.LasersGrid.ShootRoundAt(this.OuterSpace.StarShipControls.Ship, 15, 800);
        }

        private void StarShipControls_OnLaserCollidedWithShip()
        {
            this.LaserShotCount++;
            if (this.LaserShotCount < 5)
            {
                this.OuterSpace.StarShipControls.JitterShip();
            }
            else
            {
                this.OuterSpace.LasersGrid.StopRound();
                this.OuterSpace.ExplosionsGrid.CreateExplosion(this.OuterSpace.StarShipControls.Ship);
                if (this.OnShipExploded != null)
                {
                    this.OnShipExploded();
                }
            }
        }

        private void StarShipControls_OnAsteroidCollidedWithShip()
        {
            this.OuterSpace.StarShipControls.JitterShip();
        }

        async public virtual Task Initialize()
        {
            this.OuterSpace.Initialize();
            await this.Board.Initialize();
            this.SpaceMapM.Initialize();
            this.InitializeLevelSpeeds();
        }


        public void TimeElapses(int roundedTime, double deltaTime)
        {
            this.OuterSpace.TimeElapses(roundedTime, deltaTime);
            this.SpaceMapM.TimeElapses(roundedTime, deltaTime);
            if (this.IsProducingNewRowsEnabled)
            {
                this.OuterSpace.TryCreateNewRow(deltaTime);
            }
            if (!this.IsFirstWordForSpeakPrepared)
            {
                if (this.OuterSpace.PicBoxGrid.Assets.Count > 2)
                {
                    this.OuterSpace.PrepareWordToSpeak();
                    this.IsFirstWordForSpeakPrepared = true;
                    return;
                }
            }
            this.OuterSpace.DetectBulletCollisionWithAmmo();
            this.OuterSpace.DetectStarShipCollisionWithAmmo();
            this.OuterSpace.DetectBulletCollisionWithAsteroid();
            this.OuterSpace.DetectStarShipCollisionAsteroid();
            this.OuterSpace.DetectStarShipCollisionLaser();
            bool isPicCollided = this.OuterSpace.DetectBulletCollisionWithPictureBox();
            PictureBoxM Pic = this.OuterSpace.RemovePictureBoxesFromInvisibleSpace();
            this.OuterSpace.RemoveAmmosFromInvisibleSpace();
            if (this.IsProducingWordToSpeakEnabled && (isPicCollided || (Pic != null && Pic.IsThisPictureSpoken) || !this.IsWordPreparationSucceded))
            {
                PictureBoxM pic = this.OuterSpace.PrepareWordToSpeak();
                this.IsWordPreparationSucceded = (pic != null);
            }
            else
            {
            }
        }

        internal void StopSpace()
        {
            this.IsProducingNewRowsEnabled = false;
            this.IsProducingWordToSpeakEnabled = false;
            this.OuterSpace.PicBoxGrid.DeleteAllAssets();
            this.OuterSpace.AmmosGrid.DeleteAllAssets();
            this.OuterSpace.AsteroidsGrid.DeleteAllAssets();
         
            this.SpaceMapM.StopMap();
            this.SlowDownCountDownTimer.Stop();
            this.SlowDownUndergoTimer.Stop();
            this.Board.StopIndicators();
        }

        internal void DisableShip()
        {
            this.OuterSpace.StarShipControls.DisableFire();
        }

        private void space_OnPictureBoxHitted(PictureBoxM pic)
        {
            this.SlowDownCountDownTimer.Stop();
            this.SlowDownUndergoTimer.Stop();
            this.HitSuccessionCount++;
            this.MissSuccessionCount = 0;
            if (SpaceParams.BlockSpeed < this.MaxSpeed && this.HitSuccessionCount > 1)
            {
                this.HitSuccessionCount = 0;
                SpaceParams.ChangeBlockSpeed((SpaceParams.BlockSpeed + this.SpeedChangeStep) <= this.MaxSpeed ? this.SpeedChangeStep : (this.MaxSpeed - SpaceParams.BlockSpeed), false);
            }
        }

        private void space_OnBulletRunOutOfSpace()
        {
            this.SlowDownCountDownTimer.Stop();
            this.SlowDownUndergoTimer.Stop();
            this.MissSuccessionCount++;
            this.HitSuccessionCount = 0;
            if (SpaceParams.BlockSpeed > SpaceParams.MinBlockSpeed && this.MissSuccessionCount > 1)
            {
                this.MissSuccessionCount = 0;
                SpaceParams.ChangeBlockSpeed(-this.SpeedChangeStep, false);
            }
        }

        private void OuterSpace_OnTheWordPreparedForSpeak(PictureBoxM pic)
        {
            this.SlowDownCountDownTimer.Stop();
            this.SlowDownUndergoTimer.Stop();
            this.SlowDownCountDownTimer.Start(); // in 4000 ms event
        }

        private void SlowDownCountDownTimer_OnTicked()
        {
            this.SlowDownCountDownTimer.Stop();
            this.SlowDownUndergoTimer.Start();// in 3000 ms event
            if (SpaceParams.BlockSpeed > SpaceParams.MinBlockSpeed)
            {
                SpaceParams.ChangeBlockSpeed(-this.SpeedSlowDownStep, true);
            }
        }

        private void SlowDownUndergoTimer_OnTicked()
        {
            if (SpaceParams.BlockSpeed > SpaceParams.MinBlockSpeed)
            {
                SpaceParams.ChangeBlockSpeed(-this.SpeedSlowDownStep, true);
            }
        }

        private void InitializeLevelSpeeds()
        {
            double bspeed = SpaceParams.MinBlockSpeed + (Game.GameTopic.HierarchyLevel.LevelNo - 1) * SpaceParams.BlockSpeedStepPerLevel;
            SpaceParams.ChangeBlockSpeed(bspeed, false);
            this.MaxSpeed = SpaceParams.MinBlockSpeed + Game.GameTopic.HierarchyLevel.LevelNo * SpaceParams.BlockSpeedStepPerLevel;

            double t1 = SpaceParams.PictureBlockHeight * 4.5 / bspeed;
            Game.GameTopic.HierarchyLevel.TimeThreshold = ((SpaceParams.BigWayDistance - SpaceParams.PictureBlockHeight * 4.5) / ((this.MaxSpeed + bspeed) / 2)) + t1;
            SpaceParams.ShipSpeed = 0.5 + 0.7 / 11d * Game.GameTopic.HierarchyLevel.LevelNo;

            double intervalForSlowDown = SpaceParams.PictureBlockHeight * 1.5 / SpaceParams.BlockSpeed;
            intervalForSlowDown = intervalForSlowDown * (1.0 + Game.GameTopic.HierarchyLevel.LevelNo * 0.4 / 12d);
            // _slowDownCountDownTimer = new Timer(intervalForSlowDown);
            _slowDownCountDownTimer = new Timer(1500);
            this.SlowDownCountDownTimer.OnTicked += SlowDownCountDownTimer_OnTicked;

        }

        public void IdleProgazovkaCompleted()
        {
            if (this.OnIdleProgazovkaEnd != null)
            {
                this.OnIdleProgazovkaEnd();
            }
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            this.SpaceMapM.DeleteItself();
            this.SlowDownCountDownTimer.Stop();
            this.SlowDownUndergoTimer.Stop();
            this.OuterSpace.DeleteItself();
        }
        public override void DetachEvents()
        {
            base.DetachEvents();
            this.OuterSpace.OnPictureBoxHitted -= this.Board.Space_OnPictureBoxHitted;
            this.OuterSpace.OnPictureBoxHitted -= space_OnPictureBoxHitted;
            this.OuterSpace.OnBulletRunOutOfSpace -= space_OnBulletRunOutOfSpace;
            this.OuterSpace.OnTheWordPreparedForSpeak -= OuterSpace_OnTheWordPreparedForSpeak;
            this.OuterSpace.StarShipControls.OnLaserCollidedWithShip -= StarShipControls_OnLaserCollidedWithShip;
            this.OuterSpace.StarShipControls.OnAsteroidCollidedWithShip -= StarShipControls_OnAsteroidCollidedWithShip;
            this.OuterSpace.OnShipHitted += OuterSpace_OnShipHitted;
            this.SlowDownCountDownTimer.OnTicked -= SlowDownCountDownTimer_OnTicked;
            this.SlowDownUndergoTimer.OnTicked -= SlowDownUndergoTimer_OnTicked;
        }
    }

    public delegate void ShipExploded();
    public delegate void IdleProgazovkaBegin();
    public delegate void IdleProgazovkaEnd();
    public delegate void MaxSpeedLimitChanged(double speed);
}
