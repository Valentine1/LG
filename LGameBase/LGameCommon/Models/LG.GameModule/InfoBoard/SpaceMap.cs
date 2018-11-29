using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public class SpaceMap : BaseBlock
    {
        private const int AbsHeightDifference = 672;  //1440-768
        private const int SpaceMapBottomMarginMin = 40;
        private const int SpaceMapBottomMarginMax = 120;
        private bool IsMapRunning { get; set; }

        private SpaceMapAdvancerControls _advancerControls;
        public SpaceMapAdvancerControls AdvancerControls
        {
            get
            {
                if (_advancerControls == null)
                {
                    _advancerControls = new SpaceMapAdvancerControls();
                }
                return _advancerControls;
            }
        }

        public void TimeElapses(int roundedDeltaTime, double deltaTime)
        {
            if (this.IsMapRunning)
            {
                this.AdvancerControls.TimeElapses(deltaTime);
            }
        }

        public void Initialize()
        {
            this.IsMapRunning = true;
            this.AdvancerControls.Initialize();
            this.AdvancerControls.IsMovingUp = true;
            int heightDifference = GlobalGameParams.WindowHeight - GlobalGameParams.WindowHeightMin;

            int b = SpaceMapBottomMarginMin + heightDifference * (SpaceMapBottomMarginMax - SpaceMapBottomMarginMin) / AbsHeightDifference;

            double h = GlobalGameParams.WindowHeight - SpaceParams.InfoBoardHeight - b;
            this.BlockSize = new Size() { Height = h, Width = SpaceParams.SpaceMapWidth };

            this.AdvancerControls.SetAdvancerPosition(new Point() { X = 0, 
                                                                    Y = this.BlockSize.Height - this.AdvancerControls.Advancer.BlockSize.Height - SpaceParams.SpaceMapBottomHeightForShip });
        }

        public void StopMap()
        {
            this.IsMapRunning = false;
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            this.AdvancerControls.DeleteItself();
        }
    }
}
