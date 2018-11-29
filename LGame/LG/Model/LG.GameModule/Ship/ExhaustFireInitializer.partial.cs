using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public partial class ExhaustFireInitializer: Unit
    {
        public event ExhaustFireImageSourcesInitialized OnImageSourcesInitialized;

        private ObservableCollection<BitmapImage> _exhaustFireImageSource;
        private ObservableCollection<BitmapImage> ExhaustFireImageSources
        {
            get
            {
                if (this._exhaustFireImageSource == null)
                {
                    _exhaustFireImageSource = new ObservableCollection<BitmapImage>();
                }
                return _exhaustFireImageSource;
            }
        }

        async public void InitializeExhaustFireSource()
        {
            this.ExhaustFireImageSources.CollectionChanged += ExhaustFireImageSources_CollectionChanged;
            for (int i = 1; i <=4; i++)
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Images/flame/flame" + i.ToString() + ".png"));
                BitmapImage im = new BitmapImage();
                im.SetSource(await file.OpenAsync(FileAccessMode.Read));
                this.ExhaustFireImageSources.Add(im);
            }
        }

        private void ExhaustFireImageSources_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.ExhaustFireImageSources.Count == 4)
            {
                if (this.OnImageSourcesInitialized != null)
                {
                    this.OnImageSourcesInitialized();
                }
            }
        }

        public void AssignPictureBitmaps(AnimatedAssetM fire, bool random)
        {
            fire.SetPictureSources(this.ExhaustFireImageSources.ToList(), random);
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            this.ExhaustFireImageSources.Clear();
        }

        public override void DetachEvents()
        {
            base.DetachEvents();
            this.ExhaustFireImageSources.CollectionChanged -= ExhaustFireImageSources_CollectionChanged;
        }
    }

    public delegate void ExhaustFireImageSourcesInitialized();
}
