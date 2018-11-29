using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using LG.Common;

namespace LG.Models
{
    public class CrossHair : AnimatedAssetM
    {
        private List<AssetColumnsGrid> OperatioanlAreas { get; set; }
        private AnimatedAssetInitializer _picturesInitializer;
        private AnimatedAssetInitializer PicturesInitializer
        {
            get
            {
                if (_picturesInitializer == null)
                {
                    _picturesInitializer = new AnimatedAssetInitializer();
                }
                return _picturesInitializer;
            }
        }

        public AssetM GrippedAsset { get; set; }

        private AssetM _laserBeam;
        public AssetM LaserBeam
        {
            get
            {
                if(_laserBeam== null)
                {
                    _laserBeam = new AssetM();
                }
                return _laserBeam;
            }
        }

        private double InitialHeight { get; set; }
        private double InitialScaleCenterY { get; set; }
        bool IsHooked { get; set; }

        public void Initialize(List<AssetColumnsGrid> operatioanalAreas)
        {
            this.OperatioanlAreas = operatioanalAreas;
            this.PicturesInitializer.OnImageSourcesInitialized += PicturesInitializer_OnImageSourcesInitialized;
            this.PicturesInitializer.InitializeBitmapSources(new AnimatedAssetSource() { PicNameStubWithoutExt = "ms-appx:///Images/crosshair", PictureNumber = 2, FrameChangeTime = 300 });
            this.OnMovedY += CrossHair_OnMovedY;
            InitialHeight = this.LaserBeam.BlockSize.Height;
            this.LaserBeam.ScaleCenterY =this.LaserBeam.BlockSize.Height;
        }

        private void CrossHair_OnMovedY(double distance)
        {
            this.LaserBeam.ScaleY = (InitialHeight - distance) / InitialHeight;
        }
        internal void TimeElapses()
        {
            if (OperatioanlAreas != null)
            {
                TryAimCrossHairs();
            }
        }

        internal void Ship_OnMovedX(double distance)
        {
            this.PositionX = distance;
            this.LaserBeam.PositionX = distance;
        }
        internal void TryAimCrossHairs()
        {
            this.CalculateColumn();
            var assets = this.GetLastAssetsFromAreas();
            foreach (AssetM ass in assets)
            {
                if (this.IsCrossHairsWithinAsset(ass))
                {
                    this.GripOnAsset(ass);
                    break;
                }
            }
        }

        internal void GripOnAsset(AssetM asset)
        {
            if (IsHooked && this.GrippedAsset == asset)
            {
                return;
            }
        
            if (this.GrippedAsset != null)
            {
                this.GrippedAsset.OnItselfDeleted -= asset_OnItselfDeleted;
                this.UnhookFromAssetMovment();
            }
            this.GrippedAsset = asset;
            if (this.GrippedAsset != null)
            {
                this.GrippedAsset.OnItselfDeleted += asset_OnItselfDeleted;
                this.PlaceHairsWithinAsset();
                this.HookToAssetMovement();
            }
        }

        private void HookToAssetMovement()
        {
            this.GrippedAsset.OnMovedY += GrippedAsset_OnMovedY;
            IsHooked = true;
        }
        private void GrippedAsset_OnMovedY(double distance)
        {
            if (this.IsCrossHairsWithinAsset())
            {
                this.PositionY = distance - (this.StartPosition.Y - this.GrippedAsset.StartPosition.Y) + this.GrippedAsset.BlockSize.Height / 2;
            }
            else
            {
                UnhookFromAssetMovment();
            }
        }
        private void PlaceHairsWithinAsset()
        {
            this.PositionY = -(this.StartPosition.Y - this.GrippedAsset.StartPosition.Y) + this.GrippedAsset.PositionY;
        }
        private void UnhookFromAssetMovment()
        {
            this.GrippedAsset.OnMovedY -= GrippedAsset_OnMovedY;
           // this.GrippedAsset = null;
            IsHooked = false;
        }

        private void asset_OnItselfDeleted(Unit m)
        {
            m.OnItselfDeleted -= asset_OnItselfDeleted;
            this.GrippedAsset = null;
        }
      
        private void PicturesInitializer_OnImageSourcesInitialized(AnimatedAssetInitializer initer)
        {
            this.PicturesInitializer.InitializeAnimatedAsset(this);
        }
        private void CalculateColumn()
        {
            double colposition = this.Center.X / SpaceParams.PictureBlockWidth;
            if (colposition > 8)
            {
                return;
            }
            this.ColumnPositionNumber = (int)Math.Truncate(colposition);
        }
        private IEnumerable<AssetM> GetLastAssetsFromAreas()
        {
            List<AssetM> assets = new List<AssetM>();
            foreach (AssetColumnsGrid area in this.OperatioanlAreas)
            {
                AssetM ass = area.GetLastPicture(this.ColumnPositionNumber);
                if (ass != null)
                {
                    assets.Add(ass);
                }
            }
            return assets.OrderByDescending(a => a.PositionY);
        }
        private bool IsCrossHairsWithinAsset()
        {
            return IsCrossHairsWithinAsset(this.GrippedAsset);
        }
        private bool IsCrossHairsWithinAsset(AssetM ass)
        {
            if (ass == null)
            {
                return false;
            }
            double lim1 = ass.StartPosition.X + ass.PositionX;
            double lim2 = lim1 + ass.BlockSize.Width;
            if (this.Center.X < lim2 && this.Center.X > lim1)
            {
                return true;
            }
            return false;
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            this.PicturesInitializer.DeleteItself();
            this.OperatioanlAreas = null;
        }
        public override void DetachEvents()
        {
            base.DetachEvents();
            this.PicturesInitializer.OnImageSourcesInitialized -= PicturesInitializer_OnImageSourcesInitialized;
        }
    }

}
