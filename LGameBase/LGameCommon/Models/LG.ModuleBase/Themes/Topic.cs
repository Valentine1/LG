using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Data;
using LG.Common;
using LG.Models.Licenses;

namespace LG.Models
{
    public class Topic : AssetM
    {
        public event AvatarChanged OnAvatarChanged;
        public event LicenseInfoChanged OnLicenseInfoChanged;

        private ObservableCollection<PictureBoxM> _pictures;
        public ObservableCollection<PictureBoxM> Pictures
        {
            get
            {
                if (_pictures == null)
                {
                    _pictures = new ObservableCollection<PictureBoxM>();
                }
                return _pictures;
            }
            set
            {
                _pictures = value;
            }
        }

        private ObservableCollection<Letter> _nameInLetters;
        public ObservableCollection<Letter> NameInLetters
        {
            get
            {
                if (_nameInLetters == null)
                {
                    _nameInLetters = new ObservableCollection<Letter>();
                }
                return _nameInLetters;
            }
            set
            {
                _nameInLetters = value;
            }
        }

        private Avatar _hierarchyLevel;
        public Avatar HierarchyLevel
        {
            get
            {
                return _hierarchyLevel;
            }
            set
            {
                _hierarchyLevel = value;
                if (this.OnAvatarChanged != null)
                {
                    this.OnAvatarChanged(_hierarchyLevel);
                }
            }
        }

        public string PathData { get; set; }
        private License _licenseInfo;
        public License LicenseInfo
        {
            get
            {
                return _licenseInfo;
            }
            set
            {
                _licenseInfo = value;
                if (this.OnLicenseInfoChanged != null)
                {
                    this.OnLicenseInfoChanged(_licenseInfo);
                }
            }
        }

        public Topic()
        {
        }
        public Topic(Theme th)
        {
            this.ID = th.ID;
            this.BlockSize = new Size() { Width = th.Width, Height = th.Height };
            this.StartPosition = new Point() { X = th.Left, Y = th.Top };
            this.Rotation = th.Rotation;
            this.PathData = th.PathData;
            this.TextValue = th.Name;
            this.LicenseInfo = th.LicenseInfo;

            if (this.LicenseInfo.IsActive)
            {
                this.HierarchyLevel = new Avatar()
                {
                    TextValue = th.HLevel.Name,
                    LevelNo = th.HLevel.Number,
                    BestTimeResult = th.HLevel.BestTimeResult,
                    PictureUrl = th.HLevel.PictureUrl,
                    TopBorderClr = th.HLevel.TopColor,
                    BottomBorderClr = th.HLevel.BottomColor
                };
            }
            else
            {
                this.HierarchyLevel = new Avatar()
                {
                    LevelNo = 0,
                    TopBorderClr = new Color("#89898989"),
                    BottomBorderClr = new Color("#89898989")
                };
            }
            if (th.NameInLetters != null)
            {
                foreach (Symbol s in th.NameInLetters)
                {
                    Letter l = new Letter(s);
                    this.NameInLetters.Add(l);
                }
            }
            if (th.Words != null)
            {
                foreach (Word w in th.Words)
                {
                    PictureBoxM p = new PictureBoxM() { WordID = w.ID, StartPosition = new Point() { X = w.Left, Y = w.Top }, Rotation = w.Rotation };
                    this.Pictures.Add(p);
                }
            }
        }

        async public Task<bool?> TryMoveToNextLevel(Profile p, Watch w)
        {
            bool? moveSuccess = false;
            int ms = (w.Minutes * 60 + w.Seconds) * 1000;
            p.HoursPlayed.Span = p.HoursPlayed.Span.Add(TimeSpan.FromMilliseconds(ms));
            if (this.HierarchyLevel.LevelNo < 12)
            {
                if (w.TotalMilliSeconds < this.HierarchyLevel.TimeThreshold)
                {
                    this.HierarchyLevel.LevelNo++;
                    moveSuccess = true;
                    Level l = await Module.Loader.GetLevelByNumber(this.HierarchyLevel.LevelNo);
                    this.HierarchyLevel.DeleteItself();
                    this.HierarchyLevel = new Avatar(l);
                    this.HierarchyLevel.BestTimeResult = 0;
                }
            }
            else
            {
                if (p.Type == ProfileType.Local)
                {
                    moveSuccess = await Module.GetProfLoader(p.Type).CheckIfBecomesOrRemainsAllSeeing(GlobalGameParams.AppLang, p.ID, new Theme { ID = this.ID, Name = this.TextValue }, w.TotalMilliSeconds);
                }
                else
                {
                    moveSuccess = await Module.GetProfLoader(p.Type).CheckIfBecomesOrRemainsAllSeeing(GlobalGameParams.AppLang, p.ID, new Theme { ID = this.ID, Name = this.TextValue }, w.TotalMilliSeconds);
                }
                if (moveSuccess == null)
                {
                    return moveSuccess;
                }

                if (!(bool)moveSuccess && this.HierarchyLevel.LevelNo == 13)
                {
                    Level l = await Module.Loader.GetLevelByNumber(--this.HierarchyLevel.LevelNo);
                    this.HierarchyLevel.DeleteItself();
                    this.HierarchyLevel = new Avatar(l);
                    this.HierarchyLevel.BestTimeResult = 0;
                }
            }

            if (w.TotalMilliSeconds < this.HierarchyLevel.BestTimeResult || this.HierarchyLevel.BestTimeResult == 0)
            {
                this.HierarchyLevel.BestTimeResult = w.TotalMilliSeconds;
                Theme th = null;
                if (this.HierarchyLevel.LevelNo < 13)
                {
                    th = new Theme() { ID = this.ID, Name = this.TextValue, HLevel = new Level() { BestTimeResult = w.TotalMilliSeconds, Number = this.HierarchyLevel.LevelNo } };
                }
                else
                {
                    th = new Theme() { ID = this.ID, Name = this.TextValue, HLevel = new Level() { BestTimeResult = w.TotalMilliSeconds, Number = 12 } };
                }
                ProfileDAL np = new ProfileDAL() { ID = p.ID, TimePlayed = (int)Math.Ceiling(p.HoursPlayed.Span.TotalMilliseconds / 1000) };
                bool wasServerUpdated = true;
                if (p.Type == ProfileType.Internet)
                {
                    if (!await Module.GetProfLoader(p.Type).SaveLevelHierarchy(np, th, GlobalGameParams.AppLang))
                    {
                        wasServerUpdated = false;
                    }
                }
                if (wasServerUpdated)
                {
                    await Module.GetProfLoader(ProfileType.Local).SaveLevelHierarchy(np, th, GlobalGameParams.AppLang);
                }
            }
            return moveSuccess;
        }

        public Topic DeepClone()
        {
            Topic top = new Topic();
            top.ID = this.ID;
            top.BlockSize = new Size() { Width = this.BlockSize.Width, Height = this.BlockSize.Height };
            top.StartPosition = new Point() { X = this.StartPosition.X, Y = this.StartPosition.Y };
            top.TextValue = this.TextValue;
            top.LicenseInfo = new License() { ID = this.LicenseInfo.ID, IsActive = this.LicenseInfo.IsActive };
            top.HierarchyLevel = new Avatar()
            {
                TextValue = this.HierarchyLevel.TextValue,
                LevelNo = this.HierarchyLevel.LevelNo,
                BestTimeResult = this.HierarchyLevel.BestTimeResult,
                PictureUrl = this.HierarchyLevel.PictureUrl,
                PictureSource = this.HierarchyLevel.PictureSource,
                TopBorderClr = this.HierarchyLevel.TopBorderClr,
                BottomBorderClr = this.HierarchyLevel.BottomBorderClr,
            };
            return top;
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            foreach (PictureBoxM pic in this.Pictures)
            {
                pic.DeleteItself();
            }
            this.Pictures.Clear();
            this.NameInLetters.Clear();
        }

    }
    public delegate void SelectedTopicChanged(Topic topic);
    public delegate void AvatarChanged(Avatar ava);
    public delegate void LicenseInfoChanged(License lic);
}
