using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Common;
using LG.Data;


namespace LG.Models
{
    public partial class Pyramid : Unit
    {
        public event SelectedStepChanged OnSelectedStepChanged;

        private ObservableCollection<PyramidStep> _steps;
        public ObservableCollection<PyramidStep> Steps
        {
            get
            {
                if (_steps == null)
                {
                    _steps = new ObservableCollection<PyramidStep>();
                }
                return _steps;
            }
        }

        private PyramidStep _selectedStep;
        public PyramidStep SelectedStep
        {
            get
            {
                return _selectedStep;
            }
            set
            {
                _selectedStep = value;
                if (this.OnSelectedStepChanged != null)
                {
                    this.OnSelectedStepChanged(_selectedStep);
                }
            }
        }

        private Topic SelectedTopic { get; set; }

        private object UserPyramidSetLock = new object();
        private bool IsSelectedTopicSet { get; set; }
        private bool IsHierarchyInitialized { get; set; }
        private bool IsUserPositionInPyramidSet { get; set; }
        private object SelectedStepSetLock = new object();

        public Pyramid()
        {

        }

        public void Initialize(List<Level> levels)
        {
            this.Steps.CollectionChanged += Steps_CollectionChanged;
            this.InitLevels(levels);
        }

        public void Hierarchy_OnSelectedTopicChanged(Topic topic)
        {
            this.SelectedTopic = topic;
            foreach (PyramidStep step in this.Steps)
            {
                step.TopProfiles.Clear();
            }
            lock (this.SelectedStepSetLock)
            {
                if (this.IsHierarchyInitialized)
                {
                    this.SelectTopStep(); 
                }
            }
            this.IsSelectedTopicSet = true;
            this.SetCurrentUserPositionInPyramid();
        }

        private void Steps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ((PyramidStep)e.NewItems[0]).OnPyramidStepSelected += PyramidStep_OnPyramidStepSelected;
                ((PyramidStep)e.NewItems[0]).OnTopProfilesShouldGet += PyramidStep_OnTopProfilesShouldGet;
            }
            if (this.Steps.Count == 13)
            {
                this.ConstructPyramid();
                lock (this.SelectedStepSetLock)
                {
                    if (this.IsSelectedTopicSet)
                    {
                        this.SelectTopStep();
                    }
                }
                this.IsHierarchyInitialized = true;
                this.SetCurrentUserPositionInPyramid();
            }
        }

        private void PyramidStep_OnTopProfilesShouldGet(PyramidStep step)
        {
            if (step.AvatarM.LevelNo < 12)
            {
                step.InitializeTopProfiles(this.SelectedTopic, 6);
            }
            else if (step.AvatarM.LevelNo == 12)
            {
                step.InitializeTopProfiles(this.SelectedTopic, 7);
            }
            if (this.SelectedStep != step)
            {
                this.SelectedStep = step;
            }
        }

        private void PyramidStep_OnPyramidStepSelected(PyramidStep step)
        {
            if (this.SelectedStep != step)
            {
                this.SelectedStep = step;
            }
        }

       async private Task SelectTopStep()
        {
            this.Steps[11].TopProfiles.Clear();
            this.Steps[11].SelectedTopProfile = null;
            await this.Steps[11].InitializeTopProfiles(this.SelectedTopic, 7);

            this.Steps[12].TopProfiles.Clear();
            if (this.Steps[11].FirstProfileDAL != null)
            {
                this.Steps[12].TopProfiles.Add(new Profile(this.Steps[11].FirstProfileDAL, false));
                this.Steps[12].SelectedTopProfile = this.Steps[12].TopProfiles[0];
                this.SelectedStep = this.Steps[12];
            }
            else
            {
                this.SelectedStep = this.Steps[11];
            }
        }

        private void ConstructPyramid()
        {
            for (int i = 0; i < 12; i++)
            {
                this.Steps[i].StartPosition = new Point() { X = HierarchyParams.PyramidStepWidth * i, Y = GlobalGameParams.WindowHeight - HierarchyParams.PyramidStepHeight * (i + 1) };
                this.Steps[i].BlockSize = new Size() { Width = GlobalGameParams.WindowWidth - (HierarchyParams.PyramidStepWidth * i), Height = HierarchyParams.PyramidStepHeight };

                this.Steps[i].StageM.StartPosition = new Point() { X = HierarchyParams.PyramidStepWidth, Y = 0 };
                this.Steps[i].StageM.BlockSize = new Size() { Width = GlobalGameParams.WindowWidth - (HierarchyParams.PyramidStepWidth * (i + 1)), Height = HierarchyParams.PyramidStepHeight };

                double imwidth = (double)this.Steps[i].AvatarM.RealPictureWidth * HierarchyParams.PyramidAvatarHeight / (double)this.Steps[i].AvatarM.RealPictureHeight;
                this.Steps[i].AvatarM.StartPosition = new Point() { X = (HierarchyParams.PyramidStepWidth - imwidth) / 2d, Y = (HierarchyParams.PyramidStepHeight - HierarchyParams.PyramidAvatarHeight) - HierarchyParams.PyramidStepWidth * 0.14 };
                this.Steps[i].AvatarM.BlockSize = new Size() { Width = imwidth, Height = HierarchyParams.PyramidAvatarHeight };

                this.Steps[i].AvatarNameBlock.BlockSize = new Size() { Height = HierarchyParams.PyramidStepWidth * 0.14 };
            }

            double topFreeHeight = this.Steps[11].StartPosition.Y;
            double topMargin = topFreeHeight * 0 / 100d;
            double bottomMargin = topFreeHeight * 0 / 100d;
            double h = topFreeHeight - topMargin - bottomMargin;
            this.Steps[12].AvatarM.BlockSize = new Size() { Height = h, Width = (double)this.Steps[12].AvatarM.RealPictureWidth / (double)this.Steps[12].AvatarM.RealPictureHeight * h };
            this.Steps[12].AvatarM.StartPosition = new Point() { Y = topMargin, X = (GlobalGameParams.WindowWidth - this.Steps[11].StageM.BlockSize.Width) + (this.Steps[11].StageM.BlockSize.Width - this.Steps[12].AvatarM.BlockSize.Width) / 2 };
            this.Steps[12].StageM.BlockSize = new Size() { Height = HierarchyParams.PyramidStepHeight };
            this.Steps[12].StageM.StartPosition = new Point() { Y = (h * 240 / (double)this.Steps[12].AvatarM.RealPictureHeight) - HierarchyParams.PyramidStepHeight };
        }

        private void SetCurrentUserPositionInPyramid()
        {
            lock (this.UserPyramidSetLock)
            {
                if (this.IsSelectedTopicSet && this.IsHierarchyInitialized)
                {
                    if (this.Steps[12].SelectedTopProfile != null && this.Steps[12].SelectedTopProfile.ID == Hierarchy.SelectedProfile.ID)
                    {
                        foreach (PyramidStep ps in this.Steps)
                        {
                            ps.UserProfile = null;
                        }
                    }
                    else
                    {
                        foreach (PyramidStep ps in this.Steps)
                        {
                            ps.UserProfile = null;
                            if (ps.AvatarM.LevelNo == this.SelectedTopic.HierarchyLevel.LevelNo)
                            {
                                ps.UserProfile = Hierarchy.SelectedProfile;
                                this.IsUserPositionInPyramidSet = true;
                            }
                        }
                    }
                }
            }
        }

        #region dispose functions
        public override void DeleteItself()
        {
            base.DeleteItself();
            foreach (PyramidStep ps in this.Steps)
            {
                ps.DeleteItself();
            }
            Steps.Clear();
        }

        public override void DetachEvents()
        {
            base.DetachEvents();
            foreach (PyramidStep step in this.Steps)
            {
                step.OnTopProfilesShouldGet -= PyramidStep_OnTopProfilesShouldGet;
                step.OnPyramidStepSelected -= PyramidStep_OnPyramidStepSelected;
            }
            this.Steps.CollectionChanged -= Steps_CollectionChanged;
        }
        #endregion
    }

    public delegate void SelectedStepChanged(PyramidStep ps);
    public delegate void IsGetProfileEnabledChanged(bool b);
}

