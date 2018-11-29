using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Data;
using LG.Common;

namespace LG.Models
{
    public partial class Profile : BaseBlock
    {
        public event BestTimeChanged OnBestTimeChanged;

        public int ID { get; set; }

        private TimeRange _bestTime;
        public TimeRange BestTime
        {
            get
            {
                return _bestTime;
            }
            set
            {
                _bestTime = value;
                if (this.OnBestTimeChanged != null)
                {
                    this.OnBestTimeChanged(_bestTime);
                }
            }
        }
        public int SelectedThemeID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ContactInfo { get; set; }
        public bool HasPhoto { get; set; }
        public ProfileType Type { get; set; }
        public string PicUrl { get; set; }
        public TimeRange HoursPlayed
        {
            get;
            set;
        }
        protected bool TakePhotoFromServer { get; set; }

        public Profile(ProfileDAL p, bool takeFromServer)
        {
            this.TakePhotoFromServer = takeFromServer;
            this.ID = p.ID;
            this.Name = p.Name;
            this.Password = p.Password;
            this.ContactInfo = p.ContactInfo;
            this.SelectedThemeID = p.LastThemeID;
            this.BestTime = new TimeRange(p.BestTime);
            switch (p.Type)
            {
                case 0:
                    this.Type = ProfileType.Local;
                    break;
                case 1:
                    this.Type = ProfileType.Internet;
                    break;
            }
            this.HoursPlayed = new TimeRange(p.TimePlayed * 1000);
            this.HasPhoto = p.HasPhoto;

            if (!p.HasPhoto)
            {
                p.PictureUrl = "NoPhoto.png";
            }

            this.InitPicture(p);
            this.PicUrl = p.PictureUrl;

        }
        public Profile()
        {

        }
    }

    public delegate void BestTimeChanged(TimeRange bTime);

}