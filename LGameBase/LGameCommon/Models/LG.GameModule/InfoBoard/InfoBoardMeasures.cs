using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class InfoBoardMeasures
    {
        private const int AbsHeightDifference = 312;  //1080-768
        private const int SpeedometerTopMarginMin = 4;
        private const int SpeedometerTopMarginMax = 25;
        private const int SpeedometerHeightMin = 150;
        private const int SpeedometerHeightMax = 200;
        private const int SpeedometerBottomMarginMin = 6;
        private const int SpeedometerBottomMarginMax = 20;
        private const int IndicatorsBottomMarginMin = 8;
        private const int IndicatorsBottomMarginMax = 40;

        public int SpeedometerTopMargin { get; set; }
        public int SpeedometerHeight { get; set; }
        public int SpeedometerBottomMargin { get; set; }
        public int IndicatorsWidth { get; set; }
        public int IndicatorsHeight { get; set; }
        public int IndicatorsBottomMargin { get; set; }

        public void Initialize()
        {
            int heightDifference = GlobalGameParams.WindowHeight - GlobalGameParams.WindowHeightMin;
            this.SpeedometerTopMargin = SpeedometerTopMarginMin + heightDifference * (SpeedometerTopMarginMax - SpeedometerTopMarginMin) / AbsHeightDifference;
            this.SpeedometerHeight = SpeedometerHeightMin + heightDifference * (SpeedometerHeightMax - SpeedometerHeightMin) / AbsHeightDifference;
            this.SpeedometerBottomMargin = SpeedometerBottomMarginMin + heightDifference * (SpeedometerBottomMarginMax - SpeedometerBottomMarginMin) / AbsHeightDifference;
            this.IndicatorsWidth = SpaceParams.InfoBoardWidth - 10;
            this.IndicatorsHeight = (int)(this.IndicatorsWidth / 2.94);
            this.IndicatorsBottomMargin = IndicatorsBottomMarginMin + heightDifference * (IndicatorsBottomMarginMax - IndicatorsBottomMarginMin) / AbsHeightDifference;

            SpaceParams.InfoBoardHeight = SpaceParams.PictureBlockHeight + SpeedometerTopMargin + SpeedometerHeight + SpeedometerBottomMargin + IndicatorsHeight + IndicatorsBottomMargin;
        }

    }
}
