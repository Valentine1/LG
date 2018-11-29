using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public class PictureBoxGrid : AssetColumnsGrid
    {
        private PictureBoxInitializer _picBoxInitializer;
        private PictureBoxInitializer PicBoxInitializer
        {
            get
            {

                return _picBoxInitializer;
            }
        }
        public static int LastPictureBoxColumnNumber { get; set; }

        public PictureBoxGrid(): base()
        {
            _picBoxInitializer = new PictureBoxInitializer();
        }

        public PictureBoxM LastCreatedPictureBox { get; set; }
        public async Task Initialize()
        {
            this.StartPosition = new Point() { X = 0, Y = 0 };
            await PicBoxInitializer.LoadWords();
            PicBoxInitializer.InitializeImagesSources();
            await PicBoxInitializer.InitializeAudioSources();
        }
        public override void TimeElapses(double roundedDeltaTime)
        {
            base.TimeElapses(roundedDeltaTime);
            this.PositionY = this.PositionY + roundedDeltaTime * SpaceParams.BlockSpeed;
            //this.Block.MoveVertical(this.DeltaTimeFromPreviousFrame * this.SpeedY);
        }

        protected override AssetM CreateAssetAndSetInitialPos()
        {
            PictureBoxM pb = new PictureBoxM();
            int i = Rand.Next(0, SpaceParams.Columns);
            pb.ColumnPositionNumber = i;
            PictureBoxGrid.LastPictureBoxColumnNumber = i;
            pb.StartPosition = new Point() { X = i * SpaceParams.PictureBlockWidth, Y = -SpaceParams.PictureBlockHeight };
            pb.StartPositionOnMoveArea = new Point() { X = i * SpaceParams.PictureBlockWidth, Y = -SpaceParams.PictureBlockHeight - this.PositionY };
            // pb.StartPosition = new Point() { X = i * SpaceParams.PictureBlockWidth, Y = -Rand.Next(0, 1080) };
            pb.BlockSize = new Size() { Width = SpaceParams.PictureBlockWidth, Height = SpaceParams.PictureBlockHeight };
            pb.Controls.IsMovingVertical = true;
            PicBoxInitializer.InitWithValues(pb);
            AssetColumns[i].Insert(0, pb);
            this.LastCreatedPictureBox = pb;
            this.AllCreatedLinesCount++;
            return pb;
        }

        public PictureBoxM GetRandomLastWord()
        {
            Dictionary<int, int> mapToNotEmptyColumns = new Dictionary<int, int>();
            int j = 0;
            for (int i = 0; i < this.AssetColumns.Count; i++)
            {
                if (this.AssetColumns[i].Count > 0 && this.AssetColumns[i][this.AssetColumns[i].Count - 1].PositionY >= SpaceParams.PictureBlockHeight - SpaceParams.PictureBlockHeight / 2
                                                  && this.AssetColumns[i][this.AssetColumns[i].Count - 1].PositionY <= (5 * SpaceParams.PictureBlockHeight - SpaceParams.PictureBlockHeight / 3) )
                {
                    mapToNotEmptyColumns.Add(j, i);
                    j++;
                }
            }
            if (mapToNotEmptyColumns.Count > 0)
            {
                int index = Rand.Next(0, mapToNotEmptyColumns.Count);
                int colNumber = mapToNotEmptyColumns[index];
                int lastAsset = this.AssetColumns[colNumber].Count - 1;
                ((PictureBoxM)this.AssetColumns[colNumber][lastAsset]).IsThisPictureSpoken = true;
                return (PictureBoxM)this.AssetColumns[colNumber][lastAsset];
            }
            return null;
        }
        public override void DeleteItself()
        {
            base.DeleteItself();
            this.PicBoxInitializer.DeleteItself();
        }
    }
}
