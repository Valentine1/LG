using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using LG.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class SpaceV : UserControl
    {
        private SpaceVM SpaceVModel { get; set; }

        private Dictionary<int, Stack<PictureBox>> _pictureBoxStacks;
        public Dictionary<int, Stack<PictureBox>> PictureBoxStacks
        {
            get
            {
                if (_pictureBoxStacks == null)
                {
                    _pictureBoxStacks = new Dictionary<int, Stack<PictureBox>>();
                }
                return _pictureBoxStacks;
            }
        }

        private Dictionary<int, MediaElement> _audioTracks;
        public Dictionary<int, MediaElement> AudioTracks
        {
            get
            {
                if (_audioTracks == null)
                {
                    _audioTracks = new Dictionary<int, MediaElement>();
                }
                return _audioTracks;
            }
        }
        private Storyboard MovingStoryboard = new Storyboard();
        private bool IsMovingStoryboardSet;
        private double BigWayDistance { get; set; }

        public SpaceV()
        {
            this.InitializeComponent();
        }

        public void Initialize(SpaceVM svm, bool isSoundSupported)
        {
            this.SpaceVModel = svm;
            this.DataContext = svm;
            this.BigWayDistance = svm.BigWayDistance;
            this.SShip.Initialize(svm.StarShipControlsVM);
            //tCrossHairView.DataContext = svm.StarShipControlsVM.TargetCrossHairVM;
            //imLaserBeam.DataContext = svm.StarShipControlsVM.TargetCrossHairVM.LaserBeamVM;
            svm.PictureBoxVMs.CollectionChanged += PictureBoxVMs_CollectionChanged;
            svm.AmmoVMs.CollectionChanged += AssetVMs_CollectionChanged;
            svm.AsteroidsVMs.CollectionChanged += AssetVMs_CollectionChanged;
            svm.ExplosionVMs.CollectionChanged += ExplosionVMs_CollectionChanged;
            svm.SmallSalutesVMs.CollectionChanged += SmallSalutesVMs_CollectionChanged;
            svm.LasersVMs.CollectionChanged += LasersVMs_CollectionChanged;

            svm.StarShipControlsVM.ShipVM.FiredBulletVMs.CollectionChanged += FiredBulletVMs_CollectionChanged;
            if (isSoundSupported)
            {
                svm.PictureBoxVMs.CollectionChanged += PictureBoxVMs_CollectionChangedAddSounds;
                svm.OnTheWordToSpeakChanged += SpaceVM_OnTheWordToSpeakChanged;
            }
            svm.AssetsMovingArea.OnYChanged += AssetsMovingArea_OnYChanged;
            svm.OnBlockSpeedChanged += svm_OnBlockSpeedChanged;
            svm.OnItselfDeleted += SpaceVM_OnItselfDeleted;
        }
        private void svm_OnBlockSpeedChanged(double speed)
        {
            if (this.IsMovingStoryboardSet)
            {
                this.SetMovingStoryboard();
            }
        }
        private void AssetsMovingArea_OnYChanged(double y)
        {
            if (!this.IsMovingStoryboardSet)
            {
                paTrans.Y = y;
                this.SetMovingStoryboard();
                this.IsMovingStoryboardSet = true;
            }
            else if (Math.Abs(paTrans.Y - y) > 7)
            {
                paTrans.Y = y;

                this.SetMovingStoryboard();
            }
        }

        private void SetMovingStoryboard()
        {
            this.MovingStoryboard.Stop();
            this.MovingStoryboard.Children.Clear();
            DoubleAnimation daExpand = new DoubleAnimation()
            {
                From = SpaceVModel.AssetsMovingArea.Y,
                To = this.BigWayDistance,
                AutoReverse = false,
                Duration = new TimeSpan(0, 0, 0, 0, (int)((this.BigWayDistance - SpaceVModel.AssetsMovingArea.Y) / SpaceVModel.BlockSpeed))
            };
            Storyboard.SetTarget(daExpand, this.paTrans);
            Storyboard.SetTargetProperty(daExpand, "Y");
            this.MovingStoryboard.Children.Add(daExpand);
            this.MovingStoryboard.Begin();
        }

        private void SpaceVM_OnTheWordToSpeakChanged(PictureBoxVM pbvm)
        {
            if (pbvm != null && AudioTracks.Keys.Contains(pbvm.WordID))
            {
                AudioTracks[pbvm.WordID].Play();
            }
            else
            {

            }
        }

        public void DeleteShip()
        {
            this.playArea.Children.Remove(this.SShip);
        }

        private void PictureBoxVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                PictureBoxVM pbvm = (PictureBoxVM)e.NewItems[0];

                PictureBox pbv = new PictureBox(pbvm);
                pbv.OnShouldBeRemoved += pbv_OnShouldBeRemoved;
                moveArea.Children.Add(pbv);
            }
        }

 
        private void PictureBoxVMs_CollectionChangedAddSounds(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                PictureBoxVM pbvm = (PictureBoxVM)e.NewItems[0];
                if (!AudioTracks.Keys.Contains(pbvm.WordID) && pbvm.AudioStream != null)
                {
                    MediaElement me = new MediaElement();
                    mainGridSpace.Children.Add(me);
                    me.SetSource(pbvm.AudioStream, "audio/mpeg");
                    me.IsLooping = false;
                    me.MediaFailed += me_MediaFailed;
                    me.AutoPlay = false;
                    AudioTracks.Add(pbvm.WordID, me);
                }
            }
        }

        void me_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            
        }
        private PictureBox GetAvailablePictureBox(int id)
        {
            if (PictureBoxStacks.Keys.Contains(id))
            {
                if (PictureBoxStacks[id].Count > 0)
                {
                    return PictureBoxStacks[id].Pop();
                }
            }
            else
            {
                PictureBoxStacks.Add(id, new Stack<PictureBox>());
            }
            return null;
        }

        private void AssetVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AssetVM avm = (AssetVM)e.NewItems[0];
                AssetV av = new AssetV(avm);
                av.OnShouldBeRemoved += av_OnShouldBeRemoved;
                moveArea.Children.Add(av);
            }
        }

        private void ExplosionVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ExplosionVM evm = (ExplosionVM)e.NewItems[0];
                ExplosionV ev = new ExplosionV(evm);
                ev.OnShouldBeRemoved += ev_OnShouldBeRemoved;
                playArea.Children.Add(ev);
            }
        }

        private void SmallSalutesVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                SaluteVM svm = (SaluteVM)e.NewItems[0];
                SaluteV sv = new SaluteV(svm);
                sv.OnShouldBeRemoved += sv_OnShouldBeRemoved;
                playArea.Children.Add(sv);
            }
        }

        private void LasersVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AssetVM lvm = (AssetVM)e.NewItems[0];
                LaserV lv = new LaserV(lvm);
                lv.OnShouldBeRemoved += lv_OnShouldBeRemoved;
                playArea.Children.Add(lv);
            }
        }

        private void FiredBulletVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                BulletVM bvm = (BulletVM)e.NewItems[0];
                Bullet bv = new Bullet(bvm);
                bv.OnShouldBeRemoved += bv_OnShouldBeRemoved;
                playArea.Children.Add(bv);
            }
        }

        public void SpaceVM_OnItselfDeleted(UnitVM uvm)
        {
            DetachEvents(uvm as SpaceVM);
            this.MovingStoryboard.Stop();
            this.MovingStoryboard.Children.Clear();

            AudioTracks.Clear();
            _audioTracks = null;
            mainGridSpace.Children.Clear();
        }

        private void pbv_OnShouldBeRemoved(UIElement sender)
        {
            ((PictureBox)sender).OnShouldBeRemoved -= pbv_OnShouldBeRemoved;
            //sender.Opacity = 0.001;
            // PictureBoxStacks[((PictureBox)sender).WordID].Push(((PictureBox)sender));
            moveArea.Children.Remove(sender);
        }
        private void av_OnShouldBeRemoved(UIElement sender)
        {
            ((AssetV)sender).OnShouldBeRemoved -= av_OnShouldBeRemoved;
            moveArea.Children.Remove(sender);
        }
        private void ev_OnShouldBeRemoved(UIElement sender)
        {
            ((ExplosionV)sender).OnShouldBeRemoved -= ev_OnShouldBeRemoved;
            playArea.Children.Remove(sender);
        }
        private void bv_OnShouldBeRemoved(UIElement sender)
        {
            ((Bullet)sender).OnShouldBeRemoved -= bv_OnShouldBeRemoved;
            playArea.Children.Remove(sender);
        }
        private void sv_OnShouldBeRemoved(UIElement sender)
        {
            ((SaluteV)sender).OnShouldBeRemoved -= sv_OnShouldBeRemoved;
            playArea.Children.Remove(sender);
        }
        private void lv_OnShouldBeRemoved(UIElement sender)
        {
            ((LaserV)sender).OnShouldBeRemoved -= lv_OnShouldBeRemoved;
            playArea.Children.Remove(sender);
        }

        private void DetachEvents(SpaceVM svm)
        {
            svm.PictureBoxVMs.CollectionChanged -= PictureBoxVMs_CollectionChanged;
            svm.AmmoVMs.CollectionChanged -= AssetVMs_CollectionChanged;
            svm.ExplosionVMs.CollectionChanged -= ExplosionVMs_CollectionChanged;
            svm.SmallSalutesVMs.CollectionChanged -= SmallSalutesVMs_CollectionChanged;
            svm.LasersVMs.CollectionChanged -= LasersVMs_CollectionChanged;

            svm.StarShipControlsVM.ShipVM.FiredBulletVMs.CollectionChanged -= FiredBulletVMs_CollectionChanged;
            svm.OnTheWordToSpeakChanged -= SpaceVM_OnTheWordToSpeakChanged;
            svm.AssetsMovingArea.OnYChanged -= AssetsMovingArea_OnYChanged;
            svm.OnBlockSpeedChanged -= svm_OnBlockSpeedChanged;

        }

    }
}
