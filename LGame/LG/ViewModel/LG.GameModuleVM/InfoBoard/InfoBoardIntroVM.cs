using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class InfoBoardIntroVM : InfoBoardVM
    {
       

        public InfoBoardIntroVM(SpaceControllerBase controler) : base(controler)
        {
            controler.Board.OnTheWordToSpeakChanged += board_OnTheWordToSpeakChanged;
           
        }

      
        private void board_OnTheWordToSpeakChanged(PictureBoxM pb)
        {
            this.SelectedWord = new PictureBoxVM(pb);
        }

       
        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);
            this.SelectedWord = null;
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            (m as SpaceControllerBase).Board.OnTheWordToSpeakChanged -= board_OnTheWordToSpeakChanged;
        }
    }

  
}
