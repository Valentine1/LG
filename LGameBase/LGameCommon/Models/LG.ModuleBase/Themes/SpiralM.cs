using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public class SpiralM : BaseBlock
    {
        public event StartRender OnStartRender;
        public event RenderSuspend OnRenderSuspend;
        public event ReInitSizesAndPositions OnReInitSizesAndPositions;

        private double SpiralWidth { get; set; }
        private double SpiralHeight { get; set; }
        public double Dx;
        public double Dy;
        public double Limitx;
        public double LimitxBottom = 1.0;
        public double LimitxTop = 6.0;
        public double Limity;
        public double LimityBottom = 6.0;
        public double LimityTop = 18.0;

        public double Thickness = 10;

        public double HalfWidth
        {
            get
            {
                return this.SpiralWidth / 2;
            }
        }
        public double HalfWidth_halfThick
        {
            get
            {
                return this.SpiralWidth / 2d + Thickness / 2d;
            }
        }
        public double HalfHeight
        {
            get
            {
                return this.SpiralHeight / 2;
            }
        }
        public double HalfHeight_halfThick
        {
            get
            {
                return this.SpiralHeight / 2 + Thickness / 2d;
            }
        }

    

        public SpiralM():base()
        {
        }
        public SpiralM(double w, double h) : base()
        {
            this.InitSizesAndPositions(w, h);
        }

        public void InitSizesAndPositions(double w, double h)
        {
            this.SpiralWidth = w;
            this.SpiralHeight = h;
            this.Dx = 1.0;
            this.Dy = 6.0;

            this.Limitx = LimitxTop;
            this.Limity = LimityTop;

            this.Thickness = 10;
            this.BlockSize = new Size() { Width = this.SpiralWidth + this.Thickness, Height = this.SpiralHeight + this.Thickness };
            this.StartPosition = new Point() { X = (GlobalGameParams.WindowWidth - this.BlockSize.Width) / 2d, Y = (GlobalGameParams.WindowHeight - this.BlockSize.Height) / 2d };
            if (this.OnReInitSizesAndPositions != null)
            {
                this.OnReInitSizesAndPositions(this);
            }
       
        }

        public void SwitchOn()
        {
            if (this.OnStartRender != null)
            {
                this.OnStartRender();
            }
        }
        public void SwitchOff()
        {
            if (this.OnRenderSuspend!= null)
            {
                this.OnRenderSuspend();
            }
        }
      
    }

    public delegate void StartRender();
    public delegate void RenderSuspend();
    public delegate void ReInitSizesAndPositions(SpiralM s);
}
