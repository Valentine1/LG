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
    public partial class AssetInitializer
    {
        private BitmapImage ImageSource;

        async public void InitializeBitmapSource(string picNamePath)
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(picNamePath));
            this.ImageSource = new BitmapImage();
            this.ImageSource.SetSource(await file.OpenAsync(FileAccessMode.Read));
        }
        async public Task InitializeBitmapSource(StorageFile sfile)
        {
            this.ImageSource = new BitmapImage();
            this.ImageSource.SetSource(await sfile.OpenAsync(FileAccessMode.Read));
        }

        public void InitializeAsset(AssetM aa, double magnifyRatio)
        {
            aa.BlockSize = new Size() { Width = ImageSource.PixelWidth * magnifyRatio, Height = ImageSource.PixelHeight * magnifyRatio };
            aa.PictureSource = ImageSource;
        }

        public virtual void DeleteItself()
        {
            ImageSource = null;
        }
    }
}
