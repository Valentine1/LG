using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Common;
using LG.Data;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace LG.Models
{
    public partial class PictureBoxInitializer
    {
        private Dictionary<int, BitmapImage> _imagesSources;
        private Dictionary<int, BitmapImage> ImagesSources
        {
            get
            {
                if (_imagesSources == null)
                {
                    _imagesSources = new Dictionary<int, BitmapImage>();
                }
                return _imagesSources;
            }
        }

        private Dictionary<int, IRandomAccessStream> _audioSources;
        private Dictionary<int, IRandomAccessStream> AudioSources
        {
            get
            {
                if (_audioSources == null)
                {
                    _audioSources = new Dictionary<int, IRandomAccessStream>();
                }
                return _audioSources;
            }
        }

        async public void InitializeImagesSources()
        {
            if (this.Words != null)
            {
                StorageFolder sf = ApplicationData.Current.LocalFolder;
                StorageFolder im = await sf.GetFolderAsync(@"images\" + Game.GameTopic.TextValue + "\\");
                foreach (Word w in this.Words)
                {
                    BitmapImage picSource = new BitmapImage();
                    StorageFile file = await im.GetFileAsync(w.PictureUrl);
                    picSource.SetSource(await EncryptionProvider.DecryptMedia(file));
                    this.ImagesSources.Add(w.ID, picSource);
                }
            }
        }

        async public Task InitializeAudioSources()
        {
            if (this.Words != null)
            {
                StorageFolder sf = ApplicationData.Current.LocalFolder;
                StorageFolder soundFolder = await sf.GetFolderAsync(@"sounds\" + GlobalGameParams.AppLang.Name + "\\" + Game.GameTopic.TextValue.ToLower());

                for (int i = 0; i < this.Words.Count; i++)
                {
                    StorageFile file = await soundFolder.GetFileAsync(this.Words[i].AudioUrl);
                    IRandomAccessStream ras = await EncryptionProvider.DecryptMedia(file);
                    this.AudioSources.Add(this.Words[i].ID, ras);
                }
            }

        }

        //creates platform specific Bitmap from url
        private void AssignPictureBitmap(PictureBoxM pb)
        {
            if (this.ImagesSources.Keys.Contains(pb.WordID))
            {
                pb.PictureSource = this.ImagesSources[pb.WordID];
            }
            else
            {

            }
        }

        //creates platform specific stream from url
        private void AssignAudioStream(PictureBoxM pb)
        {
            if (this.AudioSources.Keys.Contains(pb.WordID))
            {
                pb.AudioStream = this.AudioSources[pb.WordID];
            }
            else
            {

            }
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            ImagesSources.Clear();
            _imagesSources = null;
            AudioSources.Clear();
            _audioSources = null;
        }
    }
}
