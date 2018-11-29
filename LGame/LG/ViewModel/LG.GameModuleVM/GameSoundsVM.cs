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
    public class GameSoundsVM : UnitVM
    {
        private GameSounds GameSounds;

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
                this.NotifyPropertyChanged("ExplosionAudio");
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
                this.NotifyPropertyChanged("ShootAudio");
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
                this.NotifyPropertyChanged("RechargeAudio");
            }
        }

        private IRandomAccessStream _chargerEmptyAudio;
        public IRandomAccessStream ChargerEmptyAudio
        {
            get
            {
                return _chargerEmptyAudio;
            }
            set
            {
                _chargerEmptyAudio = value;
                this.NotifyPropertyChanged("ChargerEmptyAudio");
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
                this.NotifyPropertyChanged("EngineAudio");
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
                this.NotifyPropertyChanged("BackMusicAudio");
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
                this.NotifyPropertyChanged("SaluteAudio");
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
                this.NotifyPropertyChanged("VibrationAudio");
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
                this.NotifyPropertyChanged("SmehAudio");
            }
        }


        public GameSoundsVM(GameSounds sounds)
            : base(sounds)
        {
            this.GameSounds = sounds;
            this.GameSounds.OnAllAudiosInitialized += GameSounds_OnAllAudiosInitialized;
            this.GameSounds.OnSaluteAudioInitialized += GameSounds_OnSaluteAudioInitialized;
            this.GameSounds.OnVibrationAudioInitialized += GameSounds_OnVibrationAudioInitialized;
            this.GameSounds.OnSmehAudioInitialized += GameSounds_OnSmehAudioInitialized;
        }

        private void GameSounds_OnAllAudiosInitialized()
        {
            this.ExplosionAudio = this.GameSounds.ExplosionAudio;
            this.BackMusicAudio = this.GameSounds.BackMusicAudio;
            this.EngineAudio = this.GameSounds.EngineAudio;
            this.ShootAudio = this.GameSounds.ShootAudio;
            this.RechargeAudio = this.GameSounds.RechargeAudio;
            this.ChargerEmptyAudio = this.GameSounds.ChargerEmpty;

        }
        private void GameSounds_OnSaluteAudioInitialized()
        {
            this.SaluteAudio = this.GameSounds.SaluteAudio;
        }

        private void GameSounds_OnVibrationAudioInitialized()
        {
            this.VibrationAudio = this.GameSounds.VibrationAudio;
        }
        private void GameSounds_OnSmehAudioInitialized()
        {
            this.SmehAudio = this.GameSounds.SmehAudio;
        }

  
        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);
            _explosionAudio = null;
            _shootAudio = null;
            _rechargeAudio = null;
            _chargerEmptyAudio = null;
            _engineAudio = null;
            _backMusicAudio = null;
            _saluteAudio = null;
            _vibrationAudio = null;
            _smehAudio = null;
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            this.GameSounds.OnAllAudiosInitialized -= GameSounds_OnAllAudiosInitialized;
            this.GameSounds.OnSaluteAudioInitialized -= GameSounds_OnSaluteAudioInitialized;
            this.GameSounds.OnVibrationAudioInitialized -= GameSounds_OnVibrationAudioInitialized;
            this.GameSounds.OnSmehAudioInitialized -= GameSounds_OnSmehAudioInitialized;
        }
    }
}
