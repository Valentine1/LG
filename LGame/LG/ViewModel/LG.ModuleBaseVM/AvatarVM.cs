using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Foundation;
using LG.Models;

namespace LG.ViewModels
{
    public class AvatarVM : AssetVM
    {
        internal Avatar AvatarM
        {
            get
            {
                return (Avatar)this.BaseBlockM;
            }
        }
        private LinearGradientBrush _legendColor;
        public LinearGradientBrush LegendColor
        {
            get
            {
                return _legendColor;
            }
            set
            {
                _legendColor = value;
                this.NotifyPropertyChanged("LegendColor");
            }
        }
        private int _levelNo;
        public int LevelNo
        {
            get
            {
                return _levelNo;
            }
            set
            {
                _levelNo = value;
                this.NotifyPropertyChanged("LevelNo");
            }
        }

        public AvatarVM(Avatar av)
            : base(av)
        {
            LinearGradientBrush LegColor = new LinearGradientBrush();
            LegColor.StartPoint = new Point(0.5, 0);
            LegColor.EndPoint = new Point(0.5, 1);
            LegColor.Opacity = 0.8;
            GradientStop gstop = new GradientStop();
            gstop.Color = Color.FromArgb(av.TopBorderClr.A, av.TopBorderClr.R, av.TopBorderClr.G, av.TopBorderClr.B);
            GradientStop gsbottom = new GradientStop();
            gsbottom.Color = Color.FromArgb(av.BottomBorderClr.A, av.BottomBorderClr.R, av.BottomBorderClr.G, av.BottomBorderClr.B);
            gsbottom.Offset = 1;
            LegColor.GradientStops.Add(gstop);
            LegColor.GradientStops.Add(gsbottom);

            this.LegendColor = LegColor;

            this.LevelNo = av.LevelNo;
        }
    }
}
