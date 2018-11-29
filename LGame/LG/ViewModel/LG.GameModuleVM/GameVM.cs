using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class GameVM : ModuleVM
    {
        #region model
        private Game GameM
        {
            get
            {
                return (Game)this.ModelM;
            }
        }
        #endregion

        public event ShipExploded OnShipExploded;
        public event PlayGuilefulLaugh OnPlayGuilefulLaugh;
        public event StartPlayingBackMusicAndEnginesVM OnStartPlayingBackMusicAndEnginesVM;
        public event CoveredDistanceVMChanged OnCoveredDistanceVMChanged;

        private readonly GameSoundsVM _gameSoundsVModel;
        public GameSoundsVM GameSoundsVModel
        {
            get
            {
                return _gameSoundsVModel;
            }
        }

        private readonly SpaceVM _spaceVModel;
        public SpaceVM SpaceVModel
        {
            get
            {
                return _spaceVModel;
            }
        }

        private readonly InfoBoardVM _infoBoardVModel;
        public InfoBoardVM InfoBoardVModel
        {
            get
            {
                return _infoBoardVModel;
            }
        }

        private readonly WatchVM _watchVModel;
        public WatchVM WatchVModel
        {
            get
            {
                return _watchVModel;
            }
        }

        private readonly SpaceMapVM _spaceMapVModel;
        public SpaceMapVM SpaceMapVModel
        {
            get
            {
                return _spaceMapVModel;
            }

        }

        private double _coveredDistanceVM;
        public double CoveredDistanceVM
        {
            get
            {
                return _coveredDistanceVM;
            }
            set
            {
                _coveredDistanceVM = value;
                if (this.OnCoveredDistanceVMChanged != null)
                {
                    this.OnCoveredDistanceVMChanged(_coveredDistanceVM);
                }
                this.NotifyPropertyChanged("CoveredDistanceVM");
            }
        }

        public GameVM(Module mod) : base(mod)
        {
            _gameSoundsVModel = new GameSoundsVM(this.GameM.Sounds);
            _spaceVModel = new SpaceVM(this.GameM.SpaceController.OuterSpace);

            _infoBoardVModel = new InfoBoardIntroVM(this.GameM.SpaceController);
            _watchVModel = new WatchVM();
            _spaceMapVModel = new SpaceMapVM(this.GameM.SpaceController.SpaceMapM);
            this.GameM.SpaceController.OnShipExploded += SpaceController_OnShipExploded;
            this.GameM.OnPlayGuilefulLaugh += GameM_OnPlayGuilefulLaugh;
            this.GameM.MainWatch.OnTimePassed += MainWatch_OnTimePassed;
            this.GameM.OnStartPlayingBackMusinAndEngines += GameM_OnStartPlayingBackMusicAndEngines;
            this.GameM.OnCoveredDistanceChanged += GameM_OnCoveredDistanceChanged;
        }

        private void GameM_OnStartPlayingBackMusicAndEngines()
        {
            if (this.OnStartPlayingBackMusicAndEnginesVM != null)
            {
                this.OnStartPlayingBackMusicAndEnginesVM();
            }
        }

        private void GameM_OnPlayGuilefulLaugh()
        {
            if (this.OnPlayGuilefulLaugh != null)
            {
                this.OnPlayGuilefulLaugh();
            }
        }

        private void SpaceController_OnShipExploded()
        {
            if (OnShipExploded != null)
            {
                OnShipExploded();
            }
        }
        private void GameM_OnCoveredDistanceChanged(double dist)
        {
            this.CoveredDistanceVM = dist;
        }

        private void MainWatch_OnTimePassed(Watch w)
        {
            this.WatchVModel.Minutes = w.Minutes.ToString("D2");
            this.WatchVModel.Seconds = w.Seconds.ToString("D2");
            this.WatchVModel.MilliSeconds = w.MilliSeconds.ToString("D3");

            //        Console.WriteLine(dblValue.ToString("N1",
            //              CultureInfo.CreateSpecificCulture("sv-SE")));
            //        String.Format("{0:n0}", yourNumber)
            //.Replace(NumberFormatInfo.CurrentInfo.NumberGroupSeparator, " ");

        }

        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            (m as Game).SpaceController.OnShipExploded -= SpaceController_OnShipExploded;
            (m as Game).OnPlayGuilefulLaugh -= GameM_OnPlayGuilefulLaugh;
            (m as Game).MainWatch.OnTimePassed -= MainWatch_OnTimePassed;
            (m as Game).OnStartPlayingBackMusinAndEngines -= GameM_OnStartPlayingBackMusicAndEngines;
        }
    }
    public delegate void ShipExploded();
    public delegate void PlayGuilefulLaugh();
    public delegate void StartPlayingBackMusicAndEnginesVM();
    public delegate void CoveredDistanceVMChanged(double dist);
}
