using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using LG.Models;

namespace LG.ViewModels
{
    public class PictureBoxColorsVM: BaseNotify
    {
        private SolidColorBrush _backColor;
        public SolidColorBrush BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                this.NotifyPropertyChanged("BackColor");
            }
        }

        private SolidColorBrush _borderColor;
        public SolidColorBrush BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                this.NotifyPropertyChanged("BorderColor");
            }
        }

        private SolidColorBrush _leftColor;
        public SolidColorBrush LeftColor
        {
            get
            {
                return _leftColor;
            }
            set
            {
                _leftColor = value;
                this.NotifyPropertyChanged("LeftColor");
            }
        }

        private SolidColorBrush _topColor;
        public SolidColorBrush TopColor
        {
            get
            {
                return _topColor;
            }
            set
            {
                _topColor = value;
                this.NotifyPropertyChanged("TopColor");
            }
        }

        private SolidColorBrush _leftTopColor;
        public SolidColorBrush LeftTopColor
        {
            get
            {
                return _leftTopColor;
            }
            set
            {
                _leftTopColor = value;
                this.NotifyPropertyChanged("LeftTopColor");
            }
        }

        public PictureBoxColorsVM(PictureBoxColors picBoxClrs)
        {
            if (picBoxClrs != null)
            {
                this.BackColor = new SolidColorBrush(Color.FromArgb(picBoxClrs.BackColor.A, picBoxClrs.BackColor.R, picBoxClrs.BackColor.G, picBoxClrs.BackColor.B));
                this.BorderColor = new SolidColorBrush(Color.FromArgb(picBoxClrs.BorderColor.A, picBoxClrs.BorderColor.R, picBoxClrs.BorderColor.G, picBoxClrs.BorderColor.B));
                this.LeftColor = new SolidColorBrush(Color.FromArgb(picBoxClrs.LeftColor.A, picBoxClrs.LeftColor.R, picBoxClrs.LeftColor.G, picBoxClrs.LeftColor.B));
                this.TopColor = new SolidColorBrush(Color.FromArgb(picBoxClrs.TopColor.A, picBoxClrs.TopColor.R, picBoxClrs.TopColor.G, picBoxClrs.TopColor.B));
                this.LeftTopColor = new SolidColorBrush(Color.FromArgb(picBoxClrs.LeftTopColor.A, picBoxClrs.LeftTopColor.R, picBoxClrs.LeftTopColor.G, picBoxClrs.LeftTopColor.B));
            }
        }
    }
}
