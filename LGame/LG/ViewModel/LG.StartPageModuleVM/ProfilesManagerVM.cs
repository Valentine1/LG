using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;
using LG.ViewModels.Commands;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage.Streams;
using LG.Data;

namespace LG.ViewModels
{
    public class ProfilesManagerVM : BaseNotify
    {
        private ProfilesManager ProfileMgr { get; set; }

        private ObservableCollection<ProfileVM> profileVMs;
        public ObservableCollection<ProfileVM> ProfileVMs
        {
            get
            {
                if (profileVMs == null)
                {
                    profileVMs = new ObservableCollection<ProfileVM>();
                }
                return profileVMs;
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
                if (_selectedProfileVM == null)
                {
                    return;
                }
                if (_selectedProfileVM.FlowDirectionVM == FlowDirection.FromVMtoView)
                {
                    _selectedProfileVM.FlowDirectionVM = FlowDirection.FromViewToVM;
                    NotifyPropertyChanged("SelectedProfileVM");
                }
                else
                {
                    _selectedProfileVM.FlowDirectionVM = FlowDirection.FromVMtoView;
                    foreach (Profile p in ProfileMgr.Profiles)
                    {
                        if (_selectedProfileVM.ID == p.ID)
                        {
                            ProfileMgr.SelectedProfileChangedFromUI(p);
                            break;
                        }
                    }
                }
            }
        }

        private ProfileVM _newProfileVM;
        public ProfileVM NewProfileVM
        {
            get
            {
                return _newProfileVM;
            }
            set
            {
                _newProfileVM = value;
                NotifyPropertyChanged("NewProfileVM");
            }
        }

        private ProfileVM _editedProfileVM;
        public ProfileVM EditedProfileVM
        {
            get
            {
                return _editedProfileVM;
            }
            set
            {
                _editedProfileVM = value;
                NotifyPropertyChanged("EditedProfileVM");
            }
        }

        private BaseBlockVM _profilesPanelVM;
        public BaseBlockVM ProfilesPanelVM
        {
            get
            {
                return _profilesPanelVM;
            }
            set
            {
                _profilesPanelVM = value;
                NotifyPropertyChanged("ProfilesPanelVM");
            }
        }

        public ProfilesManagerVM(ProfilesManager profMgr)
        {
            this.ProfileMgr = profMgr;
            this.ProfileMgr.Profiles.CollectionChanged += Profiles_CollectionChanged;
            this.ProfileMgr.OnSelectedProfileChanged += StartPageM_OnSelectedProfileChanged;
            this.ProfilesPanelVM = new BaseBlockVM(profMgr.ProfilesPanel);
        }

        private void Profiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ProfileVM pvm = new ProfileVM((Profile)e.NewItems[0]);
                this.ProfileVMs.Add(pvm);
                pvm.FlowDirectionVM = FlowDirection.FromViewToVM;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Profile p = (Profile)e.OldItems[0];
                ProfileVM pvmf = null;
                foreach (ProfileVM pvm in ProfileVMs)
                {
                    if (pvm.ID == p.ID)
                    {
                        pvmf = pvm;
                        break;
                    }
                }
                ProfileVMs.Remove(pvmf);

            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                this.ProfileVMs.Clear();
            }
        }

        private void StartPageM_OnSelectedProfileChanged(Profile p)
        {
            if (p == null)
            {
                this.SelectedProfileVM = null;
                return;
            }
            foreach (ProfileVM pvm in this.ProfileVMs)
            {
                if (pvm.ID == p.ID)
                {
                    this.SelectedProfileVM = pvm;
                    break;
                }
            }
        }

        public void StartingNewProfileRegister(RegisterMode mode)
        {
            switch (mode)
            {
                case RegisterMode.Local:
                    this.ProfileMgr.StartLocalRegisterFromUI();
                    break;
                case RegisterMode.Internet:
                    this.ProfileMgr.StartOnInternetRegisterFromUI();
                    break;
            }
            _newProfileVM = new ProfileVM() { ProfileRegisterMode = mode };
        }

        public void StartingProfileEdit(RegisterMode mode)
        {
            switch (mode)
            {
                case RegisterMode.Local:
                    this.ProfileMgr.StartLocalRegisterFromUI();
                    break;
                case RegisterMode.Internet:
                    this.ProfileMgr.StartOnInternetRegisterFromUI();
                    break;
            }
            _editedProfileVM = new ProfileVM()
            {
                ProfileRegisterMode = mode,
                ID = SelectedProfileVM.ID,
                Name = SelectedProfileVM.Name,
                Password = SelectedProfileVM.Password,
                ContactInfo = SelectedProfileVM.ContactInfo,
                PictureSource = SelectedProfileVM.PictureSource,
                PictureStream = SelectedProfileVM.PictureStream,
                HasPhoto = SelectedProfileVM.HasPhoto
            };
        }

        public void Register()
        {
            Profile p = null;
            switch (NewProfileVM.ProfileRegisterMode)
            {
                case RegisterMode.Local:
                    p = new Profile()
                    {
                        Name = NewProfileVM.Name,
                        PictureSource = NewProfileVM.PictureSource,
                        PictureStream = NewProfileVM.PictureStream,
                        Type = ProfileType.Local
                    };
                    break;
                case RegisterMode.Internet:
                    p = new Profile()
                    {
                        Name = NewProfileVM.Name,
                        Password = NewProfileVM.Password,
                        ContactInfo = NewProfileVM.ContactInfo,
                        PictureStream = NewProfileVM.PictureStream,
                        PictureSource = NewProfileVM.PictureSource,
                        Type = ProfileType.Internet
                    };
                    break;
            }

            ProfileMgr.RegisterProfile(p);
        }

        public void Update()
        {
            Profile p = null;
            switch (this.EditedProfileVM.ProfileRegisterMode)
            {
                case RegisterMode.Local:
                    p = new Profile()
                    {
                        ID = EditedProfileVM.ID,
                        Name = EditedProfileVM.Name,
                        PictureSource = EditedProfileVM.PictureSource,
                        PictureStream = EditedProfileVM.PictureStream,
                        Type = ProfileType.Local,
                        HasPhoto = EditedProfileVM.HasPhoto
                    };
                    break;
                case RegisterMode.Internet:
                    p = new Profile()
                    {
                        ID = EditedProfileVM.ID,
                        Name = EditedProfileVM.Name,
                        Password = EditedProfileVM.Password,
                        ContactInfo = EditedProfileVM.ContactInfo,
                        PictureStream = EditedProfileVM.PictureStream,
                        PictureSource = EditedProfileVM.PictureSource,
                        Type = ProfileType.Internet,
                        HasPhoto = EditedProfileVM.HasPhoto
                    };
                    break;
            }
            this.ProfileMgr.UpdateProfile(p);
        }
        public void DeleteProfile()
        {
            this.ProfileMgr.DeleteProfile();
        }
        public void CloseRegister()
        {
            this.ProfileMgr.CloseRegister();
        }
    }


}
