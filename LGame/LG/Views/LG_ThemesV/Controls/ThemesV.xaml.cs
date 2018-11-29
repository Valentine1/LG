using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
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
    public sealed partial class ThemesV : UserControl, IModelV
    {
        public event ShouldBeRemoved OnShouldBeRemoved;

        private ThemesVM ThemesVModel { get; set; }

        #region SelectedTopicVMProperty
        public static readonly DependencyProperty SelectedTopicVMProperty = DependencyProperty.Register("SelectedTopicVM",
                                                                                                typeof(TopicVM),
                                                                                                typeof(ThemesV),
                                                                                                new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedTopicVMChanged))
                                                                                                );
        public TopicVM SelectedTopicVM
        {
            get
            {
                return (TopicVM)GetValue(SelectedTopicVMProperty);
            }
            set
            {
                SetValue(SelectedTopicVMProperty, value);
            }
        }
        private static void OnSelectedTopicVMChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ThemesV v = (ThemesV)d;
            v.SelectedTopicVM_Changed(e.NewValue as TopicVM);
        }
        #endregion

        private List<AvatarV> _avatarsViews;
        private List<AvatarV> AvatarsViews
        {
            get
            {
                if (_avatarsViews == null)
                {
                    _avatarsViews = new List<AvatarV>();
                }
                return _avatarsViews;
            }
        }

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

        public ThemesV(ThemesVM thvm)
        {
            this.InitializeComponent();
            this.ThemesVModel = thvm;
            this.ThemesVModel.OnItselfDeleted += ThemesVModel_OnItselfDeleted;
            this.DataContext = this.ThemesVModel;
            Binding bindTop = new Binding { Source = thvm, Path = new PropertyPath("SelectedTopicVM") };
            this.SetBinding(ThemesV.SelectedTopicVMProperty, bindTop);
            this.ThemesVModel.AvatarVMs.CollectionChanged += AvatarVMs_CollectionChanged;

            this.ThemesVModel.TopicVMs.CollectionChanged += TopicVMs_CollectionChanged;
            DNASpiralV.Initialize(thvm.DNASpiralVModel);
        }
  
        private void TopicVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                TopicV tv = new TopicV((TopicVM)e.NewItems[0]);
                tv.OnShouldBeRemoved += topv_OnShouldBeRemoved;
                tv.PointerPressed += topv_PointerPressed;
                tv.RenderTransform = new ScaleTransform();
                TopicViews.Add(tv);
                canvTopics.Children.Add(tv);
            }
        }
        private void SelectedTopicVM_Changed(TopicVM tvm)
        {
            foreach (TopicV tv in this.TopicViews)
            {
                if (tv.TopicVModel.ID == tvm.ID)
                {
                    ScaleTransform st = (ScaleTransform)tv.RenderTransform;
                    st.ScaleX = 2;
                    st.ScaleY = 2;
                    st.CenterX = tv.TopicVModel.BlockSize.Width / 2;
                    st.CenterY = tv.TopicVModel.BlockSize.Height / 2;
                    tv.SetValue(Canvas.ZIndexProperty, 1000);

                    AvatarV avaView = (from a in this.AvatarsViews where a.AvatarVModel.LevelNo == tv.TopicVModel.LevelAvaVM.LevelNo select a).SingleOrDefault();
                    if (avaView != null)
                    {
                        avaView.StartPulsing();
                    }
                    break;
                }
            }
        }

        private void topv_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            TopicV topv = (sender as TopicV);
            if (!topv.TopicVModel.LicInfoVModel.IsActive)
            {
                this.ThemesVModel.TrySelectInactiveTopic(topv.TopicVModel);
                return;
            }
            double initialScale = ((ScaleTransform)topv.RenderTransform).ScaleX;
            foreach (TopicV tv in TopicViews)
            {
                ScaleTransform st = (ScaleTransform)tv.RenderTransform;
                st.ScaleX = 1;
                st.ScaleY = 1;
                Canvas.SetZIndex(tv, 1);
            }
            foreach (AvatarV ava in this.AvatarsViews)
            {
                ava.StopPulsing();
            }
            if (initialScale == 1)
            {
                ScaleTransform st = (ScaleTransform)topv.RenderTransform;
                st.ScaleX = 2;
                st.ScaleY = 2;
                st.CenterX = topv.TopicVModel.BlockSize.Width / 2;
                st.CenterY = topv.TopicVModel.BlockSize.Height / 2;
                topv.SetValue(Canvas.ZIndexProperty, 1000);

                AvatarV avaView = (from a in this.AvatarsViews where a.AvatarVModel.LevelNo == topv.TopicVModel.LevelAvaVM.LevelNo select a).SingleOrDefault();
                if (avaView != null)
                {
                    avaView.StartPulsing();
                }
                this.ThemesVModel.SelectedTopicVM = topv.TopicVModel;
            }
            else
            {
                AvatarV avaView = (from a in this.AvatarsViews where a.AvatarVModel.LevelNo == topv.TopicVModel.LevelAvaVM.LevelNo select a).SingleOrDefault();
                if (avaView != null)
                {
                    avaView.StopPulsing();
                }
                this.ThemesVModel.SelectedTopicVM = null;
            }
        }
    
        private void AvatarVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AvatarVM avm = (AvatarVM)e.NewItems[0];
                AvatarV av = null;
                if (avm.LevelNo < 13)
                {
                    av = new AvatarV(avm, false);
                }
                else
                {
                    av = new AvatarV(avm, true);
                }

                this.AvatarsViews.Add(av);
                canvLegend.Children.Add(av);
            }
        }

        #region utility functions
        private void ThemesVModel_OnItselfDeleted(UnitVM uvm)
        {
            DetachEvents();
            if (this.OnShouldBeRemoved != null)
            {
                this.OnShouldBeRemoved(this);
            }
        }

        private void topv_OnShouldBeRemoved(UIElement sender)
        {
            (sender as TopicV).OnShouldBeRemoved -= topv_OnShouldBeRemoved;
            (sender as TopicV).PointerPressed -= topv_PointerPressed;
            TopicViews.Remove(sender as TopicV);
            canvTopics.Children.Remove(sender);
        }

        private void DetachEvents()
        {
            this.ThemesVModel.OnItselfDeleted -= ThemesVModel_OnItselfDeleted;
            this.ThemesVModel.AvatarVMs.CollectionChanged -= AvatarVMs_CollectionChanged;
            this.ThemesVModel.TopicVMs.CollectionChanged -= TopicVMs_CollectionChanged;
        }
        #endregion
    }
}
