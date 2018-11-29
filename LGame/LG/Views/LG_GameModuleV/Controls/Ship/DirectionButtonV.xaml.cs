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
    public sealed partial class DirectionButtonV : UserControl
    {
        public event DirectionButtonDown OnDirectionButtonDown;
        public event DirectionButtonUp OnDirectionButtonUp;

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

        private LinearGradientBrush _foregroundBrush;
        private LinearGradientBrush ForegroundBrush
        {
            get
            {
                if (_foregroundBrush == null)
                {
                    _foregroundBrush = new LinearGradientBrush();
                    GradientStopCollection gradCol = new GradientStopCollection();
                    GradientStop st1 = new GradientStop() { Color = Colors.LightGreen, Offset = 0 };
                    gradCol.Add(st1);
                    GradientStop st2 = new GradientStop() { Color = Colors.DarkGreen, Offset = 0.8 };
                    gradCol.Add(st2);
                    _foregroundBrush.GradientStops = gradCol;
                    _foregroundBrush.StartPoint = new Point(0.5, 0);
                    _foregroundBrush.EndPoint = new Point(0.5, 1);
                }
                return _foregroundBrush;
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

        private LinearGradientBrush _foregroundBrushPointerOver;
        private LinearGradientBrush ForegroundBrushPointerOver
        {
            get
            {
                if (_foregroundBrushPointerOver == null)
                {
                    _foregroundBrushPointerOver = new LinearGradientBrush();
                    GradientStopCollection gradCol = new GradientStopCollection();
                    GradientStop st1 = new GradientStop() { Color = Colors.LightGreen, Offset = 0.8 };
                    gradCol.Add(st1);
                    GradientStop st2 = new GradientStop() { Color = Colors.DarkGreen, Offset = 0 };
                    gradCol.Add(st2);
                    _foregroundBrushPointerOver.GradientStops = gradCol;
                    _foregroundBrushPointerOver.StartPoint = new Point(0.5, 0);
                    _foregroundBrushPointerOver.EndPoint = new Point(0.5, 1);
                }
                return _foregroundBrushPointerOver;
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
                    GradientStop st1 = new GradientStop() { Color = Color.FromArgb(255, 255, 243, 0), Offset = 0 };
                    gradCol.Add(st1);
                    GradientStop st2 = new GradientStop()  { Color = Color.FromArgb(255, 246, 150, 19), Offset = 0.8 };
                    gradCol.Add(st2);
                    _backgroundBrushPointerPressed.GradientStops = gradCol;
                    _backgroundBrushPointerPressed.StartPoint = new Point(0, 0);
                    _backgroundBrushPointerPressed.EndPoint = new Point(0.8, 1);
                }
                return _backgroundBrushPointerPressed;
            }
        }

        private LinearGradientBrush _foregroundBrushPointerPressed;
        private LinearGradientBrush ForegroundBrushPointerPressed
        {
            get
            {
                if (_foregroundBrushPointerPressed == null)
                {
                    _foregroundBrushPointerPressed = new LinearGradientBrush();
                    GradientStopCollection gradCol = new GradientStopCollection();
                    GradientStop st1 = new GradientStop() { Color = Colors.Orange, Offset = 0};
                    gradCol.Add(st1);
                    GradientStop st2 = new GradientStop() { Color = Colors.OrangeRed, Offset = 0.8 };
                    gradCol.Add(st2);
                    _foregroundBrushPointerPressed.GradientStops = gradCol;
                    _foregroundBrushPointerPressed.StartPoint = new Point(0.5, 0);
                    _foregroundBrushPointerPressed.EndPoint = new Point(0.5, 1);
                }
                return _foregroundBrushPointerPressed;
            }
        }

        public DirectionButtonV()
        {
            this.InitializeComponent();
        }

        public void EmulateDown()
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrushPointerPressed;
            this.Content.Foreground = this.ForegroundBrushPointerPressed;
        }

        public void EmulateUp()
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrush;
            this.Content.Foreground = this.ForegroundBrush;
        }

        private void grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrushPointerPressed;
            this.Content.Foreground = this.ForegroundBrushPointerPressed;
            if (this.OnDirectionButtonDown != null)
            {
                this.OnDirectionButtonDown();
            }
        }
        private void grid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrushPointerOver;
            this.Content.Foreground = this.ForegroundBrushPointerOver;
            if (this.OnDirectionButtonUp != null)
            {
                this.OnDirectionButtonUp();
            }
        }

        private void grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrushPointerOver;
            this.Content.Foreground = this.ForegroundBrushPointerOver;
        }
        private void grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.BackgroundGlyph.Fill = this.BackgroundBrush;
            this.Content.Foreground = this.ForegroundBrush;
            if (this.OnDirectionButtonUp != null)
            {
                this.OnDirectionButtonUp();
            }
        }
    }

    public delegate void DirectionButtonDown();
    public delegate void DirectionButtonUp();
}
