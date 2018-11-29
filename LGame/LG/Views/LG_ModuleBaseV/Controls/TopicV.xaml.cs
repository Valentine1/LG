using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Text;
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
    public sealed partial class TopicV : UserControl
    {
        public event ShouldBeRemoved OnShouldBeRemoved;

        public TopicVM TopicVModel { get; set; }

        public TopicV()
        {
            this.InitializeComponent();
        }
        public TopicV(TopicVM tvm)
        {
            this.InitializeComponent();
            this.TopicVModel = tvm;
            this.DataContext = this.TopicVModel;
            tvm.OnItselfDeleted += tvm_OnItselfDeleted;
            Initialize(tvm);
        }

        void tvm_OnItselfDeleted(UnitVM uvm)
        {
            uvm.OnItselfDeleted -= tvm_OnItselfDeleted;
            if (OnShouldBeRemoved != null)
            {
                OnShouldBeRemoved(this);
            }
        }

        public void Initialize(TopicVM tvm)
        {
            this.TopicVModel = tvm;
            this.DataContext = this.TopicVModel;
            foreach (LetterVM lvm in tvm.NameInLetters)
            {
                TextBlock tb = new TextBlock();
                tb.FontSize = 11.0;
                tb.Foreground = new SolidColorBrush(Colors.Black);
                tb.FontWeight = FontWeights.Bold;
                tb.Text = lvm.Value;
                tb.Margin = new Thickness(0, -2, 0, 0);
                tb.SetValue(Canvas.LeftProperty, lvm.StartPosition.X);
                tb.SetValue(Canvas.TopProperty, lvm.StartPosition.Y);
                RotateTransform rt = new RotateTransform();
                rt.Angle = lvm.Rotation;
                tb.RenderTransform = rt;
                canvTopic.Children.Add(tb);
            }
            foreach (PictureBoxVM pvm in tvm.Pictures)
            {
                pvm.BlockSize = new Size(18, 13.5);
                PictureBox pb = new PictureBox(pvm);
                pb.OnShouldBeRemoved += pb_OnShouldBeRemoved;
                pb.SetValue(Canvas.LeftProperty, pvm.StartPosition.X);
                pb.SetValue(Canvas.TopProperty, pvm.StartPosition.Y);
                RotateTransform rt = new RotateTransform();
                rt.Angle = pvm.Rotation;
                pb.RenderTransform = rt;
                canvTopic.Children.Add(pb);
            }
            this.TopicVModel.OnAvatarVMChanged += TopicVModel_OnAvatarVMChanged;
            this.TopicVModel.OnItselfDeleted += TopicVModel_OnItselfDeleted;
        }

        private void TopicVModel_OnAvatarVMChanged(AvatarVM ava)
        {
            pathTopic.Fill = ava.LegendColor;
        }

        private void TopicVModel_OnItselfDeleted(UnitVM uvm)
        {
            this.DetachEvents();
        }
        private void pb_OnShouldBeRemoved(UIElement sender)
        {
            (sender as PictureBox).OnShouldBeRemoved -= pb_OnShouldBeRemoved;
            canvTopic.Children.Remove(sender);
        }
        private void DetachEvents()
        {
            this.TopicVModel.OnAvatarVMChanged -= TopicVModel_OnAvatarVMChanged;
            this.TopicVModel.OnItselfDeleted -= TopicVModel_OnItselfDeleted;
        }
    }
}
