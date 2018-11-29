using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using LG.Models;
using LG.Common;

namespace LG.ViewModels
{
    public class ProfileVM : BaseBlockVM
    {
        public int ID { get; set; }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                NotifyPropertyChanged("Password");
            }
        }

        private string _contactInfo;
        public string ContactInfo
        {
            get
            {
                return _contactInfo;
            }
            set
            {
                _contactInfo = value;
                NotifyPropertyChanged("ContactInfo");
            }
        }

        private WatchVM _bestTimeVModel;
        public WatchVM BestTimeVModel
        {
            get
            {
                return _bestTimeVModel;
            }
            set
            {
                _bestTimeVModel = value;
                this.NotifyPropertyChanged("BestTimeVModel");
            }
        }

        private RegisterMode _profileRegisterMode;
        public RegisterMode ProfileRegisterMode
        {
            get
            {
                return _profileRegisterMode;
            }
            set
            {
                _profileRegisterMode = value;
                NotifyPropertyChanged("ProfileRegisterMode");
            }
        }

        public IRandomAccessStream PictureStream { get; set; }

        private BitmapImage _pictureSource;
        public BitmapImage PictureSource
        {
            get
            {
                return _pictureSource;
            }
            set
            {
                _pictureSource = value;
                this.NotifyPropertyChanged("PictureSource");
            }
        }

        private int _hoursPlayed;
        public int HoursPlayed
        {
            get
            {
                return _hoursPlayed;
            }
            set
            {
                _hoursPlayed = value;
                this.NotifyPropertyChanged("HoursPlayed");
            }
        }

        private int _minutesPlayed;
        public int MinutesPlayed
        {
            get
            {
                return _minutesPlayed;
            }
            set
            {
                _minutesPlayed = value;
                this.NotifyPropertyChanged("MinutesPlayed");
            }
        }

        private bool _hasPhoto;
        public bool HasPhoto
        {
            get
            {
                return _hasPhoto;
            }
            set
            {
                _hasPhoto = value;
                NotifyPropertyChanged("HasPhoto");
            }
        }

        public ProfileVM()
        {
        }
 
        public ProfileVM(Profile p):base(p)
        {
            this.ID = p.ID;
            this.Name = p.Name;
            this.HasPhoto = p.HasPhoto;
            BestTimeVModel = new WatchVM(p.BestTime);

            this.HoursPlayed = (int)Math.Floor(p.HoursPlayed.Span.TotalHours);
            this.MinutesPlayed = p.HoursPlayed.Span.Minutes;
            this.ContactInfo = p.ContactInfo;
            this.ProfileRegisterMode = p.Type == Data.ProfileType.Local ? RegisterMode.Local : RegisterMode.Internet;
            p.OnPictureSourceChanged += p_OnPictureSourceChanged;
            p.OnBestTimeChanged += p_OnBestTimeChanged;
            this.PictureSource = p.PictureSource;
        }

        private void p_OnBestTimeChanged(TimeRange bTime)
        {
            BestTimeVModel.ReInit(bTime);
        }

        private void p_OnPictureSourceChanged(BitmapImage pic)
        {
            this.PictureSource = pic;
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            (m as Profile).OnPictureSourceChanged -= p_OnPictureSourceChanged;
            (m as Profile).OnBestTimeChanged -= p_OnBestTimeChanged;
        }
    }


    public enum RegisterMode { Local, Internet, Edit }
}
