using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class FireButtonV : UserControl
    {
        public event FireButtonDown OnFireButtonDown;
        public event FireButtonUp OnFireButtonUp;


        private LinearGradientBrush _backgroundBrush;
        private LinearGradientBrush BackgroundBrush
        {
            get
            {
                if (_backgroundBrush == null)
                {
                    _backgroundBrush = new LinearGradientBrush();
                    GradientStopCollection gradCol = new GradientStopCollection();
                    GradientStop st1 = new GradientStop() { Color = Color.FromArgb(255, 246, 150, 19), Offset = 0 };
                    gradCol.Add(st1);
                    GradientStop st2 = new GradientStop() { Color = Colors.OrangeRed, Offset = 0.8 };
                    gradCol.Add(st2);
                    _backgroundBrush.GradientStops = gradCol;
                    _backgroundBrush.StartPoint = new Point(0, 0);
                    _backgroundBrush.EndPoint = new Point(1, 1);
                }
                return _backgroundBrush;
            }
        }

        private LinearGradientBrush _backgroundBrushPointerOver;
        private LinearGradientBrush BackgroundBrushPointerOver
        {
            get
            {
                if (_backgroundBrushPointerOver == null)
                {
                    _backgroundBrushPointerOver = new LinearGradientBrush();
                    GradientStopCollection gradCol = new GradientStopCollection();
                    GradientStop st1 = new GradientStop() { Color = Color.FromArgb(255, 246, 150, 19), Offset = 0.8 };
                    gradCol.Add(st1);
                    GradientStop st2 = new GradientStop() { Color = Colors.OrangeRed, Offset = 0 };
                    gradCol.Add(st2);
                    _backgroundBrushPointerOver.GradientStops = gradCol;
                    _backgroundBrushPointerOver.StartPoint = new Point(0, 0);
                    _backgroundBrushPointerOver.EndPoint = new Point(1, 1);
                }
                return _backgroundBrushPointerOver;
            }
        }

        private LinearGradientBrush _backgroundBrushPointerPressed;
        private LinearGradientBrush BackgroundBrushPointerPressed
        {
            get
            {
                if (_backgroundBrushPointerPressed == null)
                {
                    _backgroundBrushPointerPressed = new LinearGradientBrush();
                    GradientStopCollection gradCol = new GradientStopCollection();
                    GradientStop st1 = new GradientStop() { Color = Color.FromArgb(255, 246, 150, 19), Offset = 0 };
                    gradCol.Add(st1);
                    GradientStop st2 = new GradientStop() { Color = Colors.Red, Offset = 0.8 };
                    gradCol.Add(st2);
                    _backgroundBrushPointerPressed.GradientStops = gradCol;
                    _backgroundBrushPointerPressed.StartPoint = new Point(0, 0);
                    _backgroundBrushPointerPressed.EndPoint = new Point(1, 1);
                }
                return _backgroundBrushPointerPressed;
            }
        }

        public FireButtonV()
        {
            this.InitializeComponent();
        }

        public void EmulateDown()
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrushPointerPressed;
        }

        public void EmulateUp()
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrush;
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrushPointerOver;
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrush;
            if (this.OnFireButtonUp != null)
            {
                this.OnFireButtonUp();
            }
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrushPointerPressed;
            if (this.OnFireButtonDown != null)
            {
                this.OnFireButtonDown();
            }
        }

        private void Grid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrushPointerOver;
            if (this.OnFireButtonUp != null)
            {
                this.OnFireButtonUp();
            }
        }
    }

    public delegate void FireButtonDown();
    public delegate void FireButtonUp();
}
