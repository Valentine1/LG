using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public partial class Game : Module
    {
        public event CoveredDistanceChanged OnCoveredDistanceChanged;
        public event PlayGuilefulLaugh OnPlayGuilefulLaugh;
        public event StartPlayingBackMusinAndEngines OnStartPlayingBackMusinAndEngines;
        public event SelectedTopicChanged OnSelectedTopicChanged;
        public event MovedToAnotherLevel OnMovedToAnotherLevel;

        private SpaceControllerBase _spaceController;
        public SpaceControllerBase SpaceController
        {
            get
            {
                return _spaceController;
            }
        }

        private Watch _mainWatch;
        public Watch MainWatch
        {
            get
            {
                if (_mainWatch == null)
                {
                    _mainWatch = new Watch();
                }
                return _mainWatch;
            }
        }

        private bool IsGameRunning { get; set; }
        private bool IsGameEnding { get; set; }

        public static Topic GameTopic { get; set; }
        public static Profile SelectedProfile { get; set; }

        internal static double CoveredDistancePercantage;

        private double _coveredDistance;
        public double CoveredDistance
        {
            get
            {
                return _coveredDistance;
            }
            set
            {
                _coveredDistance = value;
                if (this.OnCoveredDistanceChanged != null)
                {
                    this.OnCoveredDistanceChanged(_coveredDistance);
                }
            }
        }

        public Game(Topic top, Profile p)
        {
            Game.GameTopic = top;
            Game.SelectedProfile = p;
            SpaceParams.ResetValues();
            this.IsGameRunning = false;
            this.NavigationMethod = ModuleNavigationMethod.Shuffle;
            _spaceController = new SpaceControllerBase();
            _spaceController.OnIdleProgazovkaEnd += SpaceController_OnIdleProgazovkaEnd;
        }

        public void BackModule_OnAvatarShowEnd()
        {
            this.SpaceController.BeginIdleProgazovka();
        }
        private void SpaceController_OnIdleProgazovkaEnd()
        {
            this.IsGameRunning = true;
            this.MainWatch.Start();
            if (this.OnStartPlayingBackMusinAndEngines != null)
            {
                this.OnStartPlayingBackMusinAndEngines();
            }
        }

        public override void TimeElapses(int roundedDeltaTime, double deltaTime)
        {
            if (this.IsGameRunning)
            {
                this.SpaceController.TimeElapses(roundedDeltaTime, deltaTime);
                if (!this.IsGameEnding)
                {
                    this.CoveredDistance = this.CoveredDistance + deltaTime * SpaceParams.BlockSpeed;
                }
                Game.CoveredDistancePercantage = this.CoveredDistance * 100 / SpaceParams.BigWayDistance;
                //if (!this.IsGameEnding)
                //{
                //    this.IsGameEnding = true;
                //    GameEnding();
                //}
                if (!this.IsGameEnding && this.CoveredDistance >= SpaceParams.BigWayDistance)
                {
                    this.IsGameEnding = true;
                    this.GameEnding();
                }
            }
        }

        async internal void GameEnding()
        {
            this.MainWatch.Stop();
            this.SpaceController.OuterSpace.OnPictureBoxesRunOut += OuterSpace_OnPictureBoxesRunOut;
            this.SpaceController.StopSpace();
            bool? moveSuc = await Game.GameTopic.TryMoveToNextLevel(Game.SelectedProfile, this.MainWatch);
            if (moveSuc != null && (bool)moveSuc)
            {
                this.SpaceController.OuterSpace.StartSalute();
                if (this.OnMovedToAnotherLevel != null)
                {
                    this.OnMovedToAnotherLevel(Game.GameTopic.HierarchyLevel);
                }
                Game.GameTopic = null;
            }
            else
            {
                if (this.OnMovedToAnotherLevel != null)
                {
                    this.OnMovedToAnotherLevel(Game.GameTopic.HierarchyLevel);
                }
                if (this.OnPlayGuilefulLaugh != null)
                {
                    this.OnPlayGuilefulLaugh();
                }
                this.SpaceController.ShootRoundAtShip();
            }
        }

        private void OuterSpace_OnPictureBoxesRunOut()
        {
            this.GameEnded();
        }
        private void GameEnded()
        {
            // this.IsGameRunning = false;
            this.SpaceController.DisableShip();
        }
        public override void DeleteItself()
        {
            base.DeleteItself();
            this.SpaceController.OuterSpace.OnPictureBoxesRunOut -= OuterSpace_OnPictureBoxesRunOut;
            this.SpaceController.OuterSpace.SmallSalutesGrid.StopSalute();
            this.SpaceController.DeleteItself();
            this.MainWatch.Stop();
            this.SpaceController.StopSpace();
            this.Sounds.DeleteItself();
        }
        public override void DetachEvents()
        {
            this.SpaceController.OuterSpace.OnPictureBoxesRunOut -= OuterSpace_OnPictureBoxesRunOut;
            _spaceController.OnIdleProgazovkaEnd -= SpaceController_OnIdleProgazovkaEnd;
            this.SpaceController.Board.DetachEvents();
        }

    }
    public delegate void CoveredDistanceChanged(double dist);
    public delegate void PlayGuilefulLaugh();
    public delegate void StartPlayingBackMusinAndEngines();
    public delegate void MovedToAnotherLevel(Avatar ava);
}
public enum GameСomplexity { Intro, Basic, Pro, Adept }


