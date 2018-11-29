using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Streams;
using Windows.Storage;
using LG.ViewModels;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class GameSoundsV : UserControl
    {
        #region ExplosionSoundSourceProperty
        public static readonly DependencyProperty ExplosionSoundSourceProperty = DependencyProperty.Register("ExplosionSoundSource",
                                                                                                typeof(IRandomAccessStream),
                                                                                                typeof(GameSoundsV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(OnExplosionSoundSourceChanged))
                                                                                                );
        public IRandomAccessStream ExplosionSoundSource
        {
            get
            {
                return (IRandomAccessStream)GetValue(ExplosionSoundSourceProperty);
            }
            set
            {
                SetValue(ExplosionSoundSourceProperty, value);
            }
        }
        private static void OnExplosionSoundSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameSoundsV gsoundsView = (GameSoundsV)d;
            gsoundsView.SetExplosionSoundSource();
        }
        #endregion

        #region ShootSoundSourceProperty
        public static readonly DependencyProperty ShootSoundSourceProperty = DependencyProperty.Register("ShootSoundSource",
                                                                                                typeof(IRandomAccessStream),
                                                                                                typeof(GameSoundsV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(OnShootSoundSourceChanged))
                                                                                                );
        public IRandomAccessStream ShootSoundSource
        {
            get
            {
                return (IRandomAccessStream)GetValue(ShootSoundSourceProperty);
            }
            set
            {
                SetValue(ShootSoundSourceProperty, value);
            }
        }
        private static void OnShootSoundSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameSoundsV gsoundsView = (GameSoundsV)d;
            gsoundsView.SetShootSoundSource();
        }
        #endregion

        #region RechargeSoundSourceProperty
        public static readonly DependencyProperty RechargeSoundSourceProperty = DependencyProperty.Register("RechargeSoundSource",
                                                                                                typeof(IRandomAccessStream),
                                                                                                typeof(GameSoundsV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(OnRechargeSoundSourceChanged))
                                                                                                );
        public IRandomAccessStream RechargeSoundSource
        {
            get
            {
                return (IRandomAccessStream)GetValue(RechargeSoundSourceProperty);
            }
            set
            {
                SetValue(RechargeSoundSourceProperty, value);
            }
        }
        private static void OnRechargeSoundSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameSoundsV gsoundsView = (GameSoundsV)d;
            gsoundsView.SetRechargeSoundSource();
        }
        #endregion

        #region ChargerEmptySoundSourceProperty
        public static readonly DependencyProperty ChargerEmptySoundSourceProperty = DependencyProperty.Register("ChargerEmptySoundSource",
                                                                                                typeof(IRandomAccessStream),
                                                                                                typeof(GameSoundsV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(OnChargerEmptySoundSourceChanged))
                                                                                                );
        public IRandomAccessStream ChargerEmptySoundSource
        {
            get
            {
                return (IRandomAccessStream)GetValue(ChargerEmptySoundSourceProperty);
            }
            set
            {
                SetValue(ChargerEmptySoundSourceProperty, value);
            }
        }
        private static void OnChargerEmptySoundSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameSoundsV gsoundsView = (GameSoundsV)d;
            gsoundsView.SetChargerEmptySoundSource();
        }
        #endregion

        #region EngineSoundSourceProperty
        public static readonly DependencyProperty EngineSoundSourceProperty = DependencyProperty.Register("EngineSoundSource",
                                                                                                typeof(IRandomAccessStream),
                                                                                                typeof(GameSoundsV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(OnEngineSoundSourceChanged))
                                                                                                );
        public IRandomAccessStream EngineSoundSource
        {
            get
            {
                return (IRandomAccessStream)GetValue(EngineSoundSourceProperty);
            }
            set
            {
                SetValue(EngineSoundSourceProperty, value);
            }
        }
        private static void OnEngineSoundSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameSoundsV gsoundsView = (GameSoundsV)d;
            gsoundsView.SetEngineSoundSource();
        }
        #endregion

        #region BackMusicSourceProperty
        public static readonly DependencyProperty BackMusicSourceProperty = DependencyProperty.Register("BackMusicSource",
                                                                                                typeof(IRandomAccessStream),
                                                                                                typeof(GameSoundsV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(OnBackMusicSourceChanged))
                                                                                                );
        public IRandomAccessStream BackMusicSource
        {
            get
            {
                return (IRandomAccessStream)GetValue(BackMusicSourceProperty);
            }
            set
            {
                SetValue(BackMusicSourceProperty, value);
            }
        }
        private static void OnBackMusicSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameSoundsV gsoundsView = (GameSoundsV)d;
            gsoundsView.SetBackMusicSource();
        }
        #endregion

        #region SaluteSoundSourceProperty
        public static readonly DependencyProperty SaluteSoundSourceProperty = DependencyProperty.Register("SaluteSoundSource",
                                                                                                typeof(IRandomAccessStream),
                                                                                                typeof(GameSoundsV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(OnSaluteSoundSourceChanged))
                                                                                                );
        public IRandomAccessStream SaluteSoundSource
        {
            get
            {
                return (IRandomAccessStream)GetValue(SaluteSoundSourceProperty);
            }
            set
            {
                SetValue(SaluteSoundSourceProperty, value);
            }
        }
        private static void OnSaluteSoundSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameSoundsV gsoundsView = (GameSoundsV)d;
            gsoundsView.SetSaluteSoundSource();
        }
        #endregion

        #region WordSoundSourceProperty
        public static readonly DependencyProperty WordSoundSourceProperty = DependencyProperty.Register("WordSoundSource",
                                                                                                typeof(IRandomAccessStream),
                                                                                                typeof(GameSoundsV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(WordSoundSourceChanged))
                                                                                                );
        public IRandomAccessStream WordSoundSource
        {
            get
            {
                return (IRandomAccessStream)GetValue(WordSoundSourceProperty);
            }
            set
            {
                SetValue(WordSoundSourceProperty, value);
            }
        }
        private static void WordSoundSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameSoundsV gsoundsView = (GameSoundsV)d;
            gsoundsView.SetWordSoundSource();
        }
        #endregion

        #region VibrationSourceProperty
        public static readonly DependencyProperty VibrationSoundSourceProperty = DependencyProperty.Register("VibrationSoundSource",
                                                                                                typeof(IRandomAccessStream),
                                                                                                typeof(GameSoundsV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(VibrationSoundSourceChanged))
                                                                                                );
        public IRandomAccessStream VibrationSoundSource
        {
            get
            {
                return (IRandomAccessStream)GetValue(VibrationSoundSourceProperty);
            }
            set
            {
                SetValue(VibrationSoundSourceProperty, value);
            }
        }
        private static void VibrationSoundSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameSoundsV gsoundsView = (GameSoundsV)d;
            gsoundsView.SetVibrationSoundSource();
        }
        #endregion

        #region SmehSoundSourceProperty
        public static readonly DependencyProperty SmehSoundSourceProperty = DependencyProperty.Register("SmehSoundSource",
                                                                                                typeof(IRandomAccessStream),
                                                                                                typeof(GameSoundsV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(SmehSoundSourceChanged))
                                                                                                );
        public IRandomAccessStream SmehSoundSource
        {
            get
            {
                return (IRandomAccessStream)GetValue(SmehSoundSourceProperty);
            }
            set
            {
                SetValue(SmehSoundSourceProperty, value);
            }
        }
        private static void SmehSoundSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameSoundsV gsoundsView = (GameSoundsV)d;
            gsoundsView.SetSmehSoundSource();
        }
        #endregion

        public GameSoundsV()
        {
            this.InitializeComponent();
        }
        public void Initialize(GameSoundsVM gsvm )
        {
            this.DataContext = gsvm;
            gsvm.OnItselfDeleted += gsvm_OnItselfDeleted;
         
        }

        public void PlayBackMusic()
        {
            this.backMusicPlayer.Play();
        }
        public void PlayEngines()
        {
            this.engineMusicPlayer.Play();
        }
        public void PlayExplosion()
        {
            this.explMusicPlayer.Play();
        }
        public void PlayShoot()
        {
            this.shootMusicPlayer.Play();
        }
        public void PlayRecharge()
        {
            this.rechargeMusicPlayer.Play();
        }
        public void PlayChargerEmpty()
        {
            this.chargerEmptyPlayer.Play();
        }
        public void PlaySalute()
        {
            this.saluteMusicPlayer.Play();
        }
     
        public void PlayVibration()
        {
            this.vibroMusicPlayer.Play();
        }
        public void PlaySmeh()
        {
            this.smehMusicPlayer.Play();
        }
        public void StopBackMusic()
        {
            this.backMusicPlayer.Stop();
        }
        public void StopEngineMusic()
        {
            this.engineMusicPlayer.Stop();
        }

        private void SetExplosionSoundSource()
        {
            this.explMusicPlayer.SetSource(this.ExplosionSoundSource, "audio/mpeg");
        }
        private void SetShootSoundSource()
        {
            this.shootMusicPlayer.SetSource(this.ShootSoundSource, "audio/mpeg");
        }
        private void SetRechargeSoundSource()
        {
            this.rechargeMusicPlayer.SetSource(this.RechargeSoundSource, "audio/mpeg");
        }
        private void SetChargerEmptySoundSource()
        {
            this.chargerEmptyPlayer.SetSource(this.ChargerEmptySoundSource, "audio/mpeg");
        }
        private void SetEngineSoundSource()
        {
            this.engineMusicPlayer.SetSource(this.EngineSoundSource, "audio/mpeg");
        }
        private void SetWordSoundSource()
        {
            if (this.WordSoundSource != null)
            {
                this.wordSoundPlayer.SetSource(this.WordSoundSource, "audio/mpeg");
            }
        }
        private void SetBackMusicSource()
        {
            this.backMusicPlayer.SetSource(this.BackMusicSource, "audio/mpeg");
        }
        private void SetSaluteSoundSource()
        {
            this.saluteMusicPlayer.SetSource(this.SaluteSoundSource, "audio/mpeg");
        }
        private void SetVibrationSoundSource()
        {
            this.vibroMusicPlayer.SetSource(this.VibrationSoundSource, "audio/mpeg");
        }
        private void SetSmehSoundSource()
        {
            this.smehMusicPlayer.SetSource(this.SmehSoundSource, "audio/mpeg");
        }

        void gsvm_OnItselfDeleted(UnitVM uvm)
        {
            DetachEvents(uvm as GameSoundsVM);
            this.explMusicPlayer.Source = null;
            this.shootMusicPlayer.Source = null;
            this.rechargeMusicPlayer.Source = null;
            this.chargerEmptyPlayer.Source = null;
            this.engineMusicPlayer.Source = null;
            this.wordSoundPlayer.Source = null;
            this.backMusicPlayer.Source = null;
            this.saluteMusicPlayer.Source = null;
            vibroMusicPlayer.Source = null;
            smehMusicPlayer.Source = null;
        }

        private void DetachEvents(GameSoundsVM gsvm)
        {
            gsvm.OnItselfDeleted -= gsvm_OnItselfDeleted;
        }

    }
}
