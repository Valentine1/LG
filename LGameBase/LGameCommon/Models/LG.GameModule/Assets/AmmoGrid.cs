using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public class AmmoGrid : AssetColumnsGrid
    {
        private AnimatedAssetInitializer _ammoBoxInitializer;
        private AnimatedAssetInitializer AmmosInitializer
        {
            get
            {
                if (_ammoBoxInitializer == null)
                {
                    _ammoBoxInitializer = new AnimatedAssetInitializer();
                }
                return _ammoBoxInitializer;
            }
        }

        public static int LastAmmoColumnNumber { get; set; }

        public int LastAmmoRowNumber { get; set; }

        public void Initialize()
        {
            AmmosInitializer.InitializeBitmapSources(new AnimatedAssetSource() { PicNameStubWithoutExt = "ms-appx:///Images/ammoBlock", PictureNumber = 2, FrameChangeTime = 700 });
        }
        public override void TimeElapses(double roundedDeltaTime)
        {
            base.TimeElapses(roundedDeltaTime);
            this.PositionY = this.PositionY+ roundedDeltaTime * SpaceParams.BlockSpeed;
        }
        protected override AssetM CreateAssetAndSetInitialPos()
        {
            AmmoM amm = new AmmoM();
            amm.ID = AssetIdIncrementer++;
            int pos = this.GetRandomColumnNumberForAmmo();
            amm.ColumnPositionNumber = pos;
            AmmoGrid.LastAmmoColumnNumber = pos;
            amm.StartPosition = new Point() { X = pos * SpaceParams.PictureBlockWidth, Y = -SpaceParams.PictureBlockHeight };
            amm.StartPosition = new Point()
            {
                X = amm.StartPosition.X + (SpaceParams.PictureBlockWidth - SpaceParams.AssetBlockWidth) / 2,
                Y = amm.StartPosition.Y + (SpaceParams.PictureBlockHeight - SpaceParams.AssetBlockHeight) / 2
            };
            amm.StartPositionOnMoveArea = new Point() { X =pos * SpaceParams.PictureBlockWidth, Y = -SpaceParams.PictureBlockHeight - this.PositionY };
            amm.StartPositionOnMoveArea = new Point()
            {
                X = amm.StartPositionOnMoveArea.X + (SpaceParams.PictureBlockWidth - SpaceParams.AssetBlockWidth) / 2,
                Y = amm.StartPositionOnMoveArea.Y + (SpaceParams.PictureBlockHeight - SpaceParams.AssetBlockHeight) / 2
            };
         
            amm.Controls.IsMovingVertical = true;
            this.AmmosInitializer.InitializeAnimatedAsset(amm, false, 0.6);
            amm.BlockSize = new Size() { Width = SpaceParams.AssetBlockWidth, Height = SpaceParams.AssetBlockHeight };
            this.AssetColumns[pos].Insert(0, amm);
            return amm;
        }
        protected override bool CheckIfAssetIsOut(AssetM ass)
        {
            if ((ass.StartPosition.Y + ass.PositionY) > (SpaceParams.PictureBlockHeight * 7))
            {
                return true;
            }
            return false;
        }

        private int GetRandomColumnNumberForAmmo()
        {
            if (PictureBoxGrid.LastPictureBoxColumnNumber == 0)
            {
                return Rand.Next(PictureBoxGrid.LastPictureBoxColumnNumber + 1, SpaceParams.Columns);
            }
            int i1 = Rand.Next(0,  PictureBoxGrid.LastPictureBoxColumnNumber);
            if ( PictureBoxGrid.LastPictureBoxColumnNumber < SpaceParams.Columns - 1)
            {
                int i2 = Rand.Next( PictureBoxGrid.LastPictureBoxColumnNumber + 1, SpaceParams.Columns);
                int i3 = Rand.Next(0, 2);
                return i3 == 0 ? i1 : i2;
            }
            return i1;
        }
    }
}
