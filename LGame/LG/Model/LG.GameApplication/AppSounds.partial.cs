using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LG.Common;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace LG.Models
{
    public partial class AppSounds
    {
        public event BackMusicAudioInitialized BackMusicAudioInitialized;
        public event BackMusicStarted OnBackMusicStarted;
        public event BackMusicStopped OnBackMusicStopped;

        private IRandomAccessStream _backMusicAudio;
        public IRandomAccessStream BackMusicAudio
        {
            get
            {
                return _backMusicAudio;
            }
            set
            {
                _backMusicAudio = value;
                if (this.BackMusicAudioInitialized != null)
                {
                    this.BackMusicAudioInitialized();
                }
            }
        }

        private List<IRandomAccessStream> _backMusicAudios;
        public List<IRandomAccessStream> BackMusicAudios
        {
            get
            {
                if (_backMusicAudios == null)
                {
                    _backMusicAudios = new List<IRandomAccessStream>();
                }
                return _backMusicAudios;
            }
        }

        private bool ShouldBePlayed;
        private object Sync = new object();

        public async void Initialize()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Sounds/back2.mp3"));
            IRandomAccessStream bm = await file.OpenReadAsync();
            this.BackMusicAudios.Add(bm);
            Monitor.Enter(this.Sync);
            this.BackMusicAudio = this.BackMusicAudios[0];
            if (this.ShouldBePlayed)
            {
                this.StartBackMusic();
            }
            Monitor.Exit(this.Sync);
        }

        public void PlayBackMusic()
        {
            Monitor.Enter(this.Sync);
            if (BackMusicAudio != null)
            {
                this.StartBackMusic();
            }
            else
            {
                this.ShouldBePlayed = true;
            }
            Monitor.Exit(this.Sync);
      
        }

        public void StopBackMusic()
        {
            if (this.OnBackMusicStopped != null)
            {
                this.OnBackMusicStopped();
            }
        }

        private void StartBackMusic()
        {
            if(this.OnBackMusicStarted!= null)
            {
                this.OnBackMusicStarted();
            }
        }
    }


    public delegate void BackMusicAudioInitialized();
    public delegate void BackMusicStarted();
    public delegate void BackMusicStopped();
}
