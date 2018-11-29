using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using LG.Models;

namespace LG.ViewModels
{
    public class BaseBlockVM : UnitVM
    {
        public event YChanged OnYChanged;
        public event XChanged OnXChanged;
        public event RotationChanged OnRotationChanged;

        protected BaseBlock BaseBlockM
        {
            get
            {
                return (BaseBlock)this.UnitM;
            }
            set
            {
                this.UnitM = value;
            }
        }

        #region view properties
        private Point _startPosition;
        public Point StartPosition
        {
            get
            {
                return _startPosition;
            }
            set
            {
                _startPosition = value;
                this.NotifyPropertyChanged("StartPosition");
            }
        }

        private Size _blockSize;
        public Size BlockSize
        {
            get
            {
                return _blockSize;
            }
            set
            {
                _blockSize = value;
                this.NotifyPropertyChanged("BlockSize");
            }
        }

        private double _x;

        public virtual double X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                if (this.OnXChanged != null)
                {
                    OnXChanged(_x);
                }
                this.NotifyPropertyChanged("X");
            }
        }

        private double _y;
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                if (this.OnYChanged != null)
                {
                    OnYChanged(_y);
                }
                this.NotifyPropertyChanged("Y");
            }
        }

        private double _rotation;
        public double Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                if (this.OnRotationChanged != null)
                {
                    this.OnRotationChanged(_rotation);
                }
                this.NotifyPropertyChanged("Rotation");
            }
        }

        private double _scaleX;
        public double ScaleX
        {
            get
            {
                return _scaleX;
            }
            set
            {
                _scaleX = value;
                this.NotifyPropertyChanged("ScaleX");
            }
        }

        private double _scaleY;
        public double ScaleY
        {
            get
            {
                return _scaleY;
            }
            set
            {
                _scaleY = value;
                this.NotifyPropertyChanged("ScaleY");
            }
        }

        private double _centerScaleX;
        public double CenterScaleX
        {
            get
            {
                return _centerScaleX;
            }
            set
            {
                _centerScaleX = value;
                this.NotifyPropertyChanged("CenterScaleX");
            }
        }

        private double _centerScaleY;
        public double CenterScaleY
        {
            get
            {
                return _centerScaleY;
            }
            set
            {
                _centerScaleY = value;
                this.NotifyPropertyChanged("CenterScaleY");
            }
        }

        #endregion

        public BaseBlockVM()
        {

        }
        public BaseBlockVM(BaseBlock baseBlock): base(baseBlock)
        {
            this.StartPosition = new Point(this.BaseBlockM.StartPosition.X, this.BaseBlockM.StartPosition.Y);
            this.BlockSize = new Size(this.BaseBlockM.BlockSize.Width, this.BaseBlockM.BlockSize.Height);
            this.Rotation = this.BaseBlockM.Rotation;
            this.X = baseBlock.PositionX;
            this.Y = baseBlock.PositionY;
            this.CenterScaleX = baseBlock.ScaleCenterX;
            this.CenterScaleY = baseBlock.ScaleCenterY;

            this.BaseBlockM.OnStartPositionChanged += BaseBlockM_OnStartPositionChanged;
            this.BaseBlockM.OnBlockSizeChanged += BaseBlockM_OnBlockSizeChanged;
            this.OnRotationChanged += BaseBlockVM_OnRotationChanged;
            this.BaseBlockM.OnMovedX += BaseBlockM_OnMovedX;
            this.BaseBlockM.OnMovedY += BaseBlockM_OnMovedY;
            this.BaseBlockM.OnScaledX += BaseBlockM_OnScaledX;
            this.BaseBlockM.OnScaledY += BaseBlockM_OnScaledY;
            this.BaseBlockM.OnScaleCenterXChanged += BaseBlockM_OnScaleCenterXChanged;
            this.BaseBlockM.OnScaleCenterYChanged += BaseBlockM_OnScaleCenterYChanged;

            this.ScaleX = this.BaseBlockM.ScaleX;
            this.ScaleY = this.BaseBlockM.ScaleY;
        }

        protected void BaseBlockM_OnStartPositionChanged(Common.Point pos)
        {
            this.StartPosition = new Point(this.BaseBlockM.StartPosition.X, this.BaseBlockM.StartPosition.Y);
        }

        private void BaseBlockM_OnBlockSizeChanged(Common.Size sz)
        {
            if (sz.Height == 0)
            {

            }
            this.BlockSize = new Size(sz.Width, sz.Height);
        }
        private void BaseBlockVM_OnRotationChanged(double rot)
        {
            this.Rotation = rot;
        }
        protected void BaseBlockM_OnMovedX(double distance)
        {
            this.X = distance;
        }
        protected void BaseBlockM_OnMovedY(double distance)
        {
            this.Y = distance;
        }
        private void BaseBlockM_OnScaledX(double sx)
        {
            this.ScaleX = sx;
        }
        private void BaseBlockM_OnScaledY(double sy)
        {
            this.ScaleY = sy;
        }
        private void BaseBlockM_OnScaleCenterXChanged(double cx)
        {
            this.CenterScaleX = cx;
        }

        private void BaseBlockM_OnScaleCenterYChanged(double cy)
        {
            this.CenterScaleY = cy;
        }

        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);

            this.BaseBlockM.OnStartPositionChanged -= BaseBlockM_OnStartPositionChanged;
            this.BaseBlockM.OnBlockSizeChanged -= BaseBlockM_OnBlockSizeChanged;
            this.OnRotationChanged -= BaseBlockVM_OnRotationChanged;
            this.BaseBlockM.OnMovedX -= BaseBlockM_OnMovedX;
            this.BaseBlockM.OnMovedY -= BaseBlockM_OnMovedY;
            this.BaseBlockM.OnScaledX -= BaseBlockM_OnScaledX;
            this.BaseBlockM.OnScaledY -= BaseBlockM_OnScaledY;
            this.BaseBlockM.OnScaleCenterXChanged -= BaseBlockM_OnScaleCenterXChanged;
            this.BaseBlockM.OnScaleCenterYChanged -= BaseBlockM_OnScaleCenterYChanged;
        }

    }
    public delegate void YChanged(double y);
    public delegate void XChanged(double x);
    public delegate void RotationChanged(double rot);

}
