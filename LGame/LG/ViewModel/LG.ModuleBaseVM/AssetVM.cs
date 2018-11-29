using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using Windows.UI;
using LG.Models;

namespace LG.ViewModels
{
    public class AssetVM : BaseBlockVM
    {
        public event PictureSourceChanged OnPictureSourceChanged;

        public int ID { get; set; }
        internal AssetM AssM
        {
            get
            {
                return (AssetM)this.BaseBlockM;
            }
        }

        #region view properties
        private BitmapImage _pictureSource;
        public BitmapImage PictureSource
        {
            get
            {
                return _pictureSource;
            }
            set
            {
                _pictureSource = value;
                if (this.OnPictureSourceChanged != null)
                {
                    this.OnPictureSourceChanged(_pictureSource);
                }
                this.NotifyPropertyChanged("PictureSource");
            }
        }

        private IRandomAccessStream _audioStream;
        public IRandomAccessStream AudioStream
        {
            get
            {
                return _audioStream;
            }
            set
            {
                _audioStream = value;
                this.NotifyPropertyChanged("AudioStream");
            }
        }

        private Point _startPositionOnMoveArea;
        public Point StartPositionOnMoveArea
        {
            get
            {
                return _startPositionOnMoveArea;
            }
            set
            {
                _startPositionOnMoveArea = value;
                this.NotifyPropertyChanged("StartPositionOnMoveArea");
            }
        }

        private string _textValue;
        public string TextValue
        {
            get
            {
                return _textValue;
            }
            set
            {
                _textValue = value;
                this.NotifyPropertyChanged("TextValue");
            }
        }

        #endregion

        public AssetVM(AssetM ass) : base(ass)
        {
            this.ID = ass.ID;
            this.TextValue = ass.TextValue;
            //if (this.AssM.BlockSize != null)
            //{
            this.BlockSize = new Size(this.AssM.BlockSize.Width, this.AssM.BlockSize.Height);
            //}
            this.StartPositionOnMoveArea = new Point(this.AssM.StartPositionOnMoveArea.X, this.AssM.StartPositionOnMoveArea.Y);
            this.PictureSource = this.AssM.PictureSource;
            this.AudioStream = this.AssM.AudioStream;

            this.AssM.OnPictureSourceChanged += AssM_OnPictureSourceChanged;
            this.AssM.OnStartPositionOnMoveAreaChanged += AssM_OnStartPositionOnMoveAreaChanged;
            this.AssM.OnDisplayStartPositionChanged += AssM_OnDisplayStartPositionChanged;

        }

        private void AssM_OnStartPositionOnMoveAreaChanged(Common.Point sp)
        {
            this.StartPositionOnMoveArea = new Point(sp.X, sp.Y);
        }

        private void AssM_OnPictureSourceChanged(BitmapImage pic)
        {
            this.PictureSource = pic;

        }

        private void AssM_OnDisplayStartPositionChanged(Common.Point pos)
        {
            this.StartPosition = new Point(this.BaseBlockM.DisplayStartPosition.X, this.BaseBlockM.DisplayStartPosition.Y);
        }
        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);
            PictureSource = null;
            AudioStream = null;
            this.TextValue = string.Empty;
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            this.AssM.OnPictureSourceChanged -= AssM_OnPictureSourceChanged;
            this.AssM.OnStartPositionOnMoveAreaChanged -= AssM_OnStartPositionOnMoveAreaChanged;
            this.AssM.OnDisplayStartPositionChanged -= AssM_OnDisplayStartPositionChanged;

        }
    }

    public delegate void PictureSourceChanged(BitmapImage im);
}
