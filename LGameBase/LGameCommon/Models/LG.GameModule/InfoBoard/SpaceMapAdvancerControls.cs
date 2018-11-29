using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public class SpaceMapAdvancerControls : BaseControls
    {
        public SpaceMapAdvancer Advancer { get; set; }

        public bool IsMovingUp { get; set; }

        public SpaceMapAdvancerControls()
            : base()
        {
            Advancer = new SpaceMapAdvancer();
        }

        public void Initialize()
        {
            this.Advancer.BlockSize = new Size() { Width = SpaceParams.SpaceMapWidth, Height = SpaceParams.SpaceMapWidth * SpaceParams.SpaceAspectRatio };
            this.RecalculateAdvancerSpeed();
            SpaceParams.OnBlockSpeedChanged += SpaceParams_OnBlockSpeedChanged;
        }

        public void SetAdvancerPosition(Point p)
        {
            this.Advancer.StartPosition = p;
            this.RecalculateAdvancerSpeed();
        }

        protected override void Move()
        {
            if (this.IsMovingUp)
            {
                this.Advancer.MoveVertical(-this.DeltaTimeFromPreviousFrame * this.SpeedX);
            }
        }
        public override void DeleteItself()
        {
            base.DeleteItself();
        }
        public override void DetachEvents()
        {
            base.DetachEvents();
            SpaceParams.OnBlockSpeedChanged -= SpaceParams_OnBlockSpeedChanged;
        }
        private void SpaceParams_OnBlockSpeedChanged(double ajustedSpeed, double absoluteSpeed, bool isSmallChange)
        {
            this.RecalculateAdvancerSpeed();
        }

        private void RecalculateAdvancerSpeed()
        {
            double estTime = SpaceParams.BigWayDistance / (SpaceParams.BlockSpeed);
            this.SpeedX = Advancer.StartPosition.Y / estTime;
        }
    }
}
