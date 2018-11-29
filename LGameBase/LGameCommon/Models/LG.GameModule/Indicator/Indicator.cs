using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using LG.Common;

namespace LG.Models
{
    public abstract class Indicator: Unit
    {
        public event OpacityChanged OnOpacityChanged;

        private int _emptyammo;
        protected int Emptyammo
        {
            get
            {
                return _emptyammo;
            }
            set
            {
                _emptyammo = value;
                if (this.Ammount < 3 && this.FlashingEnabled)
                {
                    this.Flash();
                }
            }
        }

        private readonly int _capacity;
        public int Capacity
        {
            get
            {
                return _capacity;
            }
        }

        public int Ammount
        {
            get
            {
                return this.Capacity - this.Emptyammo;
            }
        }

        private double _opacity;
        public double Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                _opacity = value;
                if (this.OnOpacityChanged != null)
                {
                    this.OnOpacityChanged(_opacity);
                }
            }
        }

        private readonly Color _stripeFullClr;
        protected Color StripeFullClr
        {
            get
            {
                return _stripeFullClr;
            }
        }

        private readonly Color _stripeEmptyClr;
        protected Color StripeEmptyClr
        {
            get
            {
                return _stripeEmptyClr;
            }
        }

        protected bool FlashingEnabled
        {
            get;
            set;
        }
        private bool IsFlashing { get; set; }

        private List<Stripe> _panel;
        public List<Stripe> Panel
        {
            get
            {
                if (_panel == null)
                {
                    _panel = new List<Stripe>();
                }
                return _panel;
            }
        }

        private Timer PictureTimer
        {
            get;
            set;
        }

        private int flashCounter = 0;

        public Indicator(int stripeNumber, Color clr, IndicatorType itype, bool isFull)
        {
            _capacity = stripeNumber;
            _stripeFullClr = clr;
            this.Opacity = 1;
            if (!isFull)
            {
                this.Emptyammo = this.Capacity;
            }
            _stripeEmptyClr = new Color() { A = 255, R = 103, G = 105, B = 104 };
            Stripe str0 = new Stripe() { Area = new Rect() { Height = 3, Width = 16 }, Clr = this.StripeEmptyClr, Margin = new Thickness(1) };
            this.Panel.Add(str0);

            for (int i = 1; i <= this.Capacity; i++)
            {
                #region draw indicator stripes
                Stripe str = null;
                switch (itype)
                {
                    case IndicatorType.Thin:
                        str = new Stripe() { Area = new Rect() { Height = 3, Width = 16 }, Margin = new Thickness(1) };
                        break;
                    case IndicatorType.Middle:
                        str = new Stripe() { Area = new Rect() { Height = 5, Width = 20 }, Margin = new Thickness(1, 1.5, 1, 1.5) };
                        if (i == 1)
                        {
                            str.Margin = new Thickness(1, 1, 1, 1);
                        }
                        if (i == stripeNumber)
                        {
                            str.Margin = new Thickness(1, 1.5, 1, 1);
                        }
                        break;
                    case IndicatorType.Thick:
                        str = new Stripe() { Area = new Rect() { Height = 8, Width = 24 }, Margin = new Thickness(1, 1.5, 1, 1.5) };
                        if (i == 1)
                        {
                            str.Margin = new Thickness(1, 1, 1, 1);
                        }
                        if (i == stripeNumber)
                        {
                            str.Margin = new Thickness(1, 1.5, 1, 1);
                        }
                        break;
                }
                str.Clr = isFull ? this.StripeFullClr : this.StripeEmptyClr;
                this.Panel.Add(str);
                #endregion
            }
        }

        public void Flash()
        {
            if (!this.IsFlashing)
            {
                this.IsFlashing = true;
                PictureTimer = new Timer(90);
                PictureTimer.OnTicked += PictureTimer_OnTicked;
                this.PictureTimer.Start();
            }
        }

        private void PictureTimer_OnTicked()
        {
            this.flashCounter++;
            if (this.flashCounter == 7)
            {
                PictureTimer.OnTicked -= PictureTimer_OnTicked;
                this.PictureTimer.Stop();
                this.flashCounter = 0;
                this.IsFlashing = false;
                return;
            }
            if (this.flashCounter % 2 == 0)
            {
                this.Opacity = 1;
            }
            else
            {
                this.Opacity = 0.1;
            }
        }

        protected void ChangeColorOfUnderChargedStripes(Color clr)
        {
            for (int i = this.Capacity; i > this.Emptyammo; i--)
            {
                this.Panel[i].Clr = clr;
            }
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            foreach (Stripe str in this.Panel)
            {
                str.DeleteItself();
            }
            Panel.Clear();
            _panel = null;
        }

        public override void DetachEvents()
        {
            base.DetachEvents();
            if (PictureTimer != null)
            {
                PictureTimer.OnTicked -= PictureTimer_OnTicked;
            }

        }
    }
    public delegate void OpacityChanged(double op);

    public enum IndicatorType { Thin, Middle, Thick };


}
