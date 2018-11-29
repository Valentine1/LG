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

namespace LG.Views
{
    public sealed partial class SpiralV : UserControl
    {
        private double Dx { get; set; }
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
            p.Opacity = 0.35;
            pb.Opacity = 0.40;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        public void Initialize(SpiralVM spiralVModel)
        {
            this.DataContext = spiralVModel;

            this.Dx = spiralVModel.Dx;
            this.Dy = spiralVModel.Dy;
            this.Limitx = spiralVModel.Limitx;
            this.LimitxBottom = spiralVModel.LimitxBottom;
            this.LimitxTop = spiralVModel.LimitxTop;
            this.Limity = spiralVModel.Limity;
            this.LimityBottom = spiralVModel.LimityBottom;
            this.LimityTop = spiralVModel.LimityTop;
            halfWidth = spiralVModel.HalfWidth;
            halfHeight = spiralVModel.HalfHeight;
            halfWidth_halfThick = spiralVModel.HalfWidth_halfThick;
            halfHeight_halfThick = spiralVModel.HalfHeight_halfThick;

            spiralVModel.OnStartRender += spiralVModel_OnStartRender;
            spiralVModel.OnSuspendRender += spiralVModel_OnSuspendRender;
            spiralVModel.OnReInitSizesAndPositions += spiralVModel_OnReInitSizesAndPositions;
        }

        private void spiralVModel_OnReInitSizesAndPositions(SpiralVM svm)
        {
            this.halfWidth = svm.HalfWidth;
            this.halfHeight = svm.HalfHeight;
            this.halfWidth_halfThick = svm.HalfWidth_halfThick;
            this.halfHeight_halfThick = svm.HalfHeight_halfThick;
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
                        Limitx = LimitxTop;
                        Dx += 0.004;
                    }
                    else
                    {
                        Limitx = LimitxBottom;
                        Dx -= 0.004;
                    }

                    if (Dy < Limity)
                    {
                        Limity = LimityTop;
                        Dy += 0.002;
                    }
                    else
                    {
                        Limity = LimityBottom;
                        Dy -= 0.002;
                    }
                }
            }
        }
    }
}
