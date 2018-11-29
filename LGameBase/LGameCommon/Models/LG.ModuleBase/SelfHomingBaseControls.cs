using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class SelfHomingBaseControls : BaseControls
    {
        List<double> Angles = new List<double>();

        public SelfHomingBaseControls(BaseBlock block, BaseBlock target)
        {
            this._block = block;

            double xDelta = target.Center.X - this.Block.Center.X;
            double yDelta = target.Center.Y - this.Block.Center.Y;
            xDelta = xDelta == 0 ? 0.001 : xDelta;
            yDelta = yDelta == 0 ? 0.001 : yDelta;

            if ((Math.Abs(xDelta) - Math.Abs(yDelta)) > 0)
            {
                this.SpeedX = SpaceParams.BulletSpeed;
                this.SpeedY = SpaceParams.BulletSpeed * (Math.Abs(yDelta) / Math.Abs(xDelta));
            }
            else
            {
                this.SpeedY = SpaceParams.BulletSpeed;
                this.SpeedX = SpaceParams.BulletSpeed * (Math.Abs(xDelta) / Math.Abs(yDelta));
            }
            this.SpeedX = xDelta >= 0 ? this.SpeedX : -this.SpeedX;
            this.Block.Rotation = -Math.Atan(xDelta / yDelta) * 180 / Math.PI;
            this.Angles.Add(this.Block.Rotation);

            this.Block.CalculateVertexesForRotation();

        }

    }
}
