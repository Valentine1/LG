using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Streams;
using LG.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class AppSounds : UserControl
    {

        #region BackMusicSourceProperty
        public static readonly DependencyProperty BackMusicSourceProperty = DependencyProperty.Register("BackMusicSource",
                                                                                                typeof(IRandomAccessStream),
                                                                                                typeof(AppSounds),
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
            AppSounds gsoundsView = (AppSounds)d;
            gsoundsView.SetBackMusicSource();
        }
        #endregion

        public AppSounds()
        {
            this.InitializeComponent();

        }

        public void Initialize(AppSoundsVM soundsVM)
        {
            soundsVM.OnBackMusicStarted += Sounds_OnBackMusicStarted;
            soundsVM.OnBackMusicStopped += soundsVM_OnBackMusicStopped;
        }


        private void Sounds_OnBackMusicStarted()
        {
            backMusicPlayer.Play();
        }


        private void soundsVM_OnBackMusicStopped()
        {
            backMusicPlayer.Stop();
        }
        private void SetBackMusicSource()
        {
            this.backMusicPlayer.SetSource(this.BackMusicSource, "audio/mpeg");
        }

        private void backMusicPlayer_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (this.backMusicPlayer.CurrentState == MediaElementState.Closed)
            {
                this.backMusicPlayer.SetSource(this.BackMusicSource, "audio/mpeg");
            }
        }
    }
}
