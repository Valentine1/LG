using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using LG.Models;

namespace LG.ViewModels
{

    public class MarkedSpeedVM:BaseNotify
    {
        private double _speed;
        public double Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                NotifyPropertyChanged("Speed");
            }
        }
        private bool _isSmallChange;
        public bool IsSmallChange
        {
            get
            {
                return _isSmallChange;
            }
            set
            {
                _isSmallChange = value;
                NotifyPropertyChanged("IsSmallChange");
            }
        }
    }
    public class InfoBoardVM : UnitVM
    {
        SpaceControllerBase SpaceController { get; set; }

        public event SelectedWordChanged OnSelectedWordChanged;
        public event IdleProgazovkaBegin OnIdleProgazovkaBegin;
        public event DelayEndedForPictureToSpeakShowVM OnDelayEndedForPictureToSpeakShowVM;
        public event ClearShowWordVM OnClearShowWordVM;

        #region View -properties

        private MarkedSpeedVM _shipSpeed;
        public MarkedSpeedVM ShipSpeed
        {
            get
            {
                if (_shipSpeed == null)
                {
                    _shipSpeed = new MarkedSpeedVM();
                }
                return _shipSpeed;
            }
            set
            {
                _shipSpeed = value;
                this.NotifyPropertyChanged("ShipSpeed");
            }
        }

        private double _pictureHeight;
        public double PictureHeight
        {
            get
            {
                return _pictureHeight;
            }
            set
            {
                _pictureHeight = value;
                this.NotifyPropertyChanged("PictureHeight");
            }
        }

        private double _speedometerTopMargin;
        public double SpeedometerTopMargin
        {
            get
            {
                return _speedometerTopMargin;
            }
            set
            {
                _speedometerTopMargin = value;
                this.NotifyPropertyChanged("SpeedometerTopMargin");
            }
        }

        private int _speedometerHeight;
        public int SpeedometerHeight
        {
            get
            {
                return _speedometerHeight;
            }
            set
            {
                _speedometerHeight = value;
                this.NotifyPropertyChanged("SpeedometerHeight");
            }
        }

        private int _speedometerBottomMargin;
        public int SpeedometerBottomMargin
        {
            get
            {
                return _speedometerBottomMargin;
            }
            set
            {
                _speedometerBottomMargin = value;
                this.NotifyPropertyChanged("SpeedometerBottomMargin");
            }
        }

        private int _indicatorsWidth;
        public int IndicatorsWidth
        {
            get
            {
                return _indicatorsWidth;
            }
            set
            {
                _indicatorsWidth = value;
                this.NotifyPropertyChanged("IndicatorsWidth");
            }
        }

        private int _indicatorsHeight;
        public int IndicatorsHeight
        {
            get
            {
                return _indicatorsHeight;
            }
            set
            {
                _indicatorsHeight = value;
                this.NotifyPropertyChanged("IndicatorsHeight");
            }
        }

        private int _indicatorsBottomMargin;
        public int IndicatorsBottomMargin
        {
            get
            {
                return _indicatorsBottomMargin;
            }
            set
            {
                _indicatorsBottomMargin = value;
                this.NotifyPropertyChanged("IndicatorsBottomMargin");
            }
        }

        private PictureBoxVM _selectedWord;
        public PictureBoxVM SelectedWord
        {
            get
            {
                return _selectedWord;
            }
            set
            {
                _selectedWord = value;
                if (this.OnSelectedWordChanged != null)
                {
                    this.OnSelectedWordChanged(_selectedWord);
                }
            }
        }

        private IRandomAccessStream _acceleratingLongAudio;
        public IRandomAccessStream AcceleratingLongAudio
        {
            get
            {
                return _acceleratingLongAudio;
            }
            set
            {
                _acceleratingLongAudio = value;
                NotifyPropertyChanged("AcceleratingLongAudio");
            }
        }
        private IRandomAccessStream _breakingLongAudio;
        public IRandomAccessStream BreakingLongAudio
        {
            get
            {
                return _breakingLongAudio;
            }
            set
            {
                _breakingLongAudio = value;
                NotifyPropertyChanged("BreakingLongAudio");
            }
        }

        #endregion

        public double MaxSpeed { get; set; }
        public Visibility StaticPictureVisibility { get; set; }
        public Visibility WordVisibility { get; set; }
        public bool IsPictureInitiallyVisible { get; set; }

        public InfoBoardVM(SpaceControllerBase controler) : base(controler)
        {
            this.PictureHeight = SpaceParams.PictureBlockHeight;
            this.SpaceController = controler;
            this.SpeedometerTopMargin = controler.Board.Measures.SpeedometerTopMargin;
            this.SpeedometerHeight = controler.Board.Measures.SpeedometerHeight;
            this.SpeedometerBottomMargin = controler.Board.Measures.SpeedometerBottomMargin;
            this.IndicatorsWidth = controler.Board.Measures.IndicatorsWidth;
            this.IndicatorsHeight = controler.Board.Measures.IndicatorsHeight;
            this.IndicatorsBottomMargin = controler.Board.Measures.IndicatorsBottomMargin;


            controler.Board.OnShipSpeedChanged += board_OnShipSpeedChanged;
            controler.Board.OnClearShowWord += Board_OnClearShowWord;
            controler.OnIdleProgazovkaBegin += controler_OnIdleProgazovkaBegin;
            controler.Board.OnDelayEndedForPictureToSpeakShow += Board_OnDelayEndedForPictureToSpeakShow;
            this.AcceleratingLongAudio = controler.Board.AcceleratingLongAudio;
            this.BreakingLongAudio = controler.Board.BreakingLongAudio;

            this.MaxSpeed = controler.MaxSpeed /SpaceParams.SpaceHeightRatioTo900;
            this.StaticPictureVisibility = controler.Board.PicShowMode == PictureBoxShowMode.PictureAndWord ? Visibility.Visible : Visibility.Collapsed;
            this.WordVisibility = (controler.Board.PicShowMode == PictureBoxShowMode.PictureAndWord || controler.Board.PicShowMode == PictureBoxShowMode.WordOnly)? Visibility.Visible:Visibility.Collapsed;
            IsPictureInitiallyVisible = controler.Board.IsPictureInitiallyVisible;
        }

     
        private void controler_OnIdleProgazovkaBegin()
        {
            if (this.OnIdleProgazovkaBegin != null)
            {
                this.OnIdleProgazovkaBegin();
            }
        }

        private void board_OnShipSpeedChanged(MarkedSpeed mspeed)
        {
            this.ShipSpeed.IsSmallChange = mspeed.IsSmallChange;
            this.ShipSpeed.Speed = mspeed.Speed;
        }
        private void Board_OnClearShowWord()
        {
            if (this.OnClearShowWordVM != null)
            {
                this.OnClearShowWordVM();
            }
        }

        public void IdleProgazovkaCompleted()
        {
            SpaceController.IdleProgazovkaCompleted();
            this.ShipSpeed.IsSmallChange = false;
            this.ShipSpeed.Speed = SpaceParams.BlockSpeed;
        }
        private void Board_OnDelayEndedForPictureToSpeakShow()
        {
            if (this.OnDelayEndedForPictureToSpeakShowVM != null)
            {
                this.OnDelayEndedForPictureToSpeakShowVM();
            }
        }
        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);
        }
        public override void DetachEvents(Unit c)
        {
            base.DetachEvents(c);
            (c as SpaceControllerBase).Board.OnShipSpeedChanged -= board_OnShipSpeedChanged;
            (c as SpaceControllerBase).OnIdleProgazovkaBegin -= controler_OnIdleProgazovkaBegin;
            (c as SpaceControllerBase).Board.OnDelayEndedForPictureToSpeakShow -= Board_OnDelayEndedForPictureToSpeakShow;
            (c as SpaceControllerBase).Board.OnClearShowWord -= Board_OnClearShowWord;
        }
    }

    public delegate void SelectedWordChanged(PictureBoxVM pbox);
    public delegate void IdleProgazovkaBegin();
    public delegate void DelayEndedForPictureToSpeakShowVM();
    public delegate void ClearShowWordVM();
}
