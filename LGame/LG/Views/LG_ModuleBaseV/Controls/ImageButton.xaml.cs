using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.Windows.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class ImageButton : UserControl, INotifyPropertyChanged
    {
        public event Click OnClick;

        private BitmapImage _imSourceNormal;
        public BitmapImage ImSourceNormal
        {
            get
            {
                return _imSourceNormal;
            }
            set
            {
                _imSourceNormal = value;
                this.NotifyPropertyChanged("ImSourceNormal");
            }
        }

        private BitmapImage _imSourceOver;
        public BitmapImage ImSourceOver
        {
            get
            {
                return _imSourceOver;
            }
            set
            {
                _imSourceOver = value;
                this.NotifyPropertyChanged("ImSourceOver");
            }
        }

        private BitmapImage _imSourcePressed;
        public BitmapImage ImSourcePressed
        {
            get
            {
                return _imSourcePressed;
            }
            set
            {
                _imSourcePressed = value;
                this.NotifyPropertyChanged("ImSourcePressed");
            }
        }

        private string ImageNormal { get; set; }
        private string ImagePressed { get; set; }
        private string ImageOver { get; set; }

        private StorageFolder localStorFolder { get; set; }
        private StorageFolder buttonsFolder { get; set; }
        #region INotifyPropertyChanged implement
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public ImageButton()
        {
            this.InitializeComponent();

            btImage.DataContext = this;
        }
        async public void Initialize(string imNormal, string imPressed, string imOver)
        {
            this.ImageNormal = imNormal;
            this.ImagePressed = imPressed;
            this.ImageOver = imOver;

            this.ImSourceNormal = await this.GetSource(this.ImageNormal);
            this.ImSourceOver = await this.GetSource(this.ImageOver);
            this.ImSourcePressed = await this.GetSource(this.ImagePressed);
        }

        private async Task<BitmapImage> GetSource(string imPath)
        {
            StorageFile sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Images/ButtonsImages/"+imPath));
            BitmapImage bsSource = new BitmapImage();
            bsSource.SetSource(await sf.OpenAsync(FileAccessMode.Read));
            return bsSource;
        }

        private void btImage_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            if(this.OnClick != null)
            {
                this.OnClick(this);
            }
        }

        private void btImage_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.OnClick != null)
            {
                this.OnClick(this);
            }
        }

        private void btImage_PointerEntered_1(object sender, PointerRoutedEventArgs e)
        {
            MenuItemChangePlayer.Play();
        }

        private void MenuItemChangePlayer_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (MenuItemChangePlayer.CurrentState == MediaElementState.Closed)
            {
                MenuItemChangePlayer.Source = new Uri("ms-appx:///Sounds/Shuh.mp3"); 
            }
        }

    }

    public delegate void Click (ImageButton sender);
}
