using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public class SpaceGridCell : AssetM
    {
        static public SpaceGridCell CreateCell()
        {
            SpaceGridCell cell = new SpaceGridCell();
            cell.ID = AssetInitializer.Incrementer++;
            BackSpaceGrid.CellsStoreIndexes.Add(cell.ID);
            for (int i = 0; i < 12; i++)
            {
                AssetM star = new AssetM();
                int index = BackSpaceGrid.Rand.Next(BackSpaceGrid.SmallStarsInitializers.Count);
                BackSpaceGrid.SmallStarsInitializers[index].InitializeAsset(star, 1);

                star.StartPosition = new Point()
                {
                    X = BackSpaceGrid.Rand.Next((int)(BackgroundParams.CellWidth - 10)),
                    Y = BackSpaceGrid.Rand.Next((int)(BackgroundParams.CellHeight - 10))
                };
                cell.Stars.Add(star);
            }
            for (int i = 0; i < 4; i++)
            {
                AssetM star = new AssetM();
                int index = BackSpaceGrid.Rand.Next(BackSpaceGrid.BigStarsInitializers.Count);
                int m = BackSpaceGrid.Rand.Next(4, 8);
                BackSpaceGrid.BigStarsInitializers[index].InitializeAsset(star, m * 0.1);

                star.StartPosition = new Point()
                {
                    X = BackSpaceGrid.Rand.Next((int)(BackgroundParams.CellWidth - 10)),
                    Y = BackSpaceGrid.Rand.Next((int)(BackgroundParams.CellHeight - 10))
                };
                cell.Stars.Add(star);
            }
            for (int i = 0; i <3; i++)
            {
                BlinkStar star = new BlinkStar();
                int index = BackSpaceGrid.Rand.Next(BackSpaceGrid.BlinkingStarsInitializers.Count);
                BackSpaceGrid.BlinkingStarsInitializers[index].InitializeAsset(star, 1);
                int m = BackSpaceGrid.Rand.Next(4, 6);
                star.Scale(m * 0.1, m * 0.1);
                star.ScaleCenterX = star.RealPictureWidth / 2d;
                star.ScaleCenterY = star.RealPictureHeight / 2d;

                star.StartPosition = new Point()
                {
                    X = BackSpaceGrid.Rand.Next((int)(BackgroundParams.CellWidth - 10)),
                    Y = BackSpaceGrid.Rand.Next((int)(BackgroundParams.CellHeight - 10))
                };
                cell.BlinkingStars.Add(star);
            }
            return cell;
        }
        static public SpaceGridCellNebula CreateCell(AssetInitializer ai)
        {
            SpaceGridCellNebula cell = new SpaceGridCellNebula();
            cell.ID = AssetInitializer.Incrementer++;
            BackSpaceGrid.CellsNebulaStoreIndexes.Add(cell.ID);
            AssetM nebula = new AssetM();
            ai.InitializeAsset(nebula, 1);
            nebula.BlockSize = new Size() { Width = BackgroundParams.CellWidth, Height = BackgroundParams.CellHeight };
            cell.Stars.Add(nebula);
            return cell;
        }

        private ObservableCollection<AssetM> _stars;
        public ObservableCollection<AssetM> Stars
        {
            get
            {
                if (_stars == null)
                {
                    _stars = new ObservableCollection<AssetM>();
                }
                return _stars;
            }
        }

        private ObservableCollection<BlinkStar> _blinkingStars;
        public ObservableCollection<BlinkStar> BlinkingStars
        {
            get
            {
                if (_blinkingStars == null)
                {
                    _blinkingStars = new ObservableCollection<BlinkStar>();
                }
                return _blinkingStars;
            }
        }

        private Timer _blinkStarTimer;
        protected Timer BlinkStarTimer
        {
            get
            {
                if (_blinkStarTimer == null)
                {
                    _blinkStarTimer = new Timer();
                    _blinkStarTimer.OnTicked += BlinkStarTimer_OnTicked;
                }
                return _blinkStarTimer;
            }
        }

        internal void Activate()
        {
            this.SetBlinkStarTimer();
        }

        internal virtual void DeActivate()
        {
            this.BlinkStarTimer.Stop();
            BackSpaceGrid.CellsStoreIndexes.Add(this.ID);
        }

        private void BlinkStarTimer_OnTicked()
        {
            this.BlinkStarTimer.Stop();
            this.Blink();
            this.SetBlinkStarTimer();
        }
        private void SetBlinkStarTimer()
        {
            this.BlinkStarTimer.SetInterval(BackSpaceGrid.Rand.Next(1300, 2400));
            this.BlinkStarTimer.Start();
        }

        private void Blink()
        {
            if (this.BlinkingStars.Count > 0)
            {
                int e = BackSpaceGrid.Rand.Next(8, 10);
                int i = BackSpaceGrid.Rand.Next(this.BlinkingStars.Count);
                this.BlinkingStars[i].Blink(e * 0.1);
            }
        }
    }
}
