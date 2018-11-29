using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public partial class Hierarchy : Module
    {
        public event SelectedTopicChanged OnSelectedTopicChanged;

        private Pyramid _pyramidM;
        public Pyramid PyramidM
        {
            get
            {
                if (_pyramidM == null)
                {
                    _pyramidM = new Pyramid();
                }
                return _pyramidM;
            }
        }

        private ObservableCollection<Topic> _topics;
        public ObservableCollection<Topic> Topics
        {
            get
            {
                if (_topics == null)
                {
                    _topics = new ObservableCollection<Topic>();
                }
                return _topics;
            }
        }

        private Topic _selectedTopic;
        public Topic SelectedTopic
        {
            get
            {
                return _selectedTopic;
            }
            set
            {
                _selectedTopic = value;
                if (this.OnSelectedTopicChanged != null)
                {
                    this.OnSelectedTopicChanged(_selectedTopic);
                }
            }
        }

        public static Profile SelectedProfile { get; set; }

        private BaseBlock _topProfilesPanel;
        public BaseBlock TopProfilesPanel
        {
            get
            {
                if (_topProfilesPanel == null)
                {
                    _topProfilesPanel = new BaseBlock();
                }
                return _topProfilesPanel;
            }
            set
            {
                _topProfilesPanel = value;
            }
        }

        public Hierarchy(Profile p) : base()
        {
            Hierarchy.SelectedProfile = p;
            this.NavigationMethod = ModuleNavigationMethod.Shuffle;
            this.OnSelectedTopicChanged += this.PyramidM.Hierarchy_OnSelectedTopicChanged;
        }
        public void InitSizesAndPositions()
        {
            double winDif = (1920 - GlobalGameParams.WindowWidth);
            double deviation = winDif > 0 ? HierarchyParams.TopProfilesWidth1920 * 0.2 : HierarchyParams.TopProfilesWidth1920 * 0.4;
            double winWidthDif = winDif * deviation / HierarchyParams.TopProfilesMaxDif;
            this.TopProfilesPanel.BlockSize = new Size() { Width = winWidthDif > 0 ? HierarchyParams.TopProfilesWidth1920 - winWidthDif : HierarchyParams.TopProfilesWidth1920 + Math.Abs(winWidthDif) } ;
            this.TopProfilesPanel.StartPosition = new Point() { Y = GlobalGameParams.GlobalInfoPanelHeight };
        }

        public void SelectedTopicChangedFromUI(Topic top)
        {
            this.SelectedTopic = top;
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            foreach (Topic t in this.Topics)
            {
                t.DeleteItself();
            }
            this.Topics.Clear();
            this.PyramidM.DeleteItself();
            this.TopProfilesPanel.DeleteItself();
        }
        public override void DetachEvents()
        {
            base.DetachEvents();
            this.OnSelectedTopicChanged -= this.PyramidM.Hierarchy_OnSelectedTopicChanged;
        }
    }

}
