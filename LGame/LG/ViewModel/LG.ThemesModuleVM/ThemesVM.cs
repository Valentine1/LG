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
    public class ThemesVM : ModuleVM
    {
        private Themes ThemesM
        {
            get
            {
                return (Themes)this.ModelM;
            }
        }

        #region view properties

        private ObservableCollection<AvatarVM> _avatarVMs;
        public ObservableCollection<AvatarVM> AvatarVMs
        {
            get
            {
                if (_avatarVMs == null)
                {
                    _avatarVMs = new ObservableCollection<AvatarVM>();
                }
                return _avatarVMs;
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

                if (_selectedTopicVM!= null && _selectedTopicVM.FlowDirectionVM == FlowDirection.FromVMtoView)
                {
                    _selectedTopicVM.FlowDirectionVM = FlowDirection.FromViewToVM;
                    NotifyPropertyChanged("SelectedTopicVM");
                }
                else
                {
                    if (_selectedTopicVM != null)
                    {
                        _selectedTopicVM.FlowDirectionVM = FlowDirection.FromVMtoView;
                    }
                    this.ThemesM.SelectedTopicChangedFromUI(_selectedTopicVM == null ? null : _selectedTopicVM.TopicM);  
                }  
            }
        }

        private BaseBlockVM _topicsAreaVM;
        public BaseBlockVM TopicsAreaVM
        {
            get
            {
                return _topicsAreaVM;
            }
            set
            {
                _topicsAreaVM = value;
                this.NotifyPropertyChanged("TopicsAreaVM");
            }
        }

        private double _legendWidth;
        public double LegendWidth
        {
            get
            {
                return _legendWidth;
            }
            set
            {
                _legendWidth = value;
                this.NotifyPropertyChanged("LegendWidth");
            }
        }

        private double _legendHeight;
        public double LegendHeight
        {
            get
            {
                return _legendHeight;
            }
            set
            {
                _legendHeight = value;
                this.NotifyPropertyChanged("LegendHeight");
            }
        }

        private GridLength _rightPanelWidth;
        public GridLength RightPanelWidth
        {
            get
            {
                return _rightPanelWidth;
            }
            set
            {
                _rightPanelWidth = value;
                this.NotifyPropertyChanged("RightPanelWidth");
            }
        }

        private SpiralVM _dNASpiralVModel;
        public SpiralVM DNASpiralVModel
        {
            get
            {
                return _dNASpiralVModel;
            }
        }

        #endregion

        public ThemesVM(Module mod) : base(mod)
        {
            this.ThemesM.OnAvatarsInitialized += ThemesM_OnAvatarsInitialized;
            foreach (Topic top in this.ThemesM.Topics)
            {
                this.TopicVMs.Add(new TopicVM(top));
            }
            this.ThemesM.Topics.CollectionChanged += Topics_CollectionChanged;
            this.ThemesM.OnSelectedTopicChanged += ThemesM_OnSelectedTopicChanged;
            this.ThemesM.OnThemesGroupSuggestedToBuy += ThemesM_OnThemesGroupSuggestedToBuy;
            _dNASpiralVModel = new SpiralVM(this.ThemesM.DNASpiral);
            this._topicsAreaVM = new BaseBlockVM(this.ThemesM.TopicsArea);

        }

        private void Topics_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                TopicVM tvm = new TopicVM((Topic)e.NewItems[0]);
                tvm.FlowDirectionVM = FlowDirection.FromViewToVM;
                this.TopicVMs.Add(tvm);
            }
        }

        private void ThemesM_OnSelectedTopicChanged(Topic topic)
        {
            if (topic == null)
            {
                return;
            }
            foreach (TopicVM tvm in this.TopicVMs)
            {
                if (tvm.ID == topic.ID)
                {
                    tvm.FlowDirectionVM = FlowDirection.FromVMtoView;
                    this.SelectedTopicVM = tvm;
                    break;
                }
            }
        }

        private void ThemesM_OnAvatarsInitialized(ObservableCollection<Avatar> avatars)
        {
            this.LegendWidth = ThemesParams.LegendWidth;
            this.LegendHeight = ThemesParams.LegendHeight;
            this.RightPanelWidth = new GridLength(ThemesParams.LegendWidth);
            foreach (Avatar av in avatars)
            {
                AvatarVM avm = new AvatarVM(av);
                this.AvatarVMs.Add(avm);
            }
        }
        public void TrySelectInactiveTopic(TopicVM tvm)
        {
            this.ThemesM.TrySelectInactiveTopic(tvm.TopicM);
        }
      
        private void ThemesM_OnThemesGroupSuggestedToBuy(List<Topic> tops)
        {

        }

        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);
            foreach (TopicVM tvm in this.TopicVMs)
            {
                tvm.DeleteItself(tvm.TopicM);
            }
            this.TopicVMs.Clear();
            this.AvatarVMs.Clear();
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            this.ThemesM.OnAvatarsInitialized -= ThemesM_OnAvatarsInitialized;
            this.ThemesM.Topics.CollectionChanged -= Topics_CollectionChanged;
            this.ThemesM.OnSelectedTopicChanged -= ThemesM_OnSelectedTopicChanged;
            this.ThemesM.OnThemesGroupSuggestedToBuy -= ThemesM_OnThemesGroupSuggestedToBuy;
        }
       
    }

}
