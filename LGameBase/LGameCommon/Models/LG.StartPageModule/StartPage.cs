using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public partial class StartPage : Module
    {
        private ProfilesManager _profilesMgr;
        public ProfilesManager ProfilesMgr
        {
            get
            {
                if (_profilesMgr == null)
                {
                    _profilesMgr = new ProfilesManager();
                }
                return _profilesMgr;
            }
        }

        private BaseBlock _mainMenuPanel;
        public BaseBlock MainMenuPanel
        {
            get
            {
                if (_mainMenuPanel == null)
                {
                    _mainMenuPanel = new BaseBlock();
                }
                return _mainMenuPanel;
            }
        }

        public StartPage()
        {
         
        }


        public void InitSizesAndPositions()
        {
            float w = GlobalGameParams.WindowWidth * StartPageParams.MainMenuWidth1920 / 1920f;
            this.MainMenuPanel.BlockSize = new Size() { Width = w, Height = w * StartPageParams.MainMenuHeight1920 / StartPageParams.MainMenuWidth1920 };
            this.MainMenuPanel.StartPosition = new Point() { X = (GlobalGameParams.WindowWidth - this.MainMenuPanel.BlockSize.Width) / 2d, Y = (GlobalGameParams.WindowHeight - this.MainMenuPanel.BlockSize.Height) / 2d };
        }

        public void ReInitSizesAndPositions()
        {
            this.ProfilesMgr.ReInitSizesAndPositions();
            this.InitSizesAndPositions();
        }

        private void ProfilesMgr_OnProgressShowStart(string mes)
        {
            this.StartProgressBar(mes);
        }

        private void ProfilesMgr_OnProgressShowEnd()
        {
            this.EndProgressBar();
        }
    }

    public delegate void SelectedProfileChanged(Profile p);
}
