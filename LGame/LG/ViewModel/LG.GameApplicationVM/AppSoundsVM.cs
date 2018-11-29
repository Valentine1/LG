using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using LG.Models;

namespace LG.ViewModels
{
    public class AppSoundsVM : BaseNotify
    {
        private AppSounds Sounds { get; set; }

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
                this.NotifyPropertyChanged("BackMusicAudio");
            }
        }

        public AppSoundsVM(AppSounds sounds)
        {
            Sounds = sounds;
            sounds.BackMusicAudioInitialized += sounds_BackMusicAudioInitialized;
            sounds.OnBackMusicStarted += sounds_OnBackMusicStarted;
            sounds.OnBackMusicStopped += sounds_OnBackMusicStopped;
        }

        private void sounds_BackMusicAudioInitialized()
        {
            this.BackMusicAudio = Sounds.BackMusicAudio;
        }

        private void sounds_OnBackMusicStarted()
        {
            if (this.OnBackMusicStarted != null)
            {
                this.OnBackMusicStarted();
            }
        }

        private void sounds_OnBackMusicStopped()
        {
            if (this.OnBackMusicStopped != null)
            {
                this.OnBackMusicStopped();
            }
        }

    }

    public delegate void BackMusicStarted();
    public delegate void BackMusicStopped();
}
