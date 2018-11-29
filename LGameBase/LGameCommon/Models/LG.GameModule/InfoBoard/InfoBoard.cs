using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{

    public abstract partial class InfoBoard : Unit
    {
        public event TheWordToSpeakChanged OnTheWordToSpeakChanged;
        public event ShipSpeedChanged OnShipSpeedChanged;
        public event DelayEndedForPictureToSpeakShow OnDelayEndedForPictureToSpeakShow;
        public event ClearShowWord OnClearShowWord;

        private Space BigSpace { get; set; }
        private MarkedSpeed _shipSpeed;
        public MarkedSpeed ShipSpeed
        {
            get
            {
                return _shipSpeed;
            }

            set
            {
                _shipSpeed = value;
                if (this.OnShipSpeedChanged != null)
                {
                    this.OnShipSpeedChanged(_shipSpeed);
                }
            }
        }

        protected PictureBoxM _theWordToSpeak;
        public PictureBoxM TheWordToSpeak
        {
            get
            {
                return _theWordToSpeak;
            }
            set
            {
                _theWordToSpeak = value;
                if (this.OnTheWordToSpeakChanged != null)
                {
                    this.OnTheWordToSpeakChanged(_theWordToSpeak);
                }
            }
        }

        public InfoBoardMeasures Measures { get; set; }
        public PictureBoxShowMode PicShowMode { get; set; }
        public bool IsPictureInitiallyVisible { get; set; }

        public InfoBoard(Space space)
        {
            BigSpace = space;
            SpaceParams.OnBlockSpeedChanged += SpaceParams_OnBlockSpeedChanged;
            this.BigSpace.OnTheWordPreparedForSpeak += space_OnTheWordPreparedForSpeak;
            this.BigSpace.OnDelayEndedForPictureToSpeakShow += BigSpace_OnDelayEndedForPictureToSpeakShow;
            this.Measures = new InfoBoardMeasures();
        }

        private void InitializePicShowMode()
        {
            this.IsPictureInitiallyVisible = false;
            if (Game.GameTopic.HierarchyLevel.LevelNo <= 7)
            {
                this.PicShowMode = PictureBoxShowMode.PictureAndWord;
            }
            else if (Game.GameTopic.HierarchyLevel.LevelNo <= 8)
            {
                this.PicShowMode = PictureBoxShowMode.WordOnly;
            }
            else
            {
                this.PicShowMode = PictureBoxShowMode.NoPictureNoWord;
            }
        }
        internal void Space_OnPictureBoxHitted(PictureBoxM pic)
        {
            if (this.TheWordToSpeak.ID != pic.ID && this.TheWordToSpeak.TextValue == pic.TextValue)
            {
                if (this.OnClearShowWord != null)
                {
                    this.OnClearShowWord();
                }
            }
        }
        private void SpaceParams_OnBlockSpeedChanged(double ajustedSpeed, double absoluteSpeed, bool isSmallChange)
        {
            this.ShipSpeed = new MarkedSpeed() { Speed = absoluteSpeed, IsSmallChange = isSmallChange };
        }

        protected virtual void space_OnTheWordPreparedForSpeak(PictureBoxM pic)
        {
        }
        protected void BigSpace_OnDelayEndedForPictureToSpeakShow()
        {
            if (this.OnDelayEndedForPictureToSpeakShow!= null)
            {
                this.OnDelayEndedForPictureToSpeakShow();
            }
        }

        public void StopIndicators()
        {
            this.ShipSpeed = new MarkedSpeed() { Speed = 0 };
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            BigSpace = null;
        }

        public override void DetachEvents()
        {
            base.DetachEvents();
            SpaceParams.OnBlockSpeedChanged -= SpaceParams_OnBlockSpeedChanged;
            this.BigSpace.OnTheWordPreparedForSpeak -= space_OnTheWordPreparedForSpeak;
            this.BigSpace.OnDelayEndedForPictureToSpeakShow -= BigSpace_OnDelayEndedForPictureToSpeakShow;
        }
    }

    public delegate void TheWordToSpeakChanged(PictureBoxM pb);
    public delegate void ShipSpeedChanged(MarkedSpeed speed);
    public delegate void ClearShowWord();

    public enum PictureBoxShowMode { PictureAndWord=0, WordOnly, NoPictureNoWord }

    public struct MarkedSpeed
    {
        public double Speed { get; set; }
        public bool IsSmallChange { get; set; }
    }

    //        1   0.032
    //2   0.044p
    //3   0.056
    //4   0.068
    //5   0.080
    //6   0.092
    //7   0.104
    //8   0.116
    //9   0.128
    //10  0.140
    //11  0.152
    //12  unlimited
}
