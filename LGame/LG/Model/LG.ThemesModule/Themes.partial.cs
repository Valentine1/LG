using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Security.Cryptography;
using LG.Common;
using LG.Data;
using LG.Models.Licenses;
using LGservices;

namespace LG.Models
{
    public partial class Themes : Module
    {
        public async override Task Initialize()
        {
            base.Initialize();
            List<Level> levels = await Module.Loader.GetAllHierarchyLevels();
            await this.InitAvatars(levels);
            List<Theme> themes = await Module.Loader.GetThemesByLang(GlobalGameParams.AppLang, Themes.SelectedProfile.ID);
            foreach (Theme th in themes)
            {
                th.LicenseInfo = LicenseManager.GetLicenseInfo(th.ID);
                if (th.HLevel.Number == 12)
                {
                    bool? b = await Module.GetProfLoader(Themes.SelectedProfile.Type).CheckIfBecomesOrRemainsAllSeeing(GlobalGameParams.AppLang, Themes.SelectedProfile.ID, th, th.HLevel.BestTimeResult);
                    if (b!= null && (bool)b)
                    {
                        th.HLevel.Number++;
                        th.HLevel.TopColor = new Color("#FFFFFFFF");
                        th.HLevel.BottomColor = new Color("#DDFFFFFF");
                        th.HLevel.PictureUrl = "eye.png";
                        th.HLevel.Name = "AllSeeing";
                    }
                }
                Topic top = new Topic(th);
                this.Topics.Add(top);
                if ((int)top.ID == Themes.SelectedProfile.SelectedThemeID)
                {
                    this.SelectedTopic = top;
                }
            }
            this.InitTopicFacePictures();
            this.DNASpiral.SwitchOn();
            DataController dc = new DataController();
            List<int> thIds =  await dc.LoadNewAvailableThemesFromServer(++themes[themes.Count - 1].ID, 2);
            if (thIds!= null)
            {
                List<Theme> newThemes = await Module.Loader.GetThemesByLang(GlobalGameParams.AppLang, Themes.SelectedProfile.ID, thIds);
                ObservableCollection<Topic> newTops = new ObservableCollection<Topic>();
                foreach (Theme th in newThemes)
                {
                    th.LicenseInfo = LicenseManager.GetLicenseInfo(th.ID);
                    Topic top = new Topic(th);
                    this.Topics.Add(top);
                    newTops.Add(top);
                }
                InitTopicFacePictures(newTops);
            }
        }

        private async Task InitAvatars(List<Level> levels)
        {
            foreach (Level l in levels)
            {
                Avatar av = new Avatar(l);
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Images/hierarchy/" + l.PictureUrl));
                av.PictureSource = new BitmapImage();
                av.PictureSource.SetSource(await file.OpenReadAsync());
                av.ScaleX = ThemesParams.AvatarScale;
                av.ScaleY = ThemesParams.AvatarScale;
                av.BlockSize = new Common.Size() { Width = ThemesParams.AvatarBorderWidth * ThemesParams.AvatarScale, Height = ThemesParams.AvatarBorderHeight * ThemesParams.AvatarScale };
                Avatars.Add(av);
            }
        }

        private async void InitTopicFacePictures()
        {
            this.InitTopicFacePictures(this.Topics);
        }
        private async void InitTopicFacePictures(ObservableCollection<Topic> topics)
        {
            Dictionary<int, Word> words = await Module.Loader.GetWords(GlobalGameParams.AppLang);
            StorageFolder sf = ApplicationData.Current.LocalFolder;

            foreach (Topic top in topics)
            {
                StorageFolder im = await sf.GetFolderAsync(@"images\" + top.TextValue + "\\");
                foreach (PictureBoxM pic in top.Pictures)
                {
                    Word w = words[pic.WordID];
                    pic.BoxColors = new PictureBoxColors()
                    {
                        BackColor = w.BackColor,
                        BorderColor = w.BorderColor,
                        LeftColor = w.LeftColor,
                        LeftTopColor = w.LeftTopColor,
                        TopColor = w.TopColor
                    };
                    BitmapImage picSource = new BitmapImage();
                    StorageFile file = await im.GetFileAsync(w.PictureUrl);
                    IRandomAccessStream ms =await EncryptionProvider.DecryptMedia(file);
                    picSource.SetSource(ms);
                    pic.PictureSource = picSource;
                }
            }
        }
    }
}
