using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class SpiralVM : BaseBlockVM
    {
        private SpiralM Spiral
        {
            get
            {
                return (SpiralM)this.BaseBlockM;
            }
        }

        public event StartRender OnStartRender;
        public event SuspendRender OnSuspendRender;
        public event ReInitSizesAndPositionsVM OnReInitSizesAndPositions;

        public double Dx { get; set; }
        public double Dy { get; set; }
        public double Limitx { get; set; }
        public double LimitxBottom { get; set; }
        public double LimitxTop { get; set; }
        public double Limity { get; set; }
        public double LimityBottom { get; set; }
        public double LimityTop { get; set; }
        private double _thickness;
        public double Thickness
        {
            get
            {
                return _thickness;
            }
            set
            {
                _thickness = value;
                this.NotifyPropertyChanged("Thickness");
            }
        }

        public double HalfWidth { get; set; }
        public double HalfWidth_halfThick { get; set; }
        public double HalfHeight { get; set; }
        public double HalfHeight_halfThick { get; set; }

        public SpiralVM(SpiralM spiral):base(spiral)
        {
            this.Dx = spiral.Dx;
            this.Dy = spiral.Dy;
            this.Limitx = spiral.Limitx;
            this.LimitxBottom = spiral.LimitxBottom;
            this.LimitxTop = spiral.LimitxTop;
            this.Limity = spiral.Limity;
            this.LimityBottom = spiral.LimityBottom;
            this.LimityTop = spiral.LimityTop;

            this.Thickness = spiral.Thickness;
            this.HalfWidth = spiral.HalfWidth;
            this.HalfWidth_halfThick = spiral.HalfWidth_halfThick;
            this.HalfHeight = spiral.HalfHeight;
            this.HalfHeight_halfThick = spiral.HalfHeight_halfThick;

            spiral.OnStartRender += spiral_OnStartRender;
            spiral.OnRenderSuspend += spiral_OnRenderSuspend;
            spiral.OnReInitSizesAndPositions += spiral_OnReInitSizesAndPositions;
        }

        private void spiral_OnReInitSizesAndPositions(SpiralM s)
        {
            this.HalfWidth = s.HalfWidth;
            this.HalfWidth_halfThick = s.HalfWidth_halfThick;
            this.HalfHeight = s.HalfHeight;
            this.HalfHeight_halfThick = s.HalfHeight_halfThick;
            if (this.OnReInitSizesAndPositions != null)
            {
                this.OnReInitSizesAndPositions(this);
            }
        }

        private void spiral_OnStartRender()
        {
            if (this.OnStartRender != null)
            {
                this.OnStartRender();
            }
        }

        private void spiral_OnRenderSuspend()
        {
            if (this.OnSuspendRender != null)
            {
                this.OnSuspendRender();
            }
        }

    }

    public delegate void StartRender();
    public delegate void SuspendRender();
    public delegate void ReInitSizesAndPositionsVM(SpiralVM svm);
}
