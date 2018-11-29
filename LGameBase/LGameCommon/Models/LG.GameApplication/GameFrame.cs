using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Windows.UI.Popups;
using LG.Models;
using LG.Common;
using LG.Logging;
using LG.Data;
using LG.Models.Licenses;

namespace LG.Models
{
    public class GameFrame
    {
        private event CurrentModuleChanged OnCurrentModuleChanged;
        public event SelectedProfileChanged OnSelectedProfileChanged;
        public event SelectedTopicChanged OnSelectedTopicChanged;
        public event MessageShow OnMessageShow;
        public event ProgressShowStart OnProgressShowStart;
        public event ProgressShowEnd OnProgressShowEnd;
        public event LevelColorChanged OnLevelColorChanged;

        private Module _currentModule;
        private Module CurrentModule
        {
            get
            {
                return _currentModule;
            }
            set
            {
                _currentModule = value;
                if (this.OnCurrentModuleChanged != null)
                {
                    this.OnCurrentModuleChanged();
                }
            }
        }

        private ObservableCollection<Module> _backgroundModules;
        public ObservableCollection<Module> BackgroundModules
        {
            get
            {
                if (_backgroundModules == null)
                {
                    _backgroundModules = new ObservableCollection<Module>();
                }
                return _backgroundModules;
            }
        }

        private ObservableCollection<Module> _childModules;
        public ObservableCollection<Module> ChildModules
        {
            get
            {
                if (_childModules == null)
                {
                    _childModules = new ObservableCollection<Module>();
                }
                return _childModules;
            }
        }

        private GameBackground BackModule { get; set; }

        private AppSounds _sounds;
        public AppSounds Sounds
        {
            get
            {
                if (_sounds == null)
                {
                    _sounds = new AppSounds();
                }
                return _sounds;
            }
        }

        private Topic SelectedTopic { get; set; }

        private Profile _selectedProfile;
        private Profile SelectedProfile
        {
            get
            {
                return _selectedProfile;
            }
            set
            {
                _selectedProfile = value;
            }
        }

        private BaseBlock _globalInfoPanel;
        public BaseBlock GlobalInfoPanel
        {
            get
            {
                if (_globalInfoPanel == null)
                {
                    _globalInfoPanel = new BaseBlock();
                }
                return _globalInfoPanel;
            }
        }

        private ModuleType NavigatedModuleType = 0;

        public void PreInitialize(int winWidth, int winHeight, ScreenOrientation orient)
        {
            this.ReInitSizesAndPositions(new IntSize(winWidth, winHeight), orient);
            this.OnCurrentModuleChanged += GameFrame_OnCurrentModuleChanged;
        }
        async public void Initialize()
        {
            try
            {
                DataController dc = new DataController();
                if (this.OnProgressShowStart != null)
                {
                    this.OnProgressShowStart(GlobalGameParams.Messages.Checking_Data_Integrity);
                }
                await dc.CheckDataIntegrity();
                await dc.CheckResourcesIntegrity();

                if (this.OnProgressShowEnd != null)
                {
                    this.OnProgressShowEnd();
                }
                this.Sounds.Initialize();
                StartPage startModule = new StartPage();
                this.ChildModules.Add(startModule);
                startModule.ProfilesMgr.OnSelectedProfileChanged += ProfilesMgr_OnSelectedProfileChanged;
                await startModule.Initialize();

                AttachHandlersToModule(startModule);
                this.CurrentModule = startModule;

                this.BackModule = new GameBackground();
                this.BackModule.Initialize();
                this.BackgroundModules.Add(BackModule);
                this.BackModule.PostInitialize();
                this.BackModule.SwitchBigSpiralOn();
            }
            catch (Exception ex)
            {

                Module.GetThemeLoader().ReportErrorToServer(ex.Message + "   " + ex.StackTrace);
                if (ex is UnauthorizedAccessException)
                {
                    string exs = ex.Message + "\n\r Hello! If you are seeing this then you are a tester at the content complience stage. Read, please, ATTENTIVELY because problem seems to be at your side. \n\r " +
                    "Application is just creating some data in its Local Storage place which it will use later and this is perfectly legal to do " +
                           "but this exception means that smth is wrong at your pc, namely app is denied to access its Local Storage. Do next things:\n\r " +
                           "1.Go to C:\\Users\\{UserName}\\AppData\\Local\\Packages\\701LearnAndGame.LGForeignSunriseEnglish_5kae84v54v3ne\\LocalState\n\r " +
                           "2. If you are unable to detect 701LearnAndGame.LGForeignSunriseEnglish_5kae84v54v3n folder go to your admin and explain that your pc does not function properly, it should have been created  " +
                       "during installation. Else if you detect it preceed further\n\r " +
                       "3. Right click on LocalState folder. select Properties, go to Security tab. In the list of users you should detect one like this \n\r" +
                       " Account Unknown(S-1-15-2-2243258159-3108872155-1935386791-2980838001-1890539578-2316982129-2089292232) \n\r" +
                        "this user should have Permissions Full Control checked. If you either do not see user like that one ( Account Unknown(S-1-15-2-...)) or it lacks permissions " +
                       " go to your admin and explain that process of installation of the Windows Store app goes wrong at your pc and ask him to fix that and try to launch this application once more.\n\r" +
                       " Else if you detect such user and it has permissions open folder LocalState and look if there exist  any files and folders and if they have permissions for Full Control for the same user./n/r"
                       ;
                    Exception e = new Exception(exs);
                    Logger.ShowMessageAndLog(e);
                }
                else
                {
                    Logger.ShowMessageAndLog(ex);
                }
            }
        }

        private void ReInitSizesAndPositions(IntSize winSz, ScreenOrientation orient)
        {
            GlobalGameParams.WindowWidth = winSz.Width;
            GlobalGameParams.WindowHeight = winSz.Height;
            GlobalGameParams.Orientation = orient;
            GlobalGameParams.Initialize();
            SpaceParams.Initialize();
            HierarchyParams.Initialize();
            GlobalGameParams.GlobalInfoPanelHeight = SpaceParams.PictureBlockHeight / 2;
            this.GlobalInfoPanel.BlockSize = new Size() { Height = GlobalGameParams.GlobalInfoPanelHeight, Width = GlobalGameParams.GlobalInfoPanelHeight / GlobalGameParams.GlobalInfoPanelAspectRatio };
            this.CheckScreenSizeIsGood();
        }
        public void ScreenOrientationChanged(IntSize winSz, ScreenOrientation orient)
        {
            if (!(this.CurrentModule is StartPage))
            {
                this.NavigateBack();
            }
            this.ReInitSizesAndPositions(winSz, orient);
            (this.CurrentModule as StartPage).ReInitSizesAndPositions();
            this.BackModule.Initialize();
        }
        private void GameFrame_OnCurrentModuleChanged()
        {
            if (this.CurrentModule != null)
            {
                if (this.CurrentModule is Game)
                {
                    this.Sounds.StopBackMusic();
                }
                else
                {
                    this.Sounds.PlayBackMusic();
                }
            }
        }

        public void TimeElapses(int roundedDeltaTime, double deltaTime)
        {
            foreach (Module mod in this.ChildModules)
            {
                mod.TimeElapses(roundedDeltaTime, deltaTime);

            }
            if (this.BackModule != null)
            {
                this.BackModule.TimeElapses(roundedDeltaTime, deltaTime);
            }
            Clock.TimeElapses(deltaTime);
        }

        public void NavigateBack()
        {
            this.BackModule.SwitchBigSpiralOn();
            switch (this.CurrentModule.Type)
            {
                case ModuleType.Themes:
                    if ((this.CurrentModule as Themes).SelectedTopic != null)
                    {
                        this.SelectedTopic = (this.CurrentModule as Themes).SelectedTopic.DeepClone();
                        this.SelectedProfile.SelectedThemeID = (int)(this.CurrentModule as Themes).SelectedTopic.ID;
                    }
                    (this.CurrentModule as Themes).SwitchBigSpiralOff();
                    (this.CurrentModule as Themes).SaveSelectedTopic();
                    break;
                case ModuleType.Game:
                    this.SelectedTopic = Game.GameTopic;
                    this.BackModule.OnAvatarShowEnd -= ((Game)this.CurrentModule).BackModule_OnAvatarShowEnd;
                    break;
            }

            this.DetachHandlersFromModule(this.CurrentModule);
            this.ChildModules.Remove(this.CurrentModule);
            this.CurrentModule.DeleteItself();
            if (this.ChildModules.Count > 0)
            {
                this.CurrentModule = this.ChildModules[this.ChildModules.Count - 1];
                this.NavigatedModuleType = this.CurrentModule.Type;
                if (this.CurrentModule is StartPage)
                {
                    ((StartPage)this.CurrentModule).ProfilesMgr.SelectedProfile.SelectedThemeID = this.SelectedProfile.SelectedThemeID;
                }
            }
        }

        async private void Module_OnNavigateTo(ModuleType modType)
        {
            Module mod = null;
            try
            {
                //if (!LicenseManager.IsAppActive())
                //{
                //    await LicenseManager.BuyApplication();
                //    return;
                //}

                if (this.NavigatedModuleType == modType)
                {
                    return; //Prevents doubled add of modules on quick consequent clicks on StartPage menu items;
                }
                this.NavigatedModuleType = modType;
                if (!this.CheckScreenSizeIsGood())
                {
                    return;
                }
                int prevProfileID = this.SelectedProfile != null ? this.SelectedProfile.ID : -1;
                if (this.CurrentModule is StartPage)
                {
                    this.SelectedProfile = ((StartPage)this.CurrentModule).ProfilesMgr.SelectedProfile.DeepClone();
                }
                if (this.CurrentModule == null)
                {
                    Module.GetThemeLoader().ReportErrorToServer("CurrentModule is null");
                }
                if (this.SelectedProfile == null)
                {
                    Module.GetThemeLoader().ReportErrorToServer("SelectedProfile is null");
                }
                #region create specific module
                switch (modType)
                {
                    case ModuleType.Game:
                        mod = new Game(this.SelectedTopic != null && this.SelectedProfile.ID == prevProfileID ? this.SelectedTopic.DeepClone() : null, this.SelectedProfile.DeepClone());
                        mod.Type = ModuleType.Game;
                        (mod as Game).OnItselfDeleted += GameFrame_OnItselfDeleted;
                        (mod as Game).OnSelectedTopicChanged += GameFrame_OnSelectedTopicChanged;
                        (mod as Game).OnMovedToAnotherLevel += GameFrame_OnMovedToNextLevel;
                        await mod.Initialize();
                        this.ChildModules.Add(mod);
                        this.BackModule.OnAvatarShowEnd += ((Game)mod).BackModule_OnAvatarShowEnd;
                        this.BackModule.BeginAvatarStartShow(Game.GameTopic.HierarchyLevel.DeepClone());
                        mod.PostInitialize();
                        this.BackModule.SwitchBigSpiralOff();
                        Module.GetThemeLoader().ReportErrorToServer("Module_OnNavigateTo Game success");
                        break;
                    case ModuleType.Themes:
                        mod = new Themes(this.SelectedProfile.DeepClone());
                        mod.Type = ModuleType.Themes;
                        mod.OnItselfDeleted += mod_OnItselfDeleted;
                        (mod as Themes).OnSelectedTopicChanged += GameFrame_OnSelectedTopicChanged;
                        this.ChildModules.Add(mod);
                        ((Themes)mod).SwitchBigSpiralOn();
                        this.BackModule.SwitchBigSpiralOff();
                        if (this.OnProgressShowStart != null)
                        {
                            this.OnProgressShowStart(GlobalGameParams.Messages.Checking_New_Themes);
                        }
                        await mod.Initialize();
                        Module.GetThemeLoader().ReportErrorToServer("Module_OnNavigateTo Themes success");
                        break;
                    case ModuleType.Hierarchy:
                        mod = new Hierarchy(this.SelectedProfile.DeepClone());
                        mod.Type = ModuleType.Hierarchy;
                        mod.OnItselfDeleted += mod_OnItselfDeleted;
                        (mod as Hierarchy).OnSelectedTopicChanged += HierarchyModule_OnSelectedTopicChanged;
                        this.ChildModules.Add(mod);
                        mod.Initialize();
                        this.BackModule.SwitchBigSpiralOff();
                        Module.GetThemeLoader().ReportErrorToServer("Module_OnNavigateTo Hierarchy success");
                        break;
                }
                #endregion
            }
            catch (Exception ex)
            {
                Module.GetThemeLoader().ReportErrorToServer(ex.Message + "   " + ex.StackTrace);
                LG.Logging.Logger.ShowMessageAndLog(ex);
            }
            finally
            {
                if (mod != null && mod.Type == ModuleType.Themes)
                {
                    if (this.OnProgressShowEnd != null)
                    {
                        this.OnProgressShowEnd();
                    }
                }
                if (mod != null)
                {
                    this.AttachHandlersToModule(mod);
                    this.CurrentModule = mod;
                }
            }
        }

        private void ProfilesMgr_OnSelectedProfileChanged(Profile p)
        {
            if (p != null && this.OnSelectedProfileChanged != null)
            {
                this.OnSelectedProfileChanged(p);
                this.ChangeSelectedTopic(null);
            }
        }
        private void GameFrame_OnSelectedTopicChanged(Topic topic)
        {
            this.ChangeSelectedTopic(topic);
        }

        private void ChangeSelectedTopic(Topic topic)
        {
            if (this.OnSelectedTopicChanged != null)
            {
                this.OnSelectedTopicChanged(topic == null ? null : topic.DeepClone());
            }
        }

        async private void HierarchyModule_OnSelectedTopicChanged(Topic topic)
        {
            TimeRange t = await Module.Loader.GetBestTime(GlobalGameParams.AppLang, this.SelectedProfile.ID, topic.ID);
            this.SelectedProfile.BestTime = t;
            if (this.OnLevelColorChanged != null)
            {
                this.OnLevelColorChanged(topic.HierarchyLevel.TopBorderClr, topic.HierarchyLevel.BottomBorderClr);
            }
        }
        private void GameFrame_OnMovedToNextLevel(Avatar ava)
        {
            this.BackModule.BeginAvatarFinishShow(Game.GameTopic.HierarchyLevel.DeepClone());
            if (this.OnSelectedTopicChanged != null)
            {
                this.OnSelectedTopicChanged(Game.GameTopic.DeepClone());
            }
        }

        private void GameFrame_OnItselfDeleted(Unit m)
        {
            Game.GameTopic = null;
            m.OnItselfDeleted -= GameFrame_OnItselfDeleted;
            (m as Game).OnSelectedTopicChanged -= GameFrame_OnSelectedTopicChanged;
            (m as Game).OnMovedToAnotherLevel -= GameFrame_OnMovedToNextLevel;
            this.BackModule.OnAvatarShowEnd -= (m as Game).BackModule_OnAvatarShowEnd;
        }
        private void mod_OnItselfDeleted(Unit m)
        {
            m.OnItselfDeleted -= mod_OnItselfDeleted;
            switch ((m as Module).Type)
            {
                case ModuleType.Themes:
                    (m as Themes).OnSelectedTopicChanged -= GameFrame_OnSelectedTopicChanged;
                    break;
                case ModuleType.Hierarchy:
                    (m as Hierarchy).OnSelectedTopicChanged -= HierarchyModule_OnSelectedTopicChanged;
                    break;
            }

        }

        #region utility  functions
        private bool CheckScreenSizeIsGood()
        {
            if ((GlobalGameParams.WindowWidth < 1021 || GlobalGameParams.WindowHeight < GlobalGameParams.WindowHeightMin) || GlobalGameParams.Orientation == ScreenOrientation.Portrait)
            {
                if (this.OnMessageShow != null)
                {
                    this.OnMessageShow(GlobalGameParams.Messages.Screen_IsNot_Good);
                    return false;
                }
            }
            return true;
        }
        private void AttachHandlersToModule(Module mod)
        {
            mod.OnNavigateTo += Module_OnNavigateTo;
            mod.OnProgressShowStart += Module_OnProgressShowStart;
            mod.OnProgressShowEnd += Module_OnProgressShowEnd;
        }
        private void DetachHandlersFromModule(Module mod)
        {
            if (mod != null)
            {
                mod.OnNavigateTo -= Module_OnNavigateTo;
                mod.OnProgressShowStart -= Module_OnProgressShowStart;
                mod.OnProgressShowEnd -= Module_OnProgressShowEnd;
            }
        }

        private void Module_OnProgressShowStart(string mes)
        {
            if (this.OnProgressShowStart != null)
            {
                this.OnProgressShowStart(mes);
            }
        }

        private void Module_OnProgressShowEnd()
        {
            if (this.OnProgressShowEnd != null)
            {
                this.OnProgressShowEnd();
            }
        }

        #endregion
    }

    public delegate void CurrentModuleChanged();
    public delegate void MessageShow(string mes);

    public delegate void LevelColorChanged(Color topClr, Color bottomClr);

}
