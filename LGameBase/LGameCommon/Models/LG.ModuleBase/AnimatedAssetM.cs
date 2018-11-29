using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public partial class AnimatedAssetM : AssetM
    {
        public event AnimationEnded OnAnimationEnded;
        public int? FrameChangeTime { get; set; }

        private Timer _pictureTimer;
        private Timer PictureTimer
        {
            get
            {
                if (_pictureTimer == null)
                {
                    _pictureTimer = new Timer(FrameChangeTime == null ? 700 : (int)FrameChangeTime);
                }
                return _pictureTimer;
            }
        }
        private AnimationBehavior Behavior {get; set;}
        private int PicSourcesIterator;
        
        public AnimatedAssetM()
        {
        }

        public AnimatedAssetM(double ms)
        {
            _pictureTimer = new Timer(ms);
        }
        public AnimatedAssetM(AnimationBehavior behavior)
        {
            this.Behavior = behavior;
        }
        public AnimatedAssetM(AnimationBehavior behavior, double ms)
        {
            _pictureTimer = new Timer(ms);
            this.Behavior = behavior;
        }
        public override void DeleteItself()
        {
            base.DeleteItself();
            this.PictureTimer.OnTicked -= PictureTimer_OnTicked;
            this.PictureTimer.OnTicked -= OneTimePictureTimer_OnTicked;
            this.PictureTimer.Stop();
        }
    }

    public enum AnimationBehavior { Forever, OneTime }
    public delegate void AnimationEnded(AnimatedAssetM asset);
}
