using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public class PyramidStep : BaseBlock
    {
        public event UserProfileChanged OnUserProfileChanged;
        public event StepPlayersNumberChanged OnStepPlayersNumberChanged;
        public event SelectedTopProfileChanged OnSelectedTopProfileChanged;
        public event PyramidStepSelected OnPyramidStepSelected;
        public event TopProfilesShouldGet OnTopProfilesShouldGet;

        public event IsGetProfileEnabledChanged OnIsGetProfileEnabledChanged;

        public Avatar AvatarM { get; set; }

        private Stage _stageM;
        public Stage StageM
        {
            get
            {
                if (_stageM == null)
                {
                    _stageM = new Stage();
                }
                return _stageM;
            }
        }

        private BaseBlock _avatarNameBlock;
        public BaseBlock AvatarNameBlock
        {
            get
            {
                if (_avatarNameBlock == null)
                {
                    _avatarNameBlock = new BaseBlock();
                }
                return _avatarNameBlock;
            }
        }

        private int _stepPlayersNumber;
        public int StepPlayersNumber
        {
            get
            {
                return _stepPlayersNumber;
            }
            set
            {
                _stepPlayersNumber = value;
                if (this.OnStepPlayersNumberChanged != null)
                {
                    this.OnStepPlayersNumberChanged(_stepPlayersNumber);
                }
            }
        }

        private ObservableCollection<Profile> _topProfiles;
        public ObservableCollection<Profile> TopProfiles
        {
            get
            {
                if (_topProfiles == null)
                {
                    _topProfiles = new ObservableCollection<Profile>();
                }
                return _topProfiles;
            }
        }

        private Profile _selectedTopProfile;
        public Profile SelectedTopProfile
        {
            get
            {
                return _selectedTopProfile;
            }
            set
            {
                _selectedTopProfile = value;
                if (this.OnSelectedTopProfileChanged != null)
                {
                    this.OnSelectedTopProfileChanged(_selectedTopProfile);
                }
            }
        }

        private Profile _userProfile;
        public Profile UserProfile
        {
            get
            {
                return _userProfile;
            }
            set
            {
                _userProfile = value;
                if (this.OnUserProfileChanged != null)
                {
                    this.OnUserProfileChanged(_userProfile);
                }
            }
        }

        private bool _isGetProfileEnabled;
        public bool IsGetProfileEnabled
        {
            get
            {
                return _isGetProfileEnabled;
            }
            set
            {
                _isGetProfileEnabled = value;
                if (this.OnIsGetProfileEnabledChanged != null)
                {
                    this.OnIsGetProfileEnabledChanged(_isGetProfileEnabled);
                }
            }
        }

        private Timer _getProfileTimer;
        private Timer GetProfilesTimer
        {
            get
            {
                if (_getProfileTimer == null)
                {
                    _getProfileTimer = new Timer(30000);
                    _getProfileTimer.OnTicked += GetProfileTimer_OnTicked;
                }
                return _getProfileTimer;
            }
        }

        internal ProfileDAL FirstProfileDAL { get; set; }

        public PyramidStep()
        {
            this.IsGetProfileEnabled = true;  
        }

       async public Task InitializeTopProfiles(Topic top, int pCount)
        {
            LevelProfiles levPrfs = await Module.GetProfLoader(Hierarchy.SelectedProfile.Type).GetProfiles(GlobalGameParams.AppLang, new Theme() { ID = top.ID, Name = top.TextValue },
                                                                                                            this.AvatarM.LevelNo, 0, pCount);
            if (levPrfs != null)
            {
                this.StepPlayersNumber = levPrfs.TotalLevelPlayers;
                this.TopProfiles.Clear();
                if (this.AvatarM.LevelNo == 12)
                {
                    if (levPrfs.Profiles != null && levPrfs.Profiles.Count > 0)
                    {
                        this.FirstProfileDAL = levPrfs.Profiles[0];
                        levPrfs.Profiles.RemoveAt(0);
                        this.StepPlayersNumber--;
                    }
                    else
                    {
                        this.FirstProfileDAL = null;
                    }
                }
                this.PopulateTopProfiles(levPrfs.Profiles);
            }
        }

        public void GetTopProfiles()
        {
            this.IsGetProfileEnabled = false;
            GetProfilesTimer.Start();
            if (this.OnTopProfilesShouldGet != null)
            {
                this.OnTopProfilesShouldGet(this);
            }
        }
        public void SelectPyramidStep()
        {
            if (this.OnPyramidStepSelected != null)
            {
                this.OnPyramidStepSelected(this);
            }
        }

        private void GetProfileTimer_OnTicked()
        {
            this.IsGetProfileEnabled = true;
            this.GetProfilesTimer.Stop();
        }
        private void PopulateTopProfiles(List<ProfileDAL> profilesD)
        {
            if (profilesD != null)
            {
                foreach (ProfileDAL pd in profilesD)
                {
                    Profile p = new Profile(pd, true);
                    p.BlockSize = new Size() { Height = HierarchyParams.PyramidStepHeight / 2 - 4 };
                    this.TopProfiles.Add(p);
                }
            }
            if (this.TopProfiles.Count > 0)
            {
                this.SelectedTopProfile = this.TopProfiles[0];
            }
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            AvatarM.DeleteItself();
            foreach (Profile p in this.TopProfiles)
            {
                p.DeleteItself();
            }
            this.TopProfiles.Clear();
        }
    }

    public delegate void UserProfileChanged(Profile p);
    public delegate void StepPlayersNumberChanged(int no);
    public delegate void SelectedTopProfileChanged(Profile p);
    public delegate void PyramidStepSelected(PyramidStep step);
    public delegate void TopProfilesShouldGet(PyramidStep step);
}
