using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using LG.Models;

namespace LG.ViewModels
{
    public class PyramidStepVM : BaseBlockVM
    {
        public event UserProfileChanged OnUserProfileChanged;

        public AvatarVM AvatarVModel { get; set; }
        public BaseBlockVM StageVM { get; set; }
        public BaseBlockVM AvatarNameBlockVM { get; set; }
        private ProfileVM _userProfileVM;
        public ProfileVM UserProfileVM
        {
            get
            {
                return _userProfileVM;
            }
            set
            {
                _userProfileVM = value;
                NotifyPropertyChanged("UserProfileVM");
                if (this.OnUserProfileChanged != null)
                {
                    this.OnUserProfileChanged(_userProfileVM);
                }
            }
        }

        private int _stepPlayersNumberVM;
        public int StepPlayersNumberVM
        {
            get
            {
                return _stepPlayersNumberVM;
            }
            set
            {
                _stepPlayersNumberVM = value;
                this.NotifyPropertyChanged("StepPlayersNumberVM");
            }
        }

        private ObservableCollection<ProfileVM> _topProfiles;
        public ObservableCollection<ProfileVM> TopProfilesVMs
        {
            get
            {
                if (_topProfiles == null)
                {
                    _topProfiles = new ObservableCollection<ProfileVM>();
                }
                return _topProfiles;
            }
        }

        private ProfileVM _selectedTopProfileVM;
        public ProfileVM SelectedTopProfileVM
        {
            get
            {
                return _selectedTopProfileVM;
            }
            set
            {
                _selectedTopProfileVM = value;
                NotifyPropertyChanged("SelectedTopProfileVM");
            }
        }

        private bool _isGetProfileEnabledVM;
        public bool IsGetProfileEnabledVM
        {
            get
            {
                return _isGetProfileEnabledVM;
            }
            set
            {
                _isGetProfileEnabledVM = value;
                this.NotifyPropertyChanged("IsGetProfileEnabledVM");
            }
        }

        public PyramidStepVM(PyramidStep pstep) : base(pstep)
        {
            this.IsGetProfileEnabledVM = pstep.IsGetProfileEnabled;
            this.AvatarVModel = new AvatarVM(pstep.AvatarM);
            this.StageVM = new BaseBlockVM(pstep.StageM);
            this.AvatarNameBlockVM = new BaseBlockVM(pstep.AvatarNameBlock);
            pstep.OnUserProfileChanged += pstep_OnUserProfileChanged;
            pstep.OnSelectedTopProfileChanged += pstep_OnSelectedTopProfileChanged;
            pstep.TopProfiles.CollectionChanged += TopProfiles_CollectionChanged;
            pstep.OnIsGetProfileEnabledChanged += pstep_OnIsGetProfileEnabledChanged;
            pstep.OnStepPlayersNumberChanged += pstep_OnStepPlayersNumberChanged;
        }

        public void GetTopProfiles()
        {
            ((PyramidStep)this.BaseBlockM).GetTopProfiles();
        }
        public void SelectPyramidStepVM()
        {
            ((PyramidStep)this.BaseBlockM).SelectPyramidStep();
        }

        private void TopProfiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Profile p = (Profile)e.NewItems[0];
                ProfileVM pvm = new ProfileVM(p);
                this.TopProfilesVMs.Add(pvm);
            }
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                this.TopProfilesVMs.Clear();
            }
        }

        private void pstep_OnSelectedTopProfileChanged(Profile p)
        {
            this.SelectedTopProfileVM = (from pvm in this.TopProfilesVMs where pvm.ID == p.ID select pvm).SingleOrDefault();
        }
        private void pstep_OnUserProfileChanged(Profile p)
        {
            this.UserProfileVM = p == null ? null : new ProfileVM(p);
        }

        private void pstep_OnIsGetProfileEnabledChanged(bool b)
        {
            this.IsGetProfileEnabledVM = b;
        }

        private void pstep_OnStepPlayersNumberChanged(int no)
        {
            this.StepPlayersNumberVM = no;
        }

        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);

        }

        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            (m as PyramidStep).OnUserProfileChanged -= pstep_OnUserProfileChanged;
            (m as PyramidStep).TopProfiles.CollectionChanged -= TopProfiles_CollectionChanged;
            (m as PyramidStep).OnSelectedTopProfileChanged -= pstep_OnSelectedTopProfileChanged;
            (m as PyramidStep).OnIsGetProfileEnabledChanged -= pstep_OnIsGetProfileEnabledChanged;
            (m as PyramidStep).OnStepPlayersNumberChanged -= pstep_OnStepPlayersNumberChanged;
        }
    }

    public delegate void UserProfileChanged (ProfileVM pvm);
}
