using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;


namespace LG.Models
{
    public  partial class AssetM
    {
        public event PictureSourceChanged OnPictureSourceChanged;
        public event AudioStreamChanged OnAudioStreamChanged;

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
                if (OnPictureSourceChanged != null)
                {
                    OnPictureSourceChanged(_pictureSource);
                }
            }
        }

        public int RealPictureWidth
        {
            get
            {
                return PictureSource != null ? PictureSource.PixelWidth : 0;
            }
        }
        public int RealPictureHeight
        {
            get
            {
                return PictureSource != null ? PictureSource.PixelHeight : 0;
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
                if (this.OnAudioStreamChanged != null)
                {
                    this.OnAudioStreamChanged(_audioStream);
                }
            }
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            this.Controls.DeleteItself();
            this.PictureSource = null;
            this.AudioStream = null;

        }
    }

    public delegate void PictureSourceChanged(BitmapImage pic);
    public delegate void AudioStreamChanged(IRandomAccessStream audio);
}
