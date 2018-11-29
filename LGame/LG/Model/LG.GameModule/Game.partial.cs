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
using LG.Models.Licenses;

namespace LG.Models
{
    public partial class Game
    {
        private GameSounds _sounds;
        public GameSounds Sounds
        {
            get
            {
                if (_sounds == null)
                {
                    _sounds = new GameSounds();
                }
                return _sounds;
            }
        }

        async public override Task Initialize()
        {
            base.Initialize();
            if (Game.GameTopic == null)
            {
                Game.SelectedProfile.SelectedThemeID = Game.SelectedProfile.SelectedThemeID == 0 ? 1 : Game.SelectedProfile.SelectedThemeID;
                Theme theme = null;
                try
                {
                    theme = await Module.Loader.GetThemeById(GlobalGameParams.AppLang, Game.SelectedProfile.ID, Game.SelectedProfile.SelectedThemeID);
                }
                catch
                {
                    theme = new Theme()
                    {
                        HLevel = new Level()
                        {
                            BottomColor = new Color() { A = 182, B = 255, G = 255, R = 255 },
                            TopColor = new Color() { A = 198, B = 0, G = 17, R = 255 },
                            Name = "Peon",
                            Number = 1,
                            PictureUrl = "slave.png"
                        },
                        ID = 1,
                        Name = "weather",
                        NativeName = "weather"
                    };
                }
                theme.LicenseInfo = LicenseManager.GetLicenseInfo(theme.ID);
                Game.GameTopic = new Topic(theme);
                if (this.OnSelectedTopicChanged != null)
                {
                    this.OnSelectedTopicChanged(Game.GameTopic);
                }
            }
            //else
            //{
            //    //loader.CheckIfExistAndAddProfileTheme(GlobalGameParams.AppLang, Game.SelectedProfile.ID, (int)Game.GameTopic.ID);
            //}
            await this.SpaceController.Initialize();
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            this.InitExplosionAudio();
            this.InitBackMusicAudio();
            this.InitShootAudio();
            this.InitRechargeAudio();
            this.InitEmptyFire();
            this.InitEngineAudio();
            this.InitSaluteAudio();
            this.InitVibrationAudio();
            this.InitSmehAudio();
        }

        private async void InitExplosionAudio()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Sounds/expl1.wav"));
            this.Sounds.ExplosionAudio = await file.OpenReadAsync();
        }
        private async void InitBackMusicAudio()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Sounds/starsng.wav"));
            this.Sounds.BackMusicAudio = await file.OpenReadAsync();
        }
        private async void InitSaluteAudio()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Sounds/Salute.mp3"));
            this.Sounds.SaluteAudio = await file.OpenReadAsync();
        }
        private async void InitVibrationAudio()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Sounds/Vibro.mp3"));
            this.Sounds.VibrationAudio = await file.OpenReadAsync();
        }
        private async void InitSmehAudio()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Sounds/smeh.mp3"));
            this.Sounds.SmehAudio = await file.OpenReadAsync();
        }
        private async void InitShootAudio()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Sounds/pulse.wav"));
            this.Sounds.ShootAudio = await file.OpenReadAsync();
        }
        private async void InitRechargeAudio()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Images/Recharge2.mp3"));
            this.Sounds.RechargeAudio = await file.OpenReadAsync();
        }
        private async void InitEmptyFire()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Images/ChargerEmpty.wav"));
            this.Sounds.ChargerEmpty = await file.OpenReadAsync();
        }
        private async void InitEngineAudio()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Sounds/engines.wav"));
            this.Sounds.EngineAudio = await file.OpenReadAsync();
        }
    }


}
