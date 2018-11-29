using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Common;
using LG.Data;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace LG.Models
{
    public partial class GameSounds:Unit
    {
        public event AllAudiosInitialized OnAllAudiosInitialized;
        public event SaluteAudioInitialized OnSaluteAudioInitialized;
        public event VibrationAudioInitialized OnVibrationAudioInitialized;
        public event SmehAudioInitialized OnSmehAudioInitialized;

        private IRandomAccessStream _explosionAudio;
        public IRandomAccessStream ExplosionAudio
        {
            get
            {
                return _explosionAudio;
            }
            set
            {
                _explosionAudio = value;
                this.TryOnAllAudiosInitialized();
            }
        }

        private IRandomAccessStream _shootAudio;
        public IRandomAccessStream ShootAudio
        {
            get
            {
                return _shootAudio;
            }
            set
            {
                _shootAudio = value;
                this.TryOnAllAudiosInitialized();
            }
        }

        private IRandomAccessStream _rechargeAudio;
        public IRandomAccessStream RechargeAudio
        {
            get
            {
                return _rechargeAudio;
            }
            set
            {
                _rechargeAudio = value;
                this.TryOnAllAudiosInitialized();
            }
        }

        private IRandomAccessStream _chargerEmpty;
        public IRandomAccessStream ChargerEmpty
        {
            get
            {
                return _chargerEmpty;
            }
            set
            {
                _chargerEmpty = value;
                this.TryOnAllAudiosInitialized();
            }
        }

        private IRandomAccessStream _engineAudio;
        public IRandomAccessStream EngineAudio
        {
            get
            {
                return _engineAudio;
            }
            set
            {
                _engineAudio = value;
                this.TryOnAllAudiosInitialized();
            }
        }

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
                this.TryOnAllAudiosInitialized();
            }
        }

        private IRandomAccessStream _saluteAudio;
        public IRandomAccessStream SaluteAudio
        {
            get
            {
                return _saluteAudio;
            }
            set
            {
                _saluteAudio = value;
                if (this.OnSaluteAudioInitialized != null)
                {
                    this.OnSaluteAudioInitialized();
                }
            }
        }

        private IRandomAccessStream _vibrationAudio;
        public IRandomAccessStream VibrationAudio
        {
            get
            {
                return _vibrationAudio;
            }
            set
            {
                _vibrationAudio = value;
                if (this.OnVibrationAudioInitialized != null)
                {
                    this.OnVibrationAudioInitialized();
                }
            }
        }

        private IRandomAccessStream _smehAudio;
        public IRandomAccessStream SmehAudio
        {
            get
            {
                return _smehAudio;
            }
            set
            {
                _smehAudio = value;
                if (this.OnSmehAudioInitialized != null)
                {
                    this.OnSmehAudioInitialized();
                }
            }
        }

        private void TryOnAllAudiosInitialized()
        {
            if (this.BackMusicAudio != null && this.ExplosionAudio != null && this.ShootAudio != null && this.RechargeAudio != null && this.EngineAudio != null
                && this.ChargerEmpty != null )
            {
                if (this.OnAllAudiosInitialized != null)
                {
                    this.OnAllAudiosInitialized();
                }
            }
        }
        public override void DeleteItself()
        {
            base.DeleteItself();
            _explosionAudio = null;
            _shootAudio = null;
            _rechargeAudio = null;
            _chargerEmpty = null;
            _engineAudio = null;
            _backMusicAudio = null;
            _saluteAudio = null;
            _vibrationAudio = null;
            _smehAudio = null;


        }
    }
    public delegate void AllAudiosInitialized();
    public delegate void SaluteAudioInitialized();
    public delegate void VibrationAudioInitialized();
    public delegate void SmehAudioInitialized();
}
