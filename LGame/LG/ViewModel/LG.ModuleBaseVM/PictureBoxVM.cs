using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using Windows.UI;
using LG.Models;

namespace LG.ViewModels
{
    public class PictureBoxVM : AssetVM
    {
        public PictureBoxM PictureBox
        {
            get
            {
                return (PictureBoxM)this.BaseBlockM;
            }
            set
            {
                this.BaseBlockM = value;
            }
        }
        public int WordID { get; set; }

        #region view properties
 
        private PictureBoxColorsVM _boxColorsVM;
        public PictureBoxColorsVM BoxColorsVM
        {
            get
            {
                return _boxColorsVM;
            }
            set
            {
                _boxColorsVM = value;
                this.NotifyPropertyChanged("BoxColorsVM");
            }
        }
        private string _word;
        public string Word
        {
            get
            {
                return _word;
            }
            set
            {
                _word = value;
                this.NotifyPropertyChanged("Word");
            }
        }
        #endregion

        public PictureBoxVM (PictureBoxM pb) : base(pb)
        {
            this.WordID = pb.WordID;
            this.Word = pb.TextValue;
            if (this.PictureBox.BoxColors != null)
            {
                this.BoxColorsVM = new PictureBoxColorsVM(this.PictureBox.BoxColors);
            }
            this.PictureBox.OnPictureBoxColorsChanged += PictureBox_OnPictureBoxColorsChanged;
        }

        void PictureBox_OnPictureBoxColorsChanged(PictureBoxColors pbc)
        {
            this.BoxColorsVM = new PictureBoxColorsVM(pbc);
        }
     
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            this.PictureBox.OnPictureBoxColorsChanged -= PictureBox_OnPictureBoxColorsChanged;
        }

    }
}
