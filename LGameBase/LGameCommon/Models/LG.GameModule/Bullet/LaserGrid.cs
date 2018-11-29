using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public class LaserGrid : AssetGrid
    {
        private AssetInitializer _laserInitializer;
        private AssetInitializer LaserInitializer
        {
            get
            {
                if (_laserInitializer == null)
                {
                    _laserInitializer = new AssetInitializer();
                }
                return _laserInitializer;
            }
        }

        private BaseBlock TargetBlock { get; set; }
        private int RoundShootNumber { get; set; }
        private int ShootedNumber { get; set; }
        private Timer RoundTimer { get; set; }

        public void Initialize()
        {
            this.LaserInitializer.InitializeBitmapSource("ms-appx:///Images/Laser.png");
        }

        public override void TimeElapses(double roundedDeltaTime)
        {
            base.TimeElapses(roundedDeltaTime);
            this.RemoveAssetsOutOfScope();
        }

        public void ShootAt(BaseBlock target)
        {
            LaserM laser = this.CreateAssetAndSetInitialPos(target);
            laser.Controls.IsMovingVertical = true;
            laser.Controls.IsMovingHorizontal = true;
            this.Assets.Add(laser);
        }
        public void ShootRoundAt(BaseBlock target, int shootNumber, int breaksMs)
        {
            this.ShootedNumber = 0;
            this.RoundShootNumber = shootNumber;
            this.TargetBlock = target;
            this.ShootAt(TargetBlock);
            this.ShootedNumber++;
            this.RoundTimer = new Timer(breaksMs);
            this.RoundTimer.OnTicked += RoundTimer_OnTicked;
            this.RoundTimer.Start();
        }
        public void StopRound()
        {
             this.RoundTimer.Stop();
        }
        private void RoundTimer_OnTicked()
        {
            if (this.RoundShootNumber > this.ShootedNumber)
            {
                this.ShootAt(TargetBlock);
            }
            else
            {
                this.RoundTimer.Stop();
                this.RoundTimer.OnTicked -= RoundTimer_OnTicked;
            }
            this.ShootedNumber++;
        }

        private LaserM CreateAssetAndSetInitialPos(BaseBlock target)
        {
            LaserM laser = new LaserM(target);
            // laser.BlockSize = new Size() { Width = 15, Height = 137.5 };
            laser.StartPosition = new Point() { X = this.Rand.Next(50, SpaceParams.SpaceWidth - 50), Y = 0 };
            LaserInitializer.InitializeAsset(laser, 0.4);
            return laser;
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            this.LaserInitializer.DeleteItself();
        }
    }
}
