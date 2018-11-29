using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class InfoBoardIntro : InfoBoard
    {

        public InfoBoardIntro(Space space): base(space)
        {
        }

        protected override void space_OnTheWordPreparedForSpeak(PictureBoxM pic)
        {
            base.space_OnTheWordPreparedForSpeak(pic);
            this.TheWordToSpeak = pic;
        }
        public override void DetachEvents()
        {
            base.DetachEvents();
            this._theWordToSpeak = null;
        }
    }
}
