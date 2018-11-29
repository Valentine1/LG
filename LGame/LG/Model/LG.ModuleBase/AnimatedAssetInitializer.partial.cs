using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LG.Common;
using LG.Data;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace LG.Models
{
    public partial class AnimatedAssetInitializer : Unit
    {
        public event ImageSourcesInitialized OnImageSourcesInitialized;

        private ObservableCollection<BitmapImage> _animatedAssetImageSources;
        private ObservableCollection<BitmapImage> AnimatedAssetImageSources
        {
            get
            {
                if (_animatedAssetImageSources == null)
                {
                    _animatedAssetImageSources = new ObservableCollection<BitmapImage>();
                }
                return _animatedAssetImageSources;
            }
        }
        private AnimatedAssetSource AssetSource { get; set; }

        async public void InitializeBitmapSources(AnimatedAssetSource aaSource)
        {
            this.AssetSource = aaSource;
            this.AnimatedAssetImageSources.CollectionChanged += AnimatedAssetImageSources_CollectionChanged;
            for (int i = 1; i <= this.AssetSource.PictureNumber; i++)
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(this.AssetSource.PicNameStubWithoutExt + i.ToString() + ".png"));
                BitmapImage im = new BitmapImage();
                im.SetSource(await file.OpenAsync(FileAccessMode.Read));
                this.AnimatedAssetImageSources.Add(im);
            }
        }
        public void InitializeAnimatedAsset(AnimatedAssetM aa)
        {
            aa.FrameChangeTime = this.AssetSource.FrameChangeTime;
            this.AssignPictureBitmaps(aa, false);
        }
        public void InitializeAnimatedAsset(AnimatedAssetM aa, bool random, bool enlarge)
        {
            aa.FrameChangeTime = this.AssetSource.FrameChangeTime;
            if (this.AnimatedAssetImageSources[0].PixelWidth <= 128 && enlarge)
            {
                aa.BlockSize = new Size() { Width = this.AnimatedAssetImageSources[0].PixelWidth * 2, Height = this.AnimatedAssetImageSources[0].PixelHeight * 2 };
            }
            else
            {
                aa.BlockSize = new Size() { Width = this.AnimatedAssetImageSources[0].PixelWidth, Height = this.AnimatedAssetImageSources[0].PixelHeight };
            }
            this.AssignPictureBitmaps(aa, random);
        }
        public void InitializeAnimatedAsset(AnimatedAssetM aa, bool random, double sizeRatio)
        {
            aa.FrameChangeTime = this.AssetSource.FrameChangeTime;
            aa.BlockSize = new Size() { Width = SpaceParams.PictureBlockWidth * sizeRatio, Height = SpaceParams.PictureBlockHeight * sizeRatio };
        
            this.AssignPictureBitmaps(aa, random);
        }
        private void AnimatedAssetImageSources_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.AnimatedAssetImageSources.Count == this.AssetSource.PictureNumber)
            {
                if (this.OnImageSourcesInitialized != null)
                {
                    this.OnImageSourcesInitialized(this);
                }
            }
        }

        private void AssignPictureBitmaps(AnimatedAssetM aa, bool random)
        {
            aa.SetPictureSources(this.AnimatedAssetImageSources.ToList(), random);
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            AnimatedAssetImageSources.Clear();

        }
        public override void DetachEvents()
        {
            base.DetachEvents();
            this.AnimatedAssetImageSources.CollectionChanged -= AnimatedAssetImageSources_CollectionChanged;
        }
    }

    public delegate void ImageSourcesInitialized(AnimatedAssetInitializer initer);
}
