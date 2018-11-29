using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.ViewModels;

using LG.Models;

namespace LG.ViewModels
{
    public class HierarchyVM : ModuleVM
    {
        private Hierarchy HierarchyM
        {
            get
            {
                return (Hierarchy)this.ModelM;
            }
        }

        public event SelectedTopicVMChanged OnSelectedTopicVMChanged;

        private ObservableCollection<PyramidStepVM> _steps;
        public ObservableCollection<PyramidStepVM> Steps
        {
            get
            {
                if (_steps == null)
                {
                    _steps = new ObservableCollection<PyramidStepVM>();
                }
                return _steps;
            }
        }

        private ObservableCollection<TopicVM> _topicVMs;
        public ObservableCollection<TopicVM> TopicVMs
        {
            get
            {
                if (_topicVMs == null)
                {
                    _topicVMs = new ObservableCollection<TopicVM>();
                }
                return _topicVMs;
            }
        }

        private PyramidStepVM _selectedStepVM;
        public PyramidStepVM SelectedStepVM
        {
            get
            {
                return _selectedStepVM;
            }
            set
            {
                _selectedStepVM = value;
                this.NotifyPropertyChanged("SelectedStepVM");
            }
        }

        private TopicVM _selectedTopicVM;
        public TopicVM SelectedTopicVM
        {
            get
            {
                return _selectedTopicVM;
            }
            set
            {
                _selectedTopicVM = value;
                _selectedTopicVM.TopicM.DataFlow = Flow.FromVMToModel;
                this.HierarchyM.SelectedTopicChangedFromUI(_selectedTopicVM.TopicM);
            }
        }

        private BaseBlockVM _topProfilesPanelVM;
        public BaseBlockVM TopProfilesPanelVM
        {
            get
            {
                return _topProfilesPanelVM;
            }
            set
            {
                _topProfilesPanelVM = value;
                this.NotifyPropertyChanged("TopProfilesPanelVM");
            }
        }

        public HierarchyVM(Module mod) : base(mod)
        {
            this.HierarchyM.PyramidM.Steps.CollectionChanged += Steps_CollectionChanged;
            this.HierarchyM.PyramidM.OnSelectedStepChanged += PyramidM_OnSelectedStepChanged;
            this.HierarchyM.Topics.CollectionChanged += Topics_CollectionChanged;
            this.HierarchyM.OnSelectedTopicChanged += HierarchyM_OnSelectedTopicChanged;
            this.TopProfilesPanelVM = new BaseBlockVM(this.HierarchyM.TopProfilesPanel);
        }

        private void PyramidM_OnSelectedStepChanged(PyramidStep ps)
        {
            this.SelectedStepVM = (from s in this.Steps where s.AvatarVModel.LevelNo == ps.AvatarM.LevelNo select s).SingleOrDefault();
        }

        private void HierarchyM_OnSelectedTopicChanged(Topic topic)
        {
            if (topic.DataFlow == Flow.FromModelToVM)
            {
                foreach (TopicVM tvm in this.TopicVMs)
                {
                    if (tvm.ID == topic.ID)
                    {
                        _selectedTopicVM = tvm;
                        if (this.OnSelectedTopicVMChanged != null)
                        {
                            this.OnSelectedTopicVMChanged(this._selectedTopicVM);
                        }
                        break;
                    }
                }
            }
        }

        private void Steps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                PyramidStepVM p = new PyramidStepVM((PyramidStep)e.NewItems[0]);
                this.Steps.Add(p);
            }
        }

        private void Topics_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                this.TopicVMs.Add(new TopicVM((Topic)e.NewItems[0]));
            }
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            this.HierarchyM.PyramidM.Steps.CollectionChanged -= Steps_CollectionChanged;
            this.HierarchyM.PyramidM.OnSelectedStepChanged -= PyramidM_OnSelectedStepChanged;
            this.HierarchyM.Topics.CollectionChanged -= Topics_CollectionChanged;
            this.HierarchyM.OnSelectedTopicChanged -= HierarchyM_OnSelectedTopicChanged;
        }
    }

    public delegate void SelectedTopicVMChanged(TopicVM tvm);
}
