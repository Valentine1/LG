using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public class BaseBlock : Unit
    {
        public event StartPositionChanged OnStartPositionChanged;
        public event DisplayStartPositionChanged OnDisplayStartPositionChanged;
        public event BlockSizeChanged OnBlockSizeChanged;

        public event MovedX OnMovedX;
        public event MovedY OnMovedY;

        public event ScaledX OnScaledX;
        public event ScaledY OnScaledY;

        public event ScaleCenterXChanged OnScaleCenterXChanged;
        public event ScaleCenterYChanged OnScaleCenterYChanged;

        public event RotationChanged OnRotationChanged;

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
                if (OnStartPositionChanged != null)
                {
                    OnStartPositionChanged(_startPosition);
                }
            }
        }

        private Point _displayStartPosition;
        public Point DisplayStartPosition
        {
            get
            {
                return _displayStartPosition;
            }
            set
            {
                _displayStartPosition = value;
                if (OnDisplayStartPositionChanged != null)
                {
                    OnDisplayStartPositionChanged(_displayStartPosition);
                }
            }
        }

        private RotatedRect _rotatedCoordinates;
        public RotatedRect RotatedCoordinates
        {
            get
            {
                return _rotatedCoordinates;
            }
            set
            {
                _rotatedCoordinates = value;
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
                if (OnBlockSizeChanged != null)
                {
                    OnBlockSizeChanged(_blockSize);
                }
            }
        }

        private Point _center;
        public Point Center
        {
            get
            {
                double sx = this.StartPosition.X + this.PositionX;
                double sy = this.StartPosition.Y + this.PositionY;

                double cx = sx + this.BlockSize.Width / 2;
                double cy = sy + this.BlockSize.Height / 2;
                _center.X = cx;
                _center.Y = cy;
                return _center;
            }
        }
        private Point _displayCenter;
        public Point DisplayCenter
        {
            get
            {
                double sx = this.DisplayStartPosition.X + this.PositionX;
                double sy = this.DisplayStartPosition.Y + this.PositionY;

                double cx = sx + this.BlockSize.Width / 2;
                double cy = sy + this.BlockSize.Height / 2;
                _displayCenter.X = cx;
                _displayCenter.Y = cy;
                return _displayCenter;
            }
        }
        private double _positionX;
        public double PositionX
        {
            get
            {
                return _positionX;
            }
            set
            {
                _positionX = value;
                if (this.OnMovedX != null)
                {
                    this.OnMovedX(_positionX);
                }
            }
        }

        private double _positionY;
        public double PositionY
        {
            get
            {
                return _positionY;
            }
            set
            {
                _positionY = value;
                if (this.OnMovedY != null)
                {
                    this.OnMovedY(_positionY);
                }
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
                if (this.OnScaledX != null)
                {
                    this.OnScaledX(_scaleX);
                }
            }
        }

        private double _scaley;
        public double ScaleY
        {
            get
            {
                return _scaley;
            }
            set
            {
                _scaley = value;
                if (this.OnScaledY != null)
                {
                    this.OnScaledY(_scaley);
                }
            }
        }

        private double _scaleCenterX;
        public double ScaleCenterX
        {
            get
            {
                return _scaleCenterX;
            }
            set
            {
                _scaleCenterX = value;
                if (this.OnScaleCenterXChanged != null)
                {
                    this.OnScaleCenterXChanged(_scaleCenterX);
                }
            }
        }

        private double _scaleCenterY;
        public double ScaleCenterY
        {
            get
            {
                return _scaleCenterY;
            }
            set
            {
                _scaleCenterY = value;
                if (this.OnScaleCenterYChanged != null)
                {
                    this.OnScaleCenterYChanged(_scaleCenterY);
                }
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
            }
        }

        public BaseBlock()
        {
            this.ScaleX = 1.0;
            this.ScaleY = 1.0;
        }

        public void MoveVertical(double dist)
        {
            this.PositionY = this.PositionY + dist;
        }
        public virtual void MoveHorizontal(double dist)
        {
            this.PositionX = this.PositionX + dist;
        }
        public void Scale(double sx, double sy)
        {
            this.ScaleX = sx;
            this.ScaleY = sy;
        }

        public bool IntersectsNotRotatedWithNotRotated(BaseBlock block)
        {
            if (Math.Abs(this.Center.X - block.Center.X) < (Math.Abs(this.BlockSize.Width + block.BlockSize.Width) / 2)
                 && (Math.Abs(this.Center.Y - block.Center.Y) < (Math.Abs(this.BlockSize.Height + block.BlockSize.Height) / 2)))
            {
                return true;
            }
            return false;
        }
        public bool IntersectsNotRotatedWithRotated(BaseBlock block)
        {
            if (this.HoldsNotRotatedThePoint(new Point() { X = block.RotatedCoordinates.Vertex2.X + block.PositionX, Y = block.RotatedCoordinates.Vertex2.Y + block.PositionY })
                || this.HoldsNotRotatedThePoint(new Point() { X = block.RotatedCoordinates.Vertex3.X + block.PositionX, Y = block.RotatedCoordinates.Vertex3.Y + block.PositionY })
                || this.HoldsNotRotatedThePoint(new Point() { X = block.RotatedCoordinates.Vertex4.X + block.PositionX, Y = block.RotatedCoordinates.Vertex4.Y + block.PositionY })
                || this.HoldsNotRotatedThePoint(new Point() { X = block.RotatedCoordinates.Vertex1.X + block.PositionX, Y = block.RotatedCoordinates.Vertex1.Y + block.PositionY }))
            {
                return true;
            }
            return false;
        }

        public void CalculateVertexesForRotation()
        {
            Point LeftTop = new Point() { X = -this.BlockSize.Width / 2, Y = -this.BlockSize.Height / 2 };
            Point RightTop = new Point() { X = this.BlockSize.Width / 2, Y = -this.BlockSize.Height / 2 };
            Point RightBottom = new Point() { X = this.BlockSize.Width / 2, Y = this.BlockSize.Height / 2 };
            Point LeftBottom = new Point() { X = -this.BlockSize.Width / 2, Y = this.BlockSize.Height / 2 };
            double angleRad = -this.Rotation * Math.PI / 180;

            Point LeftTopRot = new Point() { X = LeftTop.X * Math.Cos(angleRad) + LeftTop.Y * Math.Sin(angleRad), Y = -LeftTop.X * Math.Sin(angleRad) + LeftTop.Y * Math.Cos(angleRad) };
            Point RightTopRot = new Point() { X = RightTop.X * Math.Cos(angleRad) + RightTop.Y * Math.Sin(angleRad), Y = -RightTop.X * Math.Sin(angleRad) + RightTop.Y * Math.Cos(angleRad) };
            Point RightBottomRot = new Point() { X = -LeftTopRot.X, Y = -LeftTopRot.Y };//new Point() { X = RightBottom.X * Math.Cos(angleRad) + RightBottom.Y * Math.Sin(angleRad), Y = -RightBottom.X * Math.Sin(angleRad) + RightBottom.Y * Math.Cos(angleRad) };
            Point LeftBottomRot = new Point() { X = -RightTopRot.X, Y = -RightTopRot.Y };//new Point() { X = LeftBottom.X * Math.Cos(angleRad) + LeftBottom.Y * Math.Sin(angleRad), Y = -LeftBottom.X * Math.Sin(angleRad) + LeftBottom.Y * Math.Cos(angleRad) };

            this.RotatedCoordinates = new RotatedRect()
            {
                Vertex1 = new Point()
                {
                    X = this.StartPosition.X + (LeftTopRot.X - LeftTop.X),
                    Y = this.StartPosition.Y + (LeftTopRot.Y - LeftTop.Y)
                },
                Vertex2 = new Point()
                {
                    X = this.StartPosition.X + this.BlockSize.Width + (RightTopRot.X - RightTop.X),
                    Y = this.StartPosition.Y + (RightTopRot.Y - RightTop.Y)
                },
                Vertex3 = new Point()
                {
                    X = this.StartPosition.X + this.BlockSize.Width + (RightBottomRot.X - RightBottom.X),
                    Y = this.StartPosition.Y + this.BlockSize.Height + (RightBottomRot.Y - RightBottom.Y)
                },
                Vertex4 = new Point()
                {
                    X = this.StartPosition.X + (LeftBottomRot.X - LeftBottom.X),
                    Y = this.StartPosition.Y + this.BlockSize.Height + (LeftBottomRot.Y - LeftBottom.Y)
                }
            };
        }

        private bool HoldsNotRotatedThePoint(Point p)
        {
            if (p.X > (this.StartPosition.X + this.PositionX) && p.X < (this.StartPosition.X + this.PositionX + this.BlockSize.Width)
               && p.Y > (this.StartPosition.Y + this.PositionY) && p.Y < (this.StartPosition.Y + this.PositionY + this.BlockSize.Height))
            {
                return true;
            }
            return false;
        }

        //detect if rotated rectangle holds a point (vertex of another rectangle)
        //public bool IntersectsByVertexes(BaseBlock block)
        //{
        //    //x1= xcosA+ysinA
        //    //y1 = -xsinA+ycosA

        //    //            M  of coordinates (x,y)  is inside the rectangle iff


        //    //(0<AM⋅AB<AB⋅AB)∧(0<AM⋅AD<AD⋅AD) 
        //    //(scalar product of vectors)
        //    //     uv=ux*vx +uy*vy
        //    double ABvectorX = this.RotatedCoordinates.Vertex2.X - this.RotatedCoordinates.Vertex1.X;
        //    double ABvectorY = this.RotatedCoordinates.Vertex2.Y - this.RotatedCoordinates.Vertex1.Y;
        //    double ABxAB = ABvectorX * ABvectorX + ABvectorY * ABvectorY;

        //    double ADvectorX = this.RotatedCoordinates.Vertex4.X - this.RotatedCoordinates.Vertex1.X;
        //    double ADvectorY = this.RotatedCoordinates.Vertex4.Y - this.RotatedCoordinates.Vertex1.Y;
        //    double ADxAD = ADvectorX * ADvectorX + ADvectorY * ADvectorY;

        //    double AMvectorX = block.StartPosition.X + block.PositionX - this.RotatedCoordinates.Vertex1.X;
        //    double AMvectorY = block.StartPosition.Y + block.PositionY - this.RotatedCoordinates.Vertex1.Y;
        //    double ABxAM = ABvectorX * AMvectorX + ABvectorY * AMvectorY;
        //    double ADxAM = ADvectorX * AMvectorX + ADvectorY * AMvectorY;


        //    if ((ABxAM > 0 && ABxAM < ABxAB) && (ADxAM > 0 && ADxAM < ADxAD))
        //    {
        //        return true;
        //    }

        //    return false;
        //}
    }
    public delegate void StartPositionChanged(Point pos);
    public delegate void DisplayStartPositionChanged(Point pos);
    public delegate void BlockSizeChanged(Size sz);
    public delegate void MovedX(double distance);
    public delegate void MovedY(double distance);
    public delegate void ScaledX(double sx);
    public delegate void ScaledY(double sy);
    public delegate void ScaleCenterXChanged(double cx);
    public delegate void ScaleCenterYChanged(double cy);
    public delegate void RotationChanged(double rot);
}
