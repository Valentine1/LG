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
using LG.Common;
using LG.Data;
using LG.Models.Licenses;

namespace LG.Models
{
    public partial class Hierarchy
    {
        async public override Task Initialize()
        {
            base.Initialize();
            List<Level> levels = await Module.Loader.GetAllHierarchyLevels();
            this.PyramidM.Initialize(levels);
            List<Theme> themes = await Module.Loader.GetThemesByLang(GlobalGameParams.AppLang, Hierarchy.SelectedProfile.ID);
            themes = FilterActiveThemes(themes);
            foreach (Theme th in themes)
            {
                if (th.HLevel.Number == 12)
                {
                    bool? b = await Module.GetProfLoader(Hierarchy.SelectedProfile.Type).CheckIfBecomesOrRemainsAllSeeing(GlobalGameParams.AppLang, Hierarchy.SelectedProfile.ID, th, th.HLevel.BestTimeResult);
                    
                    if (b != null && (bool)b)
                    {
                        th.HLevel.Number++;
                        th.HLevel.TopColor = new Color("#FFFFFFFF");
                        th.HLevel.BottomColor = new Color("#FFFFFFFF");
                        th.HLevel.PictureUrl = "eye.png";
                        th.HLevel.Name = "AllSeeing";
                    }
                }
                   
                Topic top = new Topic(th);
                top.StartPosition = new Common.Point() { X = 0, Y = 0 };
                this.Topics.Add(top);
                if ((int)top.ID == Hierarchy.SelectedProfile.SelectedThemeID)
                {
                    top.DataFlow = Flow.FromModelToVM;
                    this.SelectedTopic = top;
                }
            }
            this.InitTopicFacePictures();
            this.InitSizesAndPositions();
        }

        private List<Theme> FilterActiveThemes( List<Theme> themes)
        {
            foreach (Theme th in themes)
            {
                th.LicenseInfo = LicenseManager.GetLicenseInfo(th.ID);
            }
            return (from t in themes where t.LicenseInfo.IsActive select t).ToList();
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
                    IRandomAccessStream ms = await EncryptionProvider.DecryptMedia(file);
                    picSource.SetSource(ms);
                    pic.PictureSource = picSource;
                }
            }
        }
        //private async void InitTopicFacePictures()
        //{
        //    IDataLoader loader = Module.LoaderFactory.CreateLoader(Module.LocalStoaragePath + "\\data");
        //    Dictionary<int, Word> words =await loader.GetWords(GlobalGameParams.AppLang);
        //    StorageFolder sf = ApplicationData.Current.LocalFolder;

        //    foreach (Topic top in this.Topics)
        //    {
        //        StorageFolder im = await sf.GetFolderAsync(@"images\" + top.TextValue + "\\");
        //        foreach (PictureBoxM pic in top.Pictures)
        //        {
        //            Word w = words[pic.WordID];
        //            pic.BoxColors = new PictureBoxColors()
        //            {
        //                BackColor = w.BackColor,
        //                BorderColor = w.BorderColor,
        //                LeftColor = w.LeftColor,
        //                LeftTopColor = w.LeftTopColor,
        //                TopColor = w.TopColor
        //            };
        //            BitmapImage picSource = new BitmapImage();
        //            StorageFile file = await im.GetFileAsync(w.PictureUrl);
        //            picSource.SetSource(await file.OpenAsync(FileAccessMode.Read));
        //            pic.PictureSource = picSource;
        //        }
        //    }
        //}
    }
}
