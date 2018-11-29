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
    public partial class Avatar
    {
     
        async public Task InitPictureBitmap()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Images/hierarchy/" + this.PictureUrl));
            this.PictureSource = new BitmapImage();
            this.PictureSource.SetSource(await file.OpenReadAsync());
        }

        public Avatar DeepClone()
        {
            Avatar av = new Avatar() { TextValue = this.TextValue, LevelNo = this.LevelNo, PictureUrl = this.PictureUrl};
            return av;
        }
    }
}
