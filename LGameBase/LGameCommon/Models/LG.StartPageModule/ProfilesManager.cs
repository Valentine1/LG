using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Windows.UI.Popups;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public partial class ProfilesManager
    {
        public event SelectedProfileChanged OnSelectedProfileChanged;
        public event ProgressShowStart OnProgressShowStart;
        public event ProgressShowEnd OnProgressShowEnd;

        #region DAL
        private DataLoaderFactory _loaderFactory;
        private DataLoaderFactory LoaderFactory
        {
            get
            {
                if (_loaderFactory == null)
                {
                    _loaderFactory = new DataLoaderFactory();
                }
                return _loaderFactory;
            }
        }

        #endregion

        private ObservableCollection<Profile> _profiles;
        public ObservableCollection<Profile> Profiles
        {
            get
            {
                if (_profiles == null)
                {
                    _profiles = new ObservableCollection<Profile>();
                }
                return _profiles;
            }
        }

        private Profile _selectedProfile;
        public Profile SelectedProfile
        {
            get
            {
                return _selectedProfile;
            }
            set
            {
                _selectedProfile = value;
                if (this.OnSelectedProfileChanged != null)
                {
                    this.OnSelectedProfileChanged(_selectedProfile);
                }
            }
        }

        private BaseBlock _profilesPanel;
        public BaseBlock ProfilesPanel
        {
            get
            {
                if (_profilesPanel == null)
                {
                    _profilesPanel = new BaseBlock();
                }
                return _profilesPanel;
            }
        }

        async public void RegisterProfile(Profile p)
        {
            try
            {
                if (this.OnProgressShowStart != null)
                {
                    this.OnProgressShowStart(GlobalGameParams.Messages.Register_New_Account);
                }

                switch (p.Type)
                {
                    case ProfileType.Local:
                        await this.RegisterProfileLocally(p);
                        break;
                    case ProfileType.Internet:
                        await this.RegisterProfileOnServer(p);
                        break;
                }
                await this.RefreshProfiles();
            }
            catch (Exception ex)
            {
                Logging.Logger.ShowMessageAndLog(ex);

            }
            finally
            {
                if (this.OnProgressShowEnd != null)
                {
                    this.OnProgressShowEnd();
                }
            }
        }
        async public void UpdateProfile(Profile p)
        {
            try
            {
                p.PicUrl = this.SelectedProfile.PicUrl;
                if (this.OnProgressShowStart != null)
                {
                    this.OnProgressShowStart(GlobalGameParams.Messages.Update_Account);
                }

                switch (this.SelectedProfile.Type)
                {
                    case ProfileType.Local:
                        await this.UpdateProfileLocally(p);
                        break;
                    case ProfileType.Internet:
                        if (await this.UpdateProfileOnServer(p))
                        {
                            await this.UpdateProfileLocally(p);
                        }
                        break;
                }
                await this.RefreshProfiles();
            }
            catch (Exception ex)
            {
                Logging.Logger.ShowMessageAndLog(ex);
            }
            finally
            {
                if (this.OnProgressShowEnd != null)
                {
                    this.OnProgressShowEnd();
                }
            }
        }

        async public void DeleteProfile()
        {
            try
            {
                if (this.Profiles.Count == 1)
                {
                    return;
                }
                if (this.OnProgressShowStart != null)
                {
                    this.OnProgressShowStart(GlobalGameParams.Messages.Delete_Account);
                }
                switch (this.SelectedProfile.Type)
                {
                    case ProfileType.Local:
                        await this.DeleteSelectedProfileLocally();
                        break;
                    case ProfileType.Internet:
                        if (await this.DeleteSelectedProfileOnServer())
                        {
                            await this.DeleteSelectedProfileLocally();
                        }
                        break;
                }

                await this.RefreshProfiles();
            }
            catch (Exception ex)
            {
                Logging.Logger.ShowMessageAndLog(ex);
            }
            finally
            {
                if (this.OnProgressShowEnd != null)
                {
                    this.OnProgressShowEnd();
                }
            }

        }
        public void ReInitSizesAndPositions()
        {
            float w = GlobalGameParams.WindowWidth * StartPageParams.ProfilesWidth1920 / 1920f;
            this.ProfilesPanel.BlockSize = new Size() { Width = w, Height = this.ProfilesPanel.BlockSize.Height * w / this.ProfilesPanel.BlockSize.Width };
        }

        public void SelectedProfileChangedFromUI(Profile p)
        {
            this.SelectedProfile = p;
            IDataLoader loader = this.LoaderFactory.GetDataLoader(Module.LocalStoaragePath + "\\data");
            loader.SetLastUser(p.ID);
        }

        public void StartLocalRegisterFromUI()
        {
            float w = GlobalGameParams.WindowWidth * StartPageParams.ProfilesWidth1920 / 1920f;
            this.ProfilesPanel.BlockSize = new Size() { Width = w, Height = w * StartPageParams.ProfilesRegisterLocalHeight1920 / StartPageParams.ProfilesWidth1920 };
        }
        public void StartOnInternetRegisterFromUI()
        {
            float w = GlobalGameParams.WindowWidth * StartPageParams.ProfilesWidth1920 / 1920f;
            this.ProfilesPanel.BlockSize = new Size() { Width = w, Height = w * StartPageParams.ProfilesRegisterOnInternetHeight1920 / StartPageParams.ProfilesWidth1920 };
        }
        public void CloseRegister()
        {
            float w = GlobalGameParams.WindowWidth * StartPageParams.ProfilesWidth1920 / 1920f;
            this.ProfilesPanel.BlockSize = new Size() { Width = w, Height = w * StartPageParams.ProfilesHeight1920 / StartPageParams.ProfilesWidth1920 };
        }


    }
}
