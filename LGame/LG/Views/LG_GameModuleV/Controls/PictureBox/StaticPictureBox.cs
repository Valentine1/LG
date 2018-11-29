using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.ViewModels;

namespace LG.Views
{
    public class StaticPictureBox : PictureBox
    {
        public StaticPictureBox()
        {
            this.DisableMovement();
        }
        public StaticPictureBox(PictureBoxVM pbvm):base(pbvm)
        {

        }
    }
}
