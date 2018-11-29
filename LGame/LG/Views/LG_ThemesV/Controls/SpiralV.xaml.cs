using System;
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

namespace LG.Viewss
{
    public sealed partial class SpiralV : UserControl
    {
        private double Dx {get; set;}
        private double Dy { get; set; }
        public double Limitx { get; set; }
        public double LimitxBottom { get; set; }
        public double LimitxTop { get; set; }
        public double Limity { get; set; }
        public double LimityBottom { get; set; }
        public double LimityTop { get; set; }
        double halfWidth;
        double halfWidth_halfThick;
        double halfHeight;
        double halfHeight_halfThick;

        bool IsRendering { get; set; }

        public SpiralV()
        {
            this.InitializeComponent();
            //x=cos(at)
            //y=sin(bt)  0<=t<=2PI

            //p.Stroke = new SolidColorBrush(Windows.UI.Colors.Red);
            //p.StrokeThickness = thinkness;
            p.Opacity = 0.25;
            //p.Width = width + thinkness;
            //p.Height = height + thinkness;

            //halfWidth = width / 2;
            //halfWidth_halfThick = halfWidth + thinkness / 2;

            //halfHeight = height / 2;
            //halfHeight_halfThick = halfHeight + thinkness / 2;

            //pb.StrokeThickness = thinkness;
            pb.Opacity = 0.3;
            //pb.Width = width + thinkness;
            //pb.Height = height + thinkness;

            //can.Children.Add(pb);
            //can.Children.Add(p);
            // canv.Children.Add(pb);
            //p.SetValue(Canvas.LeftProperty, (1920 - p.Width) / 2);
            //p.SetValue(Canvas.TopProperty, (1080 - p.Height) / 2);
            //pb.SetValue(Canvas.LeftProperty, (1920 - p.Width) / 2);
            //pb.SetValue(Canvas.TopProperty, (1080 - p.Height) / 2);
            CompositionTarget.Rendering += CompositionTarget_Rendering;
           
        }

        public void Initialize(SpiralVM spiralVModel)
        {
            this.DataContext = spiralVModel;

            this.Dx = 1.0;// spiralVModel.Dx;
            this.Dy = 1.0;//spiralVModel.Dy;
            this.Limitx = 6.0;// spiralVModel.Limitx;
            this.LimitxBottom = spiralVModel.LimitxBottom;
            this.LimitxTop = spiralVModel.LimitxTop;
            this.Limity = 12.0;// spiralVModel.Limity;
            this.LimityBottom = spiralVModel.LimityBottom;
            this.LimityTop = spiralVModel.LimityTop;
            halfWidth = spiralVModel.HalfWidth;
            halfHeight = spiralVModel.HalfHeight;
            halfWidth_halfThick = spiralVModel.HalfWidth_halfThick;
            halfHeight_halfThick = spiralVModel.HalfHeight_halfThick;

            spiralVModel.OnStartRender += spiralVModel_OnStartRender;
            spiralVModel.OnSuspendRender += spiralVModel_OnSuspendRender;
        }

        private void spiralVModel_OnStartRender()
        {
            this.IsRendering = true;
        }

        private void spiralVModel_OnSuspendRender()
        {
            p.Points = null;
            pb.Points = null;
            this.IsRendering = false;
        }

        //double d = 1.0;
        //double dy = 1.0;
        //double limit = 6.0;
        //double limity = 12.0;
        int j = 0;
        private void CompositionTarget_Rendering(object sender, object e)
        {
            if (this.IsRendering)
            {
                j++;
                if (j % 2 == 1)
                {
                    p.Points.Clear();
                    pb.Points.Clear();
                    for (double i = Math.PI + Math.PI / 2d; i <= 2 * Math.PI + Math.PI / 2d; i += 0.025)
                    {
                        p.Points.Add(new Point(halfWidth_halfThick + halfWidth * Math.Cos(this.Dx * i), halfHeight_halfThick + halfHeight * Math.Sin(this.Dy * i)));
                    }

                    for (double i = Math.PI / 2d; i <= Math.PI + Math.PI / 2d; i += 0.025)
                    {
                        pb.Points.Add(new Point(halfWidth_halfThick + halfWidth * Math.Cos(this.Dx * i), halfHeight_halfThick + halfHeight * Math.Sin(this.Dy * i)));
                    }

                    if (Dx < Limitx)
                    {
                        Limitx = 6.0;
                        Dx += 0.003;
                    }
                    else
                    {
                        Limitx = 3.0;
                        Dx -= 0.004;
                    }

                    if (Dy < Limity)
                    {
                        Limity = 12.0;
                        Dx += 0.002;
                    }
                    else
                    {
                        Limity = 9.0;
                        Dx -= 0.002;
                    }
                }
            }
        }
    }
}
