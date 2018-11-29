using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LG.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class HierarchyV : UserControl, IModelV
    {
        public event ShouldBeRemoved OnShouldBeRemoved;

        private HierarchyVM HierarchyVModel { get; set; }

        private List<TopicV> _topicViews;
        private List<TopicV> TopicViews
        {
            get
            {
                if (_topicViews == null)
                {
                    _topicViews = new List<TopicV>();
                }
                return _topicViews;
            }
        }

        public HierarchyV(HierarchyVM hierVM)
        {
            this.InitializeComponent();
            this.DataContext = hierVM;
            this.HierarchyVModel = hierVM;
           
            this.HierarchyVModel.OnItselfDeleted += HierarchyVModel_OnItselfDeleted;
            hierVM.Steps.CollectionChanged += Steps_CollectionChanged;
            this.HierarchyVModel.TopicVMs.CollectionChanged += TopicVMs_CollectionChanged;
            this.HierarchyVModel.OnSelectedTopicVMChanged += HierarchyVModel_OnSelectedTopicVMChanged;
        }

        private void HierarchyVModel_OnSelectedTopicVMChanged(TopicVM tvm)
        {
            for (int i = 0; i < this.TopicViews.Count;i++ )
            {
                if (TopicViews[i].TopicVModel.ID == tvm.ID)
                {
                    cbTopics.SelectionChanged -= cbTopics_SelectionChanged;
                    cbTopics.SelectedIndex = i;
                    cbTopics.SelectionChanged += cbTopics_SelectionChanged;
                    break;
                }
            }
        }

        private void Steps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                PyramidStepVM pvm = (PyramidStepVM)e.NewItems[0];
                UserControl pv = null;
                if (pvm.AvatarVModel.LevelNo < 13)
                {
                    pv = new PyramidStepV(pvm);
                }
                else
                {
                    pv = new PyramidTopAllSeeingV(pvm);
                }
                canHierarchy.Children.Add(pv);
            }
        }
        private void TopicVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                TopicVM tvm = (TopicVM)e.NewItems[0];
                TopicV tv = new TopicV(tvm);
                this.TopicViews.Add(tv);
                ComboBoxItem item = new ComboBoxItem();
                Canvas can = new Canvas();
                can.Height = tvm.BlockSize.Height;
                can.Width = tvm.BlockSize.Width + 5;
                can.Children.Add(tv);
                item.Content = can;
                can.Margin = new Thickness(-3, 0, 0, 0);
                item.Height = can.Height + 20;
                item.Width = can.Width;
                cbTopics.Items.Add(item);
            }
        }
      
        private void cbTopics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TopicV tv= TopicViews[cbTopics.SelectedIndex];
            foreach(TopicVM tvm in this.HierarchyVModel.TopicVMs)
            {
                if(tvm.ID == tv.TopicVModel.ID)
                {
                    this.HierarchyVModel.SelectedTopicVM = tvm;
                    break;
                }
            }
          
        }
        private void HierarchyVModel_OnItselfDeleted(UnitVM uvm)
        {
            this.HierarchyVModel.OnItselfDeleted -= HierarchyVModel_OnItselfDeleted;
            this.HierarchyVModel.Steps.CollectionChanged -= Steps_CollectionChanged;
            this.HierarchyVModel.TopicVMs.CollectionChanged -= TopicVMs_CollectionChanged;
            this.HierarchyVModel.OnSelectedTopicVMChanged -= HierarchyVModel_OnSelectedTopicVMChanged;
            this.DataContext = null;
            if (this.OnShouldBeRemoved != null)
            {
                this.OnShouldBeRemoved(this);
            }
        }

        private void Viewbox_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

    }
}
