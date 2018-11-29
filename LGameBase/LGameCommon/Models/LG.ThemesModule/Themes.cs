using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Windows.UI.Popups;
using LG.Common;
using LG.Data;
using LG.Models.Licenses;
using LG.Logging;
using LGservices;

namespace LG.Models
{
    public partial class Themes : Module
    {
        public event AvatarsInitialized OnAvatarsInitialized;
        public event SelectedTopicChanged OnSelectedTopicChanged;
        public event ThemesGroupSuggestedToBuy OnThemesGroupSuggestedToBuy;

        private ObservableCollection<Avatar> _avatars;
        public ObservableCollection<Avatar> Avatars
        {
            get
            {
                if (_avatars == null)
                {
                    _avatars = new ObservableCollection<Avatar>();
                }
                return _avatars;
            }
            set
            {
                _avatars = value;
            }
        }

        private ObservableCollection<Topic> _topics;
        public ObservableCollection<Topic> Topics
        {
            get
            {
                if (_topics == null)
                {
                    _topics = new ObservableCollection<Topic>();
                }
                return _topics;
            }
        }

        private Topic _selectedTopic;
        public Topic SelectedTopic
        {
            get
            {
                return _selectedTopic;
            }
            set
            {
                _selectedTopic = value;
                if (this.OnSelectedTopicChanged != null)
                {
                    this.OnSelectedTopicChanged(_selectedTopic);
                }
            }
        }

        private BaseBlock _topicsArea;
        public BaseBlock TopicsArea
        {
            get
            {
                if (_topicsArea == null)
                {
                    _topicsArea = new BaseBlock();
                }
                return _topicsArea;
            }
        }

        public static Profile SelectedProfile { get; set; }

        private DNASpiralM _dNASpiral;
        public DNASpiralM DNASpiral
        {
            get
            {
                return _dNASpiral;
            }
        }

        public Themes(Profile prof)
            : base()
        {
            Themes.SelectedProfile = prof;
            this.NavigationMethod = ModuleNavigationMethod.Shuffle;
            ThemesParams.Initialize();

            double aspect = (double)GlobalGameParams.WindowWidth / (double)GlobalGameParams.WindowHeight;

            if (aspect < 1.5)
            {
                this.CalcSizesAndPositionsForSquareScreen();
                this.Avatars.CollectionChanged += AvatarsSquare_CollectionChanged;
            }
            else
            {
                this.CalcSizesAndPositionsForOblongScreen();
                this.Avatars.CollectionChanged += AvatarsOblong_CollectionChanged;
            }

            _dNASpiral = new DNASpiralM(ThemesParams.LegendWidth * 0.9, ThemesParams.LegendHeight);
            this.DNASpiral.StartPosition = new Point() { X = (ThemesParams.LegendWidth - this.DNASpiral.BlockSize.Width) / 2d, Y = 0 };
            this.TopicsArea.StartPosition = new Point() { X = (GlobalGameParams.WindowWidth - ThemesParams.LegendWidth) / 2, Y = GlobalGameParams.WindowHeight / 2 };
            this.TopicsArea.ScaleX = 1.0 + (double)(GlobalGameParams.WindowWidth - 1024) * 0.51 / 1526;
            this.TopicsArea.ScaleY = this.TopicsArea.ScaleX;
        }

        public void SelectedTopicChangedFromUI(Topic t)
        {
                this.SelectedTopic = t;
        }
        public void TrySelectInactiveTopic(Topic t)
        {
            this.SuggestToBuyThemesGroup(t);
        }
        async public void SuggestToBuyThemesGroup(Topic top)
        {
            try
            {
                this.StartProgressBar(GlobalGameParams.Messages.Buying);
                List<Topic> tps = (from t in this.Topics where t.LicenseInfo.ID == top.LicenseInfo.ID select t).ToList();
                if (this.OnThemesGroupSuggestedToBuy != null)
                {
                    this.OnThemesGroupSuggestedToBuy(tps);
                }
                bool res = await Licenses.LicenseManager.BuyThemes(tps);
                if (res)
                {
                    DataController dc = new DataController();
                    List<int> thIds = await dc.LoadFullThemesAndResources(tps);
                    foreach (Topic t in tps)
                    {
                        t.LicenseInfo = LicenseManager.GetLicenseInfo(t.ID);
                        t.HierarchyLevel = new Avatar()
                        {
                            TextValue = "Peon",
                            LevelNo = 1,
                            BestTimeResult = 0,
                            PictureUrl ="slave.png",
                            TopBorderClr = new Color("#E6FF1100"),
                            BottomBorderClr = new Color("#E6FFFFFF")
                        };  
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ShowMessageAndLog(ex);
            }
            finally
            {
                this.EndProgressBar();
            }
        }

        public void SaveSelectedTopic()
        {
            if (this.SelectedTopic != null)
            {
                Module.Loader.SetLastTheme(Themes.SelectedProfile.ID, (int)this.SelectedTopic.ID);
            }
        }

        public void SwitchBigSpiralOn()
        {
            DNASpiral.SwitchOn();
        }
        public void SwitchBigSpiralOff()
        {
            DNASpiral.SwitchOff();
        }

        #region initialization
        private void AvatarsSquare_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (this.Avatars.Count == 13)
                {
                    var q = this.Avatars.OrderBy(x => x.LevelNo);
                    this.Avatars = new ObservableCollection<Avatar>(q);
                    this.Avatars[0].StartPosition = new Point() { X = ThemesParams.AvatarOffsetX, Y = ThemesParams.LegendHeight - this.Avatars[0].BlockSize.Height };
                    double w = this.Avatars[0].BlockSize.Width;
                    this.Avatars[1].StartPosition = new Point() { X = this.Avatars[0].StartPosition.X + w + ThemesParams.AvatarOffsetX, Y = this.Avatars[0].StartPosition.Y - ThemesParams.AvatarOffsetY };
                    this.Avatars[2].StartPosition = new Point() { X = this.Avatars[1].StartPosition.X + w + ThemesParams.AvatarOffsetX, Y = this.Avatars[0].StartPosition.Y - 2 * ThemesParams.AvatarOffsetY };
                    this.Avatars[3].StartPosition = new Point() { X = this.Avatars[1].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 3 * ThemesParams.AvatarOffsetY };
                    this.Avatars[4].StartPosition = new Point() { X = this.Avatars[0].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 4 * ThemesParams.AvatarOffsetY };
                    this.Avatars[5].StartPosition = new Point() { X = this.Avatars[1].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 5 * ThemesParams.AvatarOffsetY };
                    this.Avatars[6].StartPosition = new Point() { X = this.Avatars[2].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 6 * ThemesParams.AvatarOffsetY };
                    this.Avatars[7].StartPosition = new Point() { X = this.Avatars[1].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 7 * ThemesParams.AvatarOffsetY };
                    this.Avatars[8].StartPosition = new Point() { X = this.Avatars[0].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 8 * ThemesParams.AvatarOffsetY };
                    this.Avatars[9].StartPosition = new Point() { X = this.Avatars[1].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 9 * ThemesParams.AvatarOffsetY };
                    this.Avatars[10].StartPosition = new Point() { X = this.Avatars[2].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 10 * ThemesParams.AvatarOffsetY };
                    this.Avatars[11].StartPosition = new Point() { X = this.Avatars[1].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 11 * ThemesParams.AvatarOffsetY };

                    double extraTopFreeHeight = (GlobalGameParams.WindowHeight - ThemesParams.LegendHeight) / 2d;
                    double topFreeHeight = this.Avatars[11].StartPosition.Y + extraTopFreeHeight;
                    double topMargin = topFreeHeight * 0 / 100d;
                    double bottomMargin = topFreeHeight * 0 / 100d;
                    double h = topFreeHeight - topMargin - bottomMargin;
                    this.Avatars[12].BlockSize = new Size() { Height = h, Width = (double)this.Avatars[12].RealPictureWidth / (double)this.Avatars[12].RealPictureHeight * h };
                    this.Avatars[12].StartPosition = new Point() { Y = topMargin - extraTopFreeHeight, X = (ThemesParams.LegendWidth - this.Avatars[12].BlockSize.Width) / 2 };

                    if (this.OnAvatarsInitialized != null)
                    {
                        this.OnAvatarsInitialized(this.Avatars);
                    }
                }
            }
        }

        private void AvatarsOblong_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (this.Avatars.Count == 13)
                {
                    double ydisplace = 22 * GlobalGameParams.WindowWidth / 1920d;

                    var q = this.Avatars.OrderBy(x => x.LevelNo);
                    this.Avatars = new ObservableCollection<Avatar>(q);
                    this.Avatars[0].StartPosition = new Point() { X = ThemesParams.AvatarOffsetX, Y = ThemesParams.LegendHeight - this.Avatars[0].BlockSize.Height };
                    double w = this.Avatars[0].BlockSize.Width;

                    this.Avatars[1].StartPosition = new Point() { X = this.Avatars[0].StartPosition.X + w + ThemesParams.AvatarOffsetX, Y = this.Avatars[0].StartPosition.Y - ThemesParams.AvatarOffsetY };
                    this.Avatars[2].StartPosition = new Point() { X = this.Avatars[1].StartPosition.X + w + ThemesParams.AvatarOffsetX, Y = this.Avatars[0].StartPosition.Y - 2 * ThemesParams.AvatarOffsetY + ydisplace };
                    this.Avatars[3].StartPosition = new Point() { X = this.Avatars[2].StartPosition.X + w + ThemesParams.AvatarOffsetX, Y = this.Avatars[0].StartPosition.Y - 3 * ThemesParams.AvatarOffsetY + ydisplace };
                    this.Avatars[4].StartPosition = new Point() { X = this.Avatars[2].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 4 * ThemesParams.AvatarOffsetY + ydisplace };
                    this.Avatars[5].StartPosition = new Point() { X = this.Avatars[1].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 5 * ThemesParams.AvatarOffsetY + 2 * ydisplace };
                    this.Avatars[6].StartPosition = new Point() { X = this.Avatars[0].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 6 * ThemesParams.AvatarOffsetY + 2 * ydisplace };
                    this.Avatars[7].StartPosition = new Point() { X = this.Avatars[1].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 7 * ThemesParams.AvatarOffsetY + 2 * ydisplace };
                    this.Avatars[8].StartPosition = new Point() { X = this.Avatars[2].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 8 * ThemesParams.AvatarOffsetY + 3 * ydisplace };
                    this.Avatars[9].StartPosition = new Point() { X = this.Avatars[3].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 9 * ThemesParams.AvatarOffsetY + 3 * ydisplace };
                    this.Avatars[10].StartPosition = new Point() { X = this.Avatars[2].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 10 * ThemesParams.AvatarOffsetY + 3 * ydisplace };
                    this.Avatars[11].StartPosition = new Point() { X = this.Avatars[1].StartPosition.X, Y = this.Avatars[0].StartPosition.Y - 11 * ThemesParams.AvatarOffsetY + 4 * ydisplace };

                    double topFreeHeight = this.Avatars[11].StartPosition.Y;
                    double topMargin = topFreeHeight * -6 / 100d;
                    double bottomMargin = topFreeHeight * -4 / 100d;
                    double h = topFreeHeight - topMargin - bottomMargin;
                    this.Avatars[12].BlockSize = new Size() { Height = h, Width = (double)this.Avatars[12].RealPictureWidth / (double)this.Avatars[12].RealPictureHeight * h };
                    this.Avatars[12].StartPosition = new Point() { Y = topMargin, X = (ThemesParams.LegendWidth - this.Avatars[12].BlockSize.Width) / 2 };

                    if (this.OnAvatarsInitialized != null)
                    {
                        this.OnAvatarsInitialized(this.Avatars);
                    }
                }
            }
        }
        #endregion

        #region dirty technical details
        private void CalcSizesAndPositionsForSquareScreen()
        {
            ThemesParams.LegendWidth = Math.Round(GlobalGameParams.WindowWidth * ThemesParams.LegendWidthFor1024 / 1024d);
            ThemesParams.AvatarOffsetX = Math.Round(ThemesParams.AvatarOffsetXFor1024 * ThemesParams.LegendWidth / ThemesParams.LegendWidthFor1024 * 1d);
            ThemesParams.AvatarOffsetY = ThemesParams.AvatarOffsetYFor1024 * ThemesParams.LegendWidth / ThemesParams.LegendWidthFor1024 * 1d;

            ThemesParams.AvatarBorderWidth = (ThemesParams.LegendWidth - (ThemesParams.AvatarOffsetX * 4)) / 3d;
            ThemesParams.AvatarBorderHeight = ThemesParams.AvatarBorderWidth * 1.74;
            ThemesParams.LegendHeight = ThemesParams.AvatarOffsetY * 12 + ThemesParams.AvatarBorderHeight + 12;
            ThemesParams.AvatarScale = 1;// SpaceParams.WindowWidth / 1920d;
        }
        private void CalcSizesAndPositionsForOblongScreen()
        {
            ThemesParams.LegendWidth = (int)Math.Round(GlobalGameParams.WindowWidth * ThemesParams.LegendWidthFor1920 / 1920d);
            ThemesParams.AvatarOffsetX = (int)Math.Round(ThemesParams.AvatarOffsetXFor1920 * ThemesParams.LegendWidth / ThemesParams.LegendWidthFor1920 * 1d);
            ThemesParams.AvatarOffsetY = (int)(ThemesParams.AvatarOffsetYFor1920 * GlobalGameParams.WindowWidth / 1920d);

            ThemesParams.AvatarBorderWidth = (ThemesParams.LegendWidth - (ThemesParams.AvatarOffsetX * 5)) / 4d;
            ThemesParams.AvatarBorderHeight = ThemesParams.AvatarBorderWidth * 1.74;
            ThemesParams.LegendHeight = ThemesParams.AvatarOffsetY * 11 + ThemesParams.AvatarBorderHeight + 12;
            ThemesParams.AvatarScale = 1;// SpaceParams.WindowWidth / 1920d;
        }

        #endregion

        #region Dispose functions
        public override void DeleteItself()
        {
            base.DeleteItself();
            foreach (Topic t in this.Topics)
            {
                t.DeleteItself();
            }
            foreach (Avatar a in this.Avatars)
            {
                a.DeleteItself();
            }
        }

        public override void DetachEvents()
        {
            base.DetachEvents();
            this.Avatars.CollectionChanged -= AvatarsSquare_CollectionChanged;
            this.Avatars.CollectionChanged -= AvatarsOblong_CollectionChanged;
        }
        #endregion
    }

    public delegate void AvatarsInitialized(ObservableCollection<Avatar> Avatars);
    public delegate void ThemesGroupSuggestedToBuy(List<Topic> tops);
}
