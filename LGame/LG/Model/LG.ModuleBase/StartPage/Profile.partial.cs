using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using LG.Data;
using Windows.UI.Popups;

namespace LG.Models
{
    public partial class Profile
    {
        public event PictureSourceChanged OnPictureSourceChanged;

        public IRandomAccessStream PictureStream { get; set; }

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

        protected async void InitPicture(ProfileDAL p)
        {
            this.PictureSource = new BitmapImage();
            StorageFile file = null;

            if (p.Type == 1 && this.TakePhotoFromServer && p.HasPhoto)
            {
                try
                {
                    string fsu = await GlobalGameParams.FileStorageUrl();
                    this.PictureSource.UriSource = new Uri(fsu + "/profilepictures/" + p.PictureUrl, UriKind.Absolute);
                }
                catch (Exception ex)
                {
                    Logging.Logger.ShowMessageAndLog(ex);
                }
            }
            else
            {
                try
                {
                    file = await ApplicationData.Current.LocalFolder.GetFileAsync("profilepictures\\" + p.PictureUrl);
                    this.PictureSource.SetSource(await file.OpenReadAsync());
                }
                catch (Exception ex)
                {
                    Logging.Logger.ShowMessageAndLog(ex);
                }
            }
        }

        public Profile DeepClone()
        {
            Profile p = new Profile();
            p.ID = this.ID;
            p.Name = this.Name;
            p.Password = this.Password;
            p.PictureSource = this.PictureSource;
            p.Type = this.Type;
            p.SelectedThemeID = this.SelectedThemeID;
            p.HoursPlayed = this.HoursPlayed;
            p.OnBestTimeChanged = this.OnBestTimeChanged;
            //  p.OnBlockSizeChanged = this.OnBlockSizeChanged;

            return p;
        }
    }
}
