using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public partial class Pyramid
    {
        private async void InitLevels(List<Level> levels)
        {
            foreach (Level l in levels)
            {
                PyramidStep step = new PyramidStep();
                step.AvatarM = new Avatar(l);
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Images/hierarchy/" + l.PictureUrl));
                step.AvatarM.PictureSource = new BitmapImage();
                IRandomAccessStream ras = await file.OpenReadAsync();
                step.AvatarM.PictureSource.SetSource(ras);
                this.Steps.Add(step);
            }
        }

    }
}
