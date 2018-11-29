using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrepareImages
{
    /// <summary>
    /// Interaction logic for PictureBox.xaml
    /// </summary>
    public partial class PictureBox : UserControl
    {
        public event WordAdded OnWordAdded;

        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                boxBorder.Background = new SolidColorBrush(value);
            }
        }

        private Color _borderColor;
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                boxBorder.BorderBrush = new SolidColorBrush(value);
            }
        }

        private Color _leftColor;
        public Color LeftColor
        {
            get
            {
                return _leftColor;
            }
            set
            {
                _leftColor = value;
                boxLeft.Fill = new SolidColorBrush(value);
            }
        }

        private Color _topColor;
        public Color TopColor
        {
            get
            {
                return _topColor;
            }
            set
            {
                _topColor = value;
                boxTop.Fill = new SolidColorBrush(value);
            }
        }

        private Color _leftTopColor;
        public Color LeftTopColor
        {
            get
            {
                return _leftTopColor;
            }
            set
            {
                _leftTopColor = value;
                boxLeftTop.Fill = new SolidColorBrush(value);
            }
        }
        public string PictureName { set { this.tbPicName.Text = value; } }
        public string PicturePath { get; set; }
        public PictureBox(string imageName)
        {
            InitializeComponent();

            imControl.Source = new BitmapImage(new Uri(imageName));
       
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (OnWordAdded != null)
            {
                OnWordAdded(this);
            }
        }
    }

    public delegate void WordAdded(PictureBox pbox);
}
