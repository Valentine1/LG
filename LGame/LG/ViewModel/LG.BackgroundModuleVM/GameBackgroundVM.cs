using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using LG.Models;

namespace LG.ViewModels
{
    public class GameBackgroundVM : ModuleVM
    {
        public event AvatarStartShowBeginingVM OnAvatarStartShowBeginingVM;
        public event AvatarFinishShowBeginingVM OnAvatarFinishShowBeginingVM;
        public event UpperStarCellsMarginChanged OnUpperStarCellsMarginChanged;

        #region model
        private GameBackground GameBackgroundM
        {
            get
            {
                return (GameBackground)this.ModelM;
            }
        }
        #endregion

        private SpiralVM _bigSpiralVModel;
        public SpiralVM BigSpiralVModel
        {
            get
            {
                return _bigSpiralVModel;
            }
        }


        private BackSpaceGridVM _backSpaceGridVModel;
        public BackSpaceGridVM BackSpaceGridVModel
        {
            get
            {
                return _backSpaceGridVModel;
            }
        }

        private double _upperStarCellsMargin;
        public double UpperStarCellsMargin
        {
            get
            {
                return _upperStarCellsMargin;
            }
            set
            {
                _upperStarCellsMargin = value;
                if (this.OnUpperStarCellsMarginChanged != null)
                {
                    this.OnUpperStarCellsMarginChanged(_upperStarCellsMargin);
                }
            }
        }

        public GameBackgroundVM(Module mod) : base(mod)
        {
            _bigSpiralVModel = new SpiralVM(this.GameBackgroundM.BigSpiral);
            _backSpaceGridVModel = new BackSpaceGridVM((mod as GameBackground).BackSpace); 
            this.GameBackgroundM.AvatarBack.OnAvatarStartShowBegining += AvatarBack_OnAvatarStartShowBegining;
            this.GameBackgroundM.AvatarBack.OnAvatarFinishShowBegining += AvatarBack_OnAvatarFinishShowBegining;
            this.UpperStarCellsMargin = BackgroundParams.CellHeight;
            BackgroundParams.OnCellHeightChanged += BackgroundParams_OnCellHeightChanged;
        }

        public void BackgroundParams_OnCellHeightChanged(double ch)
        {
            this.UpperStarCellsMargin = ch;
        }

        private void AvatarBack_OnAvatarStartShowBegining(AssetM avaImage, AssetM avaName)
        {
            if (this.OnAvatarStartShowBeginingVM != null)
            {
                this.OnAvatarStartShowBeginingVM(new AssetVM(avaImage), new AssetVM(avaName), new Size((double)SpaceParams.SpaceWidth, (double)SpaceParams.SpaceHeight));
            }
        }

        private void AvatarBack_OnAvatarFinishShowBegining(AssetM avaImage, AssetM avaName)
        {
            if (this.OnAvatarFinishShowBeginingVM != null)
            {
                this.OnAvatarFinishShowBeginingVM(new AssetVM(avaImage), new AssetVM(avaName), new Size((double)SpaceParams.SpaceWidth, (double)SpaceParams.SpaceHeight));
            }
        }

        public void AvatarShowEnded()
        {
            GameBackgroundM.AvatarShowEnded();
        }
    }

    public delegate void AvatarStartShowBeginingVM(AssetVM avImage, AssetVM avaName, Size spaceSize);
    public delegate void AvatarFinishShowBeginingVM(AssetVM avImage, AssetVM avaName, Size spaceSize);
    public delegate void UpperStarCellsMarginChanged(double m);
}
