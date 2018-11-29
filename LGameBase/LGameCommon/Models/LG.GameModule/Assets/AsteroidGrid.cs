using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public class AsteroidGrid : AssetColumnsGrid
    {
        private AnimatedAssetInitializer _asteroidSmallInitializer;
        private AnimatedAssetInitializer AsteroidSmallInitializer
        {
            get
            {
                if (_asteroidSmallInitializer == null)
                {
                    _asteroidSmallInitializer = new AnimatedAssetInitializer();
                }
                return _asteroidSmallInitializer;
            }
        }

        public int LastAsteroidRowNumber { get; set; }

        public void Initialize()
        {
            this.AsteroidSmallInitializer.InitializeBitmapSources(new AnimatedAssetSource() { PicNameStubWithoutExt = "ms-appx:///Images/Asteroids/rockm000", PictureNumber = 15, FrameChangeTime = 80 });
        }

        public override void TimeElapses(double roundedDeltaTime)
        {
            base.TimeElapses(roundedDeltaTime);
            this.PositionY = this.PositionY + roundedDeltaTime * SpaceParams.BlockSpeed;
        }
        protected override AssetM CreateAssetAndSetInitialPos()
        {
            AnimatedAssetM aster = new AnimatedAssetM();
            aster.ID = AssetColumnsGrid.AssetIdIncrementer++;
            int pos = this.GetRandomColumnNumberForAsteroid();
            aster.ColumnPositionNumber = pos;
            AmmoGrid.LastAmmoColumnNumber = pos;
            aster.StartPosition = new Point() { X = pos * SpaceParams.PictureBlockWidth, Y = -SpaceParams.PictureBlockHeight };
            this.AsteroidSmallInitializer.InitializeAnimatedAsset(aster, false, 0.38);
            double widthDiff = SpaceParams.PictureBlockWidth - aster.BlockSize.Width;

            aster.StartPosition = new Point()
            {
                X = aster.StartPosition.X + this.Rand.Next(0, (int)Math.Truncate(widthDiff)),
                Y = aster.StartPosition.Y + (SpaceParams.PictureBlockHeight - aster.BlockSize.Height) / 2
            };
            aster.StartPositionOnMoveArea = new Point() { X = aster.StartPosition.X, Y = aster.StartPosition.Y - this.PositionY };
            aster.Controls.IsMovingVertical = true;
            this.AssetColumns[pos].Insert(0, aster);
            this.LastAsteroidRowNumber = this.AllCreatedLinesCount;
            this.AllCreatedLinesCount++;
            return aster;
        }
        protected override bool CheckIfAssetIsOut(AssetM ass)
        {
            if ((ass.StartPosition.Y + ass.PositionY) > (SpaceParams.PictureBlockHeight * 7))
            {
                return true;
            }
            return false;
        }
        private int GetRandomColumnNumberForAsteroid()
        {
            List<int> colNumbers = new List<int>();
            for (int i = 0; i < SpaceParams.Columns; i++)
            {
                if (i != PictureBoxGrid.LastPictureBoxColumnNumber && i != AmmoGrid.LastAmmoColumnNumber)
                {
                    colNumbers.Add(i);
                }
            }
            int col =  this.Rand.Next(0, colNumbers.Count);
            return   colNumbers[col];
        }
    }
}
