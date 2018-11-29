using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public sealed partial class PyramidStepV : UserControl
    {
        private uint renderCount = 0;

        public PyramidStepV(PyramidStepVM psvm)
        {
            this.InitializeComponent();
            this.DataContext = psvm;
            br.BorderBrush = new SolidColorBrush(psvm.AvatarVModel.LegendColor.GradientStops[0].Color);
            profBorder.BorderBrush = psvm.AvatarVModel.LegendColor;
            psvm.OnItselfDeleted += psvm_OnItselfDeleted;
            psvm.OnUserProfileChanged += psvm_OnUserProfileChanged;
            psvm.TopProfilesVMs.CollectionChanged += TopProfilesVMs_CollectionChanged;
        }

        private void SwitchBorderShining()
        {
            profBorder.BorderBrush = arrBorderGradientBrush;
            CompositionTarget.Rendering += OnCompositionTargetRendering;
        }
        private void UnSwicthBorderShining()
        {
            CompositionTarget.Rendering -= OnCompositionTargetRendering;
        }

        private void OnCompositionTargetRendering(object sender, object args)
        {
            renderCount++;
            if (renderCount % 2 == 0)
            {
                // Calculate t from 0 to 1 repetitively
                RenderingEventArgs renderingArgs = args as RenderingEventArgs;
                double t = (0.25 * renderingArgs.RenderingTime.TotalSeconds) % 1;
                for (int index = arrBorderGradientBrush.GradientStops.Count - 1; index >= 0; index--)
                    arrBorderGradientBrush.GradientStops[index].Offset = index / 7.0 - t;
            }
        }

        private void psvm_OnUserProfileChanged(ProfileVM pvm)
        {
            if (pvm != null)
            {
                SwitchBorderShining();
            }
            else
            {
                UnSwicthBorderShining();
            }
        }

        private void TopProfilesVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (spTopProfilesThumbs.Children.Count > 0)
                {
                    int n = spTopProfilesThumbs.Children.Count;
                    if ((spTopProfilesThumbs.Children[n - 1] as StackPanel).Children.Count == 1)
                    {
                        this.AddToTheLastColumnInTopProfilesThumbs(e.NewItems[0] as ProfileVM);
                    }
                    else
                    {
                        this.StartNewColumnInTopProfilesThumbs(e.NewItems[0] as ProfileVM);
                    }
                }
                else
                {
                    this.StartNewColumnInTopProfilesThumbs(e.NewItems[0] as ProfileVM);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                spTopProfilesThumbs.Children.Clear();
            }
        }

        private void StartNewColumnInTopProfilesThumbs(ProfileVM pvm)
        {
            StackPanel spColumn = new StackPanel();
            spTopProfilesThumbs.Children.Add(spColumn);
            spColumn.Orientation = Orientation.Vertical;
            AddImageToTheStackPanel(spColumn, pvm);
        }

        private void AddToTheLastColumnInTopProfilesThumbs(ProfileVM pvm)
        {
            int n = spTopProfilesThumbs.Children.Count;
            StackPanel spColumn = (spTopProfilesThumbs.Children[n - 1] as StackPanel);
            AddImageToTheStackPanel(spColumn, pvm);

        }

        private void AddImageToTheStackPanel(StackPanel sp, ProfileVM pvm)
        {
            Border brImage = new Border();
            sp.Children.Add(brImage);
            brImage.DataContext = pvm;
            brImage.CornerRadius = new CornerRadius(2);
            brImage.Height = pvm.BlockSize.Height;
            brImage.Margin = new Thickness(1, 0.5, 1, 0.5);
            brImage.BorderThickness = new Thickness(1);
            brImage.BorderBrush = new SolidColorBrush(Colors.White);
            brImage.HorizontalAlignment = HorizontalAlignment.Left;
            CompositeTransform ct = new CompositeTransform() { ScaleX = 1, ScaleY = 1 };
            brImage.RenderTransform = ct;
            brImage.PointerEntered += brImage_PointerEntered;
            brImage.PointerExited += brImage_PointerExited;
            Image im = new Image();
            if (pvm.HasPhoto)
            {
                brImage.Child = im;
            }
            else
            {
                Grid gr = new Grid();
                brImage.Child = gr;
                gr.Children.Add(im);
                TextBlock tb = new TextBlock();
                Border br = new Border();
                br.Child = tb;
                gr.Children.Add(br);
                tb.Text = pvm.Name;
                br.Height = pvm.BlockSize.Height;
                br.Width = pvm.BlockSize.Height * 4d / 3d - 3;
                tb.FontSize = 11;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Foreground = new SolidColorBrush(Colors.Black);
                tb.FontWeight = new FontWeight() { Weight = 600 };
            }
            im.Stretch = Stretch.UniformToFill;
            im.VerticalAlignment = VerticalAlignment.Top;
            im.HorizontalAlignment = HorizontalAlignment.Left;
            Binding imBind = new Binding();
            imBind.Source = pvm;
            imBind.Path = new PropertyPath("PictureSource");
            im.SetBinding(Image.SourceProperty, imBind);
        }

        private void brImage_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            clickPlayer.Play();
            if (this.DataContext == null)
            {
                return;
            }
            ((PyramidStepVM)this.DataContext).SelectPyramidStepVM();
            ((PyramidStepVM)this.DataContext).SelectedTopProfileVM = (sender as Border).DataContext as ProfileVM;
            this.SetValue(Canvas.ZIndexProperty, 10000);
            Border br = sender as Border;
            (br.Parent).SetValue(Canvas.ZIndexProperty, 100000);
            br.SetValue(Canvas.ZIndexProperty, 110000);
            CompositeTransform ct = br.RenderTransform as CompositeTransform;
            ct.ScaleX = 1.8;
            ct.ScaleY = 1.8;
            ct.CenterX = br.ActualWidth / 2;
            ct.CenterY = br.ActualHeight / 2;
        }

        private void brImage_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.SetValue(Canvas.ZIndexProperty, 0);
            Border br = sender as Border;
            (br.Parent).SetValue(Canvas.ZIndexProperty, 1);
            br.SetValue(Canvas.ZIndexProperty, 2);
            CompositeTransform ct = br.RenderTransform as CompositeTransform;
            ct.ScaleX = 1;
            ct.ScaleY = 1;
        }

        private void psvm_OnItselfDeleted(UnitVM uvm)
        {
            this.DetachEvnets(uvm);
            this.DataContext = null;
        }

        private void DetachEvnets(UnitVM uvm)
        {
            CompositionTarget.Rendering -= OnCompositionTargetRendering;
            uvm.OnItselfDeleted -= psvm_OnItselfDeleted;
            (uvm as PyramidStepVM).OnUserProfileChanged -= psvm_OnUserProfileChanged;
            (uvm as PyramidStepVM).TopProfilesVMs.CollectionChanged -= TopProfilesVMs_CollectionChanged;
        }

        private void GetTop_Click(object sender, RoutedEventArgs e)
        {
            ((PyramidStepVM)this.DataContext).GetTopProfiles();
        }
    }
}
