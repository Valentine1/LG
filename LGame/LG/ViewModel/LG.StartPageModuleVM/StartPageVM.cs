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

namespace LG.ViewModels
{
    public class StartPageVM : ModuleVM
    {
        private ProfilesManagerVM _profilesMgrVM;
        public ProfilesManagerVM ProfilesMgrVM
        {
            get
            {
                return _profilesMgrVM;
            }
        }

        private StartPage StartPageM
        {
            get
            {
                return (StartPage)this.ModelM;
            }
        }

        private BaseBlockVM _mainMenuPanelVM;
        public BaseBlockVM MainMenuPanelVM
        {
            get
            {
                return _mainMenuPanelVM;
            }
            set
            {
                _mainMenuPanelVM = value;
                NotifyPropertyChanged("MainMenuPanelVM");
            }

        }

        public StartPageVM(Module mod):base(mod)
        {
            _profilesMgrVM = new ProfilesManagerVM(this.StartPageM.ProfilesMgr);
            this.MainMenuPanelVM = new BaseBlockVM(StartPageM.MainMenuPanel);
        }

    }

  
}
