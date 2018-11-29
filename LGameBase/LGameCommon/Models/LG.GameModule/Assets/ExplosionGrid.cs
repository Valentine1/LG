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
    public class ExplosionGrid : AssetColumnsGrid
    {
        private AnimatedAssetInitializer _explosionIniter;
        private AnimatedAssetInitializer ExplosionIniter
        {
            get
            {
                if (_explosionIniter == null)
                {
                    _explosionIniter = new AnimatedAssetInitializer();
                }
                return _explosionIniter;
            }
        }

        private BaseBlock AssetBox { get; set; }

        public void Initialize()
        {
            this.ExplosionIniter.InitializeBitmapSources(new AnimatedAssetSource() { PicNameStubWithoutExt = "ms-appx:///Images/Explosion/Explosion", PictureNumber = 12, FrameChangeTime = 40 });
        }

        public void CreateExplosion(BaseBlock ass)
        {
            this.AssetBox = ass;
            this.CreateAssetLine();
        }
        protected override AssetM CreateAssetAndSetInitialPos()
        {
            ExplosionM expl = new ExplosionM(AnimationBehavior.OneTime);
            expl.OnAnimationEnded += expl_OnAnimationEnded;
            expl.StartPosition = new Point()
            {
                X = AssetBox.StartPosition.X + AssetBox.PositionX + (this.AssetBox.BlockSize.Width - 62) / 2,
                Y = AssetBox.StartPosition.Y + AssetBox.PositionY + (this.AssetBox.BlockSize.Height - 54) / 2
            };

            expl.BlockSize = new Size() { Width = 63, Height = 54 };
            expl.Controls.IsMovingVertical = true;
            this.ExplosionIniter.InitializeAnimatedAsset(expl, false, 0.38);

            this.AllCreatedLinesCount++;
            return expl;
        }

        private void expl_OnAnimationEnded(AnimatedAssetM asset)
        {
            asset.OnAnimationEnded -= expl_OnAnimationEnded;
            this.DeleteAsset(asset);
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            this.ExplosionIniter.DeleteItself();
        }
    }
}
