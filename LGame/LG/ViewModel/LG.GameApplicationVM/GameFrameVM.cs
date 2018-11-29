using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using LG.Models;
using LG.Common;

namespace LG.ViewModels
{
    public class GameFrameVM : BaseNotify
    {
        #region models
        private GameFrame _gameFrameM;
        private GameFrame GameFrameM
        {
            get
            {
                if (_gameFrameM == null)
                {
                    _gameFrameM = new GameFrame();
                }
                return _gameFrameM;
            }
        }
        #endregion

        public event MessageShowVM OnMessageShowVM;
        public event ProgressShowStartVM OnProgressShowStartVM;
        public event ProgressShowEndVM OnProgressShowEndVM;

        #region view models
        private ObservableCollection<ModuleVM> _backgroundModulesVM;
        public ObservableCollection<ModuleVM> BackgroundModulesVM
        {
            get
            {
                if (_backgroundModulesVM == null)
                {
                    _backgroundModulesVM = new ObservableCollection<ModuleVM>();
                }
                return _backgroundModulesVM;
            }
        }

        private ObservableCollection<ModuleVM> _childModulesVM;
        public ObservableCollection<ModuleVM> ChildModulesVM
        {
            get
            {
                if (_childModulesVM == null)
                {
                    _childModulesVM = new ObservableCollection<ModuleVM>();
                }
                return _childModulesVM;
            }
        }

        private ProfileVM _selectedProfileVM;
        public ProfileVM SelectedProfileVM
        {
            get
            {
                return _selectedProfileVM;
            }
            set
            {
                _selectedProfileVM = value;
                this.NotifyPropertyChanged("SelectedProfileVM");
            }
        }

        private TopicVM _selectedTopicVM;
        public TopicVM SelectedTopicVM
        {
            get
            {
                return _selectedTopicVM;
            }
            set
            {
                _selectedTopicVM = value;
                NotifyPropertyChanged("SelectedTopicVM");
            }
        }

        public AppSoundsVM AppSoundsVModel { get; set; }
        private BaseBlockVM _globalInfoPanelVM;
        public BaseBlockVM GlobalInfoPanelVM
        {
            get
            {
                return _globalInfoPanelVM;
            }
            set
            {
                _globalInfoPanelVM = value;
                NotifyPropertyChanged("GlobalInfoPanelVM");
            }

        }

        private LinearGradientBrush _levelColor;
        public LinearGradientBrush LevelColor
        {
            get
            {
                return _levelColor;
            }
            set
            {
                _levelColor = value;
                this.NotifyPropertyChanged("LevelColor");
            }
        }

        #endregion

        public void PreInitialize(int winWidth, int winHeight, DisplayOrientations orient)
        {
            ScreenOrientation sor = orient == DisplayOrientations.Landscape || orient == DisplayOrientations.LandscapeFlipped ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
            this.GameFrameM.OnMessageShow += GameFrameM_OnMessageShow;
            this.GameFrameM.OnProgressShowStart += GameFrameM_OnProgressShowStart;
            this.GameFrameM.OnProgressShowEnd += GameFrameM_OnProgressShowEnd;
            this.GameFrameM.OnSelectedProfileChanged += GameFrameM_OnSelectedProfileChanged;
            this.GameFrameM.OnSelectedTopicChanged += GameFrameM_OnSelectedTopicChanged;
            this.GameFrameM.PreInitialize(winWidth, winHeight, sor);
            this.GlobalInfoPanelVM = new BaseBlockVM(this.GameFrameM.GlobalInfoPanel);
            this.AppSoundsVModel = new AppSoundsVM(GameFrameM.Sounds);
        }


        private void GameFrameM_OnSelectedTopicChanged(Topic topic)
        {
            this.SelectedTopicVM = topic == null ? null : new TopicVM(topic);
        }

        private void GameFrameM_OnSelectedProfileChanged(Profile p)
        {
            this.SelectedProfileVM = new ProfileVM(p);
        }

        private void GameFrameM_OnMessageShow(string mes)
        {
            if (this.OnMessageShowVM != null)
            {
                this.OnMessageShowVM(mes);
            }
        }
        private void GameFrameM_OnProgressShowStart(string mes)
        {
            if (this.OnProgressShowStartVM != null)
            {
                this.OnProgressShowStartVM(mes);
            }
        }
        private void GameFrameM_OnProgressShowEnd()
        {
            if (this.OnProgressShowEndVM != null)
            {
                this.OnProgressShowEndVM();
            }
        }

        public void Initialize()
        {
            GameFrameM.ChildModules.CollectionChanged += ChildModules_CollectionChanged;
            GameFrameM.BackgroundModules.CollectionChanged += BackgroundModules_CollectionChanged;
            GameFrameM.Initialize();
            this.GameFrameM.OnLevelColorChanged += GameFrameM_OnLevelColorChanged;
        }

        public void NavigateBack()
        {
            this.GameFrameM.NavigateBack();
        }

        public void TimeElapses(int roundedDeltaTime, double deltaTime)
        {
            this.GameFrameM.TimeElapses(roundedDeltaTime, deltaTime);
        }
        public void DisplayOrientationChanged(Windows.Foundation.Size sz, DisplayOrientations orient)
        {
            ScreenOrientation sor = orient == DisplayOrientations.Landscape || orient == DisplayOrientations.LandscapeFlipped ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
            this.GameFrameM.ScreenOrientationChanged(new LG.Common.IntSize((int)sz.Width, (int)sz.Height), sor);
        }

        private void BackgroundModules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Module mod = (Module)e.NewItems[0];
                GameBackgroundVM gbvm = new GameBackgroundVM(mod);
                this.BackgroundModulesVM.Add(gbvm);
            }
        }
        private void ChildModules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Module mod = (Module)e.NewItems[0];
                ModuleVM moduleVM = null;
                if (mod is StartPage)
                {
                    moduleVM = new StartPageVM(mod);
                }
                else if (mod is Themes)
                {
                    moduleVM = new ThemesVM(mod);
                }
                else if (mod is Game)
                {
                    moduleVM = new GameVM(mod);
                }
                else if (mod is Hierarchy)
                {
                    moduleVM = new HierarchyVM(mod);
                }
                moduleVM.OnItselfDeleted += ModuleVM_OnItselfDeleted;
                this.ChildModulesVM.Add(moduleVM);
            }
        }

        private void GameFrameM_OnLevelColorChanged(Color topClr, Color bottomClr)
        {
            LinearGradientBrush LegColor = new LinearGradientBrush();
            LegColor.StartPoint = new Windows.Foundation.Point(0.5, 0);
            LegColor.EndPoint = new Windows.Foundation.Point(0.5, 1);
            LegColor.Opacity = 0.8;
            GradientStop gstop = new GradientStop();
            gstop.Color = Windows.UI.Color.FromArgb(topClr.A, topClr.R, topClr.G, topClr.B);
            GradientStop gsbottom = new GradientStop();
            gsbottom.Color = Windows.UI.Color.FromArgb(bottomClr.A, bottomClr.R, bottomClr.G, bottomClr.B);
            gsbottom.Offset = 1;
            LegColor.GradientStops.Add(gstop);
            LegColor.GradientStops.Add(gsbottom);

            this.LevelColor = LegColor;
        }
        private void ModuleVM_OnItselfDeleted(UnitVM uvm)
        {
            this.ChildModulesVM.Remove((ModuleVM)uvm);
        }
    }

    public delegate void MessageShowVM(string mes);
    public delegate void ProgressShowStartVM(string mes);
    public delegate void ProgressShowEndVM();
}
