using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Common;

namespace LG.Models
{
    public partial class AvatarBackground
    {
        public event AvatarStartShowBegining OnAvatarStartShowBegining;
        public event AvatarFinishShowBegining OnAvatarFinishShowBegining;

        private AssetM _avatarImage;
        public AssetM AvatarImage
        {
            get
            {
                if (_avatarImage == null)
                {
                    _avatarImage = new AssetM();
                }
                return _avatarImage;
            }
        }

        private AssetM _avatarName;
        public AssetM AvatarName
        {
            get
            {
                if (_avatarName == null)
                {
                    _avatarName = new AssetM();
                }
                return _avatarName;
            }
        }

        async public void InitializeAndBeginAvatarStartShow(Avatar av)
        {
            this.AvatarImage.DeleteItself();
            this.AvatarName.DeleteItself();
            await this.InitializeExteriors(av);
            if (this.OnAvatarStartShowBegining != null)
            {
                this.OnAvatarStartShowBegining(this.AvatarImage, this.AvatarName);
            }
        }

        async public void InitializeAndBeginAvatarFinishShow(Avatar av)
        {
            this.AvatarImage.DeleteItself();
            this.AvatarName.DeleteItself();
            await this.InitializeExteriors(av);
            if (this.OnAvatarFinishShowBegining != null)
            {
                this.OnAvatarFinishShowBegining(this.AvatarImage, this.AvatarName);
            }
        }

        async public Task InitializeExteriors(Avatar av)
        {
            await av.InitPictureBitmap();
            this.AvatarImage.PictureSource = av.PictureSource;
            double h = SpaceParams.SpaceHeight * 0.9;
            double w = h * this.AvatarImage.RealPictureWidth / this.AvatarImage.RealPictureHeight;
            this.AvatarImage.BlockSize = new Size() { Height = h, Width = w };
            this.AvatarImage.StartPosition = new Point() { X = (SpaceParams.SpaceWidth - this.AvatarImage.BlockSize.Width) / 2d, Y = -this.AvatarImage.BlockSize.Height };
            this.AvatarImage.PositionY = (SpaceParams.SpaceHeight - this.AvatarImage.BlockSize.Height) / 2d;
            AvatarName.TextValue = av.TextValue;
        }
    }

    public delegate void AvatarStartShowBegining(AssetM avaImage, AssetM avaName);
    public delegate void AvatarFinishShowBegining(AssetM avaImage, AssetM avaName);
}
