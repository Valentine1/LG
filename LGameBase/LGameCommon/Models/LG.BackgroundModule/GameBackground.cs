using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public class GameBackground : Module
    {
        public event AvatarShowEnd OnAvatarShowEnd;

        private SpiralM _bigSpiral;
        public SpiralM BigSpiral
        {
            get
            {
                if (_bigSpiral == null)
                {
                    _bigSpiral = new SpiralM();
                }
                return _bigSpiral;
            }
        }

        private AvatarBackground _avatarBack;
        public AvatarBackground AvatarBack
        {
            get
            {
                if (_avatarBack == null)
                {
                    _avatarBack = new AvatarBackground();
                }
                return _avatarBack;
            }
        }

        private BackSpaceGrid _backSpace;
        public BackSpaceGrid BackSpace
        {
            get
            {
                if (_backSpace == null)
                {
                    _backSpace = new BackSpaceGrid();
                }
                return _backSpace;
            }
        }

        public GameBackground()
        {
           
        }

        public override void TimeElapses(int roundedDeltaTime, double deltaTime)
        {
            this.BackSpace.TimeElapses(deltaTime);
        }

       async public override Task Initialize()
        {
            BackgroundParams.Initialize();
            double w = 1000 * GlobalGameParams.WindowWidth / 1920;
            this.BigSpiral.InitSizesAndPositions(w, w * 0.5);
            this.BackSpace.Initialize();
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            this.BackSpace.PostInitialize();
        }

        public void BeginAvatarStartShow(Avatar ava)
        {
            AvatarBack.InitializeAndBeginAvatarStartShow(ava);
        }

        public void BeginAvatarFinishShow(Avatar ava)
        {
            AvatarBack.InitializeAndBeginAvatarFinishShow(ava);
        }

        public void SwitchBigSpiralOn()
        {
           // BigSpiral.SwitchOn(); //after Nebulas images had been added I Decided to exclude sinusoid background functionality for nicer look
        }

        public void SwitchBigSpiralOff()
        {
            //BigSpiral.SwitchOff();
        }

        public void AvatarShowEnded()
        {
            if (this.OnAvatarShowEnd != null)
            {
                this.OnAvatarShowEnd();
            }
        }
    }

    public delegate void AvatarShowEnd();
}
