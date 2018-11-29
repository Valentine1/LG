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
    public partial class AnimatedAssetM
    {
        private List<BitmapImage> PictureSources { get; set; }

        public void SetPictureSources(List<BitmapImage> pictureSources, bool random)
        {
            if (random)
            {
                this.PictureSources = new List<BitmapImage>();
                Random rand = new Random();
                int i = rand.Next(0, 4);
                for (int j = i; j < 4; j = (j < 3 ? ++j : j = 0))
                {
                    this.PictureSources.Add(pictureSources[j]);
                    if (this.PictureSources.Count == 4)
                    {
                        break;
                    }
                }
            }
            else
            {
                this.PictureSources = pictureSources;
            }
            if (this.PictureSources != null && this.PictureSources.Count > 0)
            {
                this.PictureSource = this.PictureSources[0];
            }
            if (this.Behavior == AnimationBehavior.Forever)
            {
                this.PictureTimer.OnTicked += PictureTimer_OnTicked;
            }
            else
            {
                this.PictureTimer.OnTicked += OneTimePictureTimer_OnTicked;
            }
            this.PictureTimer.Start();
        }
        private void PictureTimer_OnTicked()
        {
            if (this.PictureSources != null)
            {
                if (this.PicSourcesIterator < this.PictureSources.Count - 1)
                {
                    this.PicSourcesIterator++;
                }
                else
                {
                    this.PicSourcesIterator = 0;
                }
                this.PictureSource = this.PictureSources[this.PicSourcesIterator];
            }
        }

        private void OneTimePictureTimer_OnTicked()
        {
            if (this.PictureSources != null)
            {
                if (this.PicSourcesIterator < this.PictureSources.Count - 1)
                {
                    this.PicSourcesIterator++;
                    this.PictureSource = this.PictureSources[this.PicSourcesIterator];
                }
                else
                {
                    if (this.OnAnimationEnded != null)
                    {
                        this.OnAnimationEnded(this);
                    }
                }
            }
        }
    }
}
