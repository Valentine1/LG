using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using LG.Models;

namespace LG.ViewModels
{
    public class StripeVM : UnitVM
    {
        private SolidColorBrush _colorBrush;
        public SolidColorBrush ColorBrush
        {
            get
            {
                return _colorBrush;
            }
            set
            {
                _colorBrush = value;
                this.NotifyPropertyChanged("ColorBrush");
            }
        }

        public Rectangle Rect
        {
            get;
            set;
        }
        public Thickness Margin { get; set; }
        public StripeVM(Stripe str):base(str)
        {
            this.ColorBrush = new SolidColorBrush(Color.FromArgb(str.Clr.A, str.Clr.R, str.Clr.G, str.Clr.B));
            this.Rect = new Rectangle() { Width = str.Area.Width, Height = str.Area.Height };
            this.Margin = new Thickness(str.Margin.Left, str.Margin.Top, str.Margin.Right, str.Margin.Bottom);

            str.OnColorChanged += str_OnColorChanged;
        }

        private void str_OnColorChanged(LG.Common.Color Clr)
        {
            this.ColorBrush.Color = Color.FromArgb(Clr.A, Clr.R, Clr.G, Clr.B);
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            (m as Stripe).OnColorChanged -= str_OnColorChanged;
        }
    }
}
