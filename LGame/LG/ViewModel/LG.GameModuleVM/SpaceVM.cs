using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;
using LG.ViewModels.Commands;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

using LG.ViewModels.Commands;

namespace LG.ViewModels
{
    public class SpaceVM : UnitVM
    {
        #region model
        private readonly Space _space;
        private Space SpaceM
        {
            get
            {
                return _space;
            }
        }
        #endregion

        public event AssetExploded OnAssetExploded;
        public event AmmoCatched OnAmmoCatched;
        public event SaluteStarted OnSaluteStarted;
        public event TheWordToSpeakChanged OnTheWordToSpeakChanged;
        public event BlockSpeedChanged OnBlockSpeedChanged;

        public ShipControlsVM StarShipControlsVM { get; set; }

        private BaseBlockVM _assetsMovingArea;
        public BaseBlockVM AssetsMovingArea
        {
            get
            {
                return _assetsMovingArea;
            }
        }

        private ObservableCollection<PictureBoxVM> _pictureBoxVMs;
        public ObservableCollection<PictureBoxVM> PictureBoxVMs
        {
            get
            {
                if (_pictureBoxVMs == null)
                {
                    _pictureBoxVMs = new ObservableCollection<PictureBoxVM>();
                }
                return _pictureBoxVMs;
            }
        }

        private ObservableCollection<AmmoVM> _ammoVMs;
        public ObservableCollection<AmmoVM> AmmoVMs
        {
            get
            {
                if (_ammoVMs == null)
                {
                    _ammoVMs = new ObservableCollection<AmmoVM>();
                }
                return _ammoVMs;
            }
        }

        private ObservableCollection<AssetVM> _asteroidsVMs;
        public ObservableCollection<AssetVM> AsteroidsVMs
        {
            get
            {
                if (_asteroidsVMs == null)
                {
                    _asteroidsVMs = new ObservableCollection<AssetVM>();
                }
                return _asteroidsVMs;
            }
        }
        private ObservableCollection<ExplosionVM> _explosionVMs;
        public ObservableCollection<ExplosionVM> ExplosionVMs
        {
            get
            {
                if (_explosionVMs == null)
                {
                    _explosionVMs = new ObservableCollection<ExplosionVM>();
                }
                return _explosionVMs;
            }
        }

        private ObservableCollection<AssetVM> _smallSalutesVMs;
        public ObservableCollection<AssetVM> SmallSalutesVMs
        {
            get
            {
                if (_smallSalutesVMs == null)
                {
                    _smallSalutesVMs = new ObservableCollection<AssetVM>();
                }
                return _smallSalutesVMs;
            }
        }

        private ObservableCollection<AssetVM> _lasersVMs;
        public ObservableCollection<AssetVM> LasersVMs
        {
            get
            {
                if (_lasersVMs == null)
                {
                    _lasersVMs = new ObservableCollection<AssetVM>();
                }
                return _lasersVMs;
            }
        }

        // public PlayCompleted ExplosionPlayCompleted;
        public RelayCommand ExplosionPlayCompleted;

        #region view properties

        private int _spaceWidth;
        public int SpaceWidth
        {
            get
            {
                return _spaceWidth;
            }
            set
            {
                _spaceWidth = value;
                this.NotifyPropertyChanged("SpaceWidth");
            }
        }
        private int _spaceHeight;
        public int SpaceHeight
        {
            get
            {
                return _spaceHeight;
            }
            set
            {
                _spaceHeight = value;
                this.NotifyPropertyChanged("SpaceHeight");
            }
        }

        private PictureBoxVM _theWordToSpeak;
        public PictureBoxVM TheWordToSpeak
        {
            get
            {
                return _theWordToSpeak;
            }
            set
            {
                _theWordToSpeak = value;
                if (OnTheWordToSpeakChanged != null)
                {
                    OnTheWordToSpeakChanged(_theWordToSpeak);
                }
            }
        }

        private GridLength _infoBoardWidth;
        public GridLength InfoBoardWidth
        {
            get
            {
                return _infoBoardWidth;
            }
            set
            {
                _infoBoardWidth = value;
                this.NotifyPropertyChanged("InfoBoardWidth");
            }
        }

        private double _blockSpeed;
        public double BlockSpeed
        {
            get
            {
                return _blockSpeed;
            }
            set
            {
                _blockSpeed = value;
                if (this.OnBlockSpeedChanged != null)
                {
                    this.OnBlockSpeedChanged(_blockSpeed);
                }
            }
        }

        private Thickness _topMargin;
        public Thickness TopMargin
        {
            get
            {
                return _topMargin;
            }
            set
            {
                _topMargin = value;
                NotifyPropertyChanged("TopMargin");
            }
        }


        #endregion

        public double BigWayDistance { get; set; }

        public SpaceVM(Space space): base(space)
        {
            this._space = space;
            this.SpaceWidth = SpaceParams.SpaceWidth;
            this.SpaceHeight = SpaceParams.SpaceHeight;
            this.BlockSpeed = space.BlocksSpeed;
            this.BigWayDistance = SpaceParams.BigWayDistance;
            _assetsMovingArea = new BaseBlockVM(space.PicBoxGrid);
            this.StarShipControlsVM = new ShipControlsVM(this.SpaceM.StarShipControls);
            this.InfoBoardWidth = new GridLength(SpaceParams.InfoBoardWidth);

            this.SpaceM.PicBoxGrid.Assets.CollectionChanged += Assets_CollectionChanged;
            this.SpaceM.AmmosGrid.Assets.CollectionChanged += Assets_CollectionChanged;
            this.SpaceM.AsteroidsGrid.Assets.CollectionChanged += Asteroids_CollectionChanged;
            this.SpaceM.ExplosionsGrid.Assets.CollectionChanged += Assets_CollectionChanged;
            this.SpaceM.SmallSalutesGrid.Assets.CollectionChanged += SmallSalutes_CollectionChanged;
            this.SpaceM.OnTheWordPreparedForSpeak += SpaceM_OnTheWordPreparedForSpeak;
            this.SpaceM.OnPictureBoxHitted += SpaceM_OnPictureBoxExploded;
            this.SpaceM.OnAssetHitted += SpaceM_OnAssetHitted;
            this.SpaceM.OnShipCollidedWithAmmo += SpaceM_OnShipCollidedWithAmmo;
            SpaceParams.OnInfoBoardWidthChanged += SpaceParams_OnInfoBoardWidthChanged;
            space.OnBlocksSpeedChanged += SpaceParams_OnBlocksSpeedChanged;
            this.SpaceM.OnSaluteStarted += SpaceM_OnSaluteStarted;
            this.SpaceM.LasersGrid.Assets.CollectionChanged += Assets_CollectionChanged;
            this.SpaceM.OnTopMarginChanged += SpaceM_OnTopMarginChanged;
            //  ExplosionPlayCompleted.PlayCompletedCommand = ExplosionPlayCompletedCommandHandler;
            ExplosionPlayCompleted = new Commands.RelayCommand(ExplosionPlayCompletedCommandHandler);

        }

        private void SpaceParams_OnBlocksSpeedChanged(double speed)
        {
            this.BlockSpeed = speed;
        }

        private void ExplosionPlayCompletedCommandHandler()
        {
            this.SpaceM.PrepareWordToSpeak();
        }

        private void Assets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AssetM ass = (AssetM)e.NewItems[0];
                if (ass is PictureBoxM)
                {
                    PictureBoxVM pbvm = new PictureBoxVM((PictureBoxM)ass);
                    pbvm.OnItselfDeleted += pbvm_OnItselfDeleted;
                    this.PictureBoxVMs.Add(pbvm);
                }
                else if (ass is AmmoM)
                {
                    AmmoVM avm = new AmmoVM((AmmoM)ass);
                    avm.OnItselfDeleted += avm_OnItselfDeleted;
                    this.AmmoVMs.Add(avm);
                }

                else if (ass is ExplosionM)
                {
                    ExplosionVM evm = new ExplosionVM((ExplosionM)ass);
                    evm.OnItselfDeleted += evm_OnItselfDeleted;
                    this.ExplosionVMs.Add(evm);
                }
                else if (ass is LaserM)
                {
                    AssetVM lvm = new AssetVM((AssetM)ass);
                    lvm.OnItselfDeleted += lvm_OnItselfDeleted;
                    this.LasersVMs.Add(lvm);
                }
            }
        }
        private void Asteroids_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AssetM ass = (AssetM)e.NewItems[0];
                AssetVM avm = new AssetVM(ass);
                avm.OnItselfDeleted += asvm_OnItselfDeleted;
                this.AsteroidsVMs.Add(avm);
            }
        }
        private void SmallSalutes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AssetM ass = (AssetM)e.NewItems[0];
                if (ass is SaluteM)
                {
                    SaluteVM svm = new SaluteVM((SaluteM)ass);
                    svm.OnItselfDeleted += svm_OnItselfDeleted;
                    this.SmallSalutesVMs.Add(svm);
                }
            }
        }

        private void SpaceM_OnTheWordPreparedForSpeak(PictureBoxM pic)
        {
            this.TheWordToSpeak = (from p in this.PictureBoxVMs where p.PictureBox.ID == pic.ID select p).SingleOrDefault();
        }

        private void SpaceM_OnPictureBoxExploded(PictureBoxM pic)
        {
            if (this.OnAssetExploded != null)
            {
                this.OnAssetExploded();
            }
        }

        private void SpaceM_OnAssetHitted(AssetM pic)
        {
            if (this.OnAssetExploded != null)
            {
                this.OnAssetExploded();
            }
        }
        private void SpaceM_OnShipCollidedWithAmmo(AssetM ass)
        {
            if (OnAmmoCatched != null)
            {
                OnAmmoCatched((from a in this.AmmoVMs where a.ID == ass.ID select a).SingleOrDefault());
            }
        }

        private void SpaceParams_OnInfoBoardWidthChanged(int width)
        {
            this.InfoBoardWidth = new GridLength(width);
        }

        private void SpaceM_OnTopMarginChanged(int topMarg)
        {
            this.TopMargin = new Thickness(0, topMarg,0,0);
        }

        private void SpaceM_OnSaluteStarted()
        {
            if (this.OnSaluteStarted != null)
            {
                this.OnSaluteStarted();
            }
        }

        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            this.SpaceM.PicBoxGrid.Assets.CollectionChanged -= Assets_CollectionChanged;
            this.SpaceM.AmmosGrid.Assets.CollectionChanged -= Assets_CollectionChanged;
            this.SpaceM.AsteroidsGrid.Assets.CollectionChanged -= Asteroids_CollectionChanged;
            this.SpaceM.ExplosionsGrid.Assets.CollectionChanged -= Assets_CollectionChanged;
            this.SpaceM.SmallSalutesGrid.Assets.CollectionChanged -= SmallSalutes_CollectionChanged;
            this.SpaceM.OnTheWordPreparedForSpeak -= SpaceM_OnTheWordPreparedForSpeak;
            this.SpaceM.OnPictureBoxHitted -= SpaceM_OnPictureBoxExploded;
            this.SpaceM.OnAssetHitted -= SpaceM_OnAssetHitted;
            this.SpaceM.OnShipCollidedWithAmmo -= SpaceM_OnShipCollidedWithAmmo;
            SpaceParams.OnInfoBoardWidthChanged -= SpaceParams_OnInfoBoardWidthChanged;
            this.SpaceM.OnBlocksSpeedChanged -= SpaceParams_OnBlocksSpeedChanged;
            this.SpaceM.OnSaluteStarted -= SpaceM_OnSaluteStarted;
            this.SpaceM.LasersGrid.Assets.CollectionChanged -= Assets_CollectionChanged;
            this.SpaceM.OnTopMarginChanged -= SpaceM_OnTopMarginChanged;
        }
        private void pbvm_OnItselfDeleted(UnitVM bbvm)
        {
            PictureBoxVM pbvm = ((PictureBoxVM)bbvm);
            pbvm.OnItselfDeleted -= pbvm_OnItselfDeleted;

            pbvm.BoxColorsVM = null;
            pbvm.PictureBox = null;
            this.PictureBoxVMs.Remove(pbvm);
            this.TheWordToSpeak = null;
        }
        private void avm_OnItselfDeleted(UnitVM bbvm)
        {
            bbvm.OnItselfDeleted -= avm_OnItselfDeleted;
            this.AmmoVMs.Remove((AmmoVM)bbvm);
        }
        private void asvm_OnItselfDeleted(UnitVM asvm)
        {
            asvm.OnItselfDeleted -= asvm_OnItselfDeleted;
            this.AsteroidsVMs.Remove((AssetVM)asvm);
        }
        private void evm_OnItselfDeleted(UnitVM bbvm)
        {
            bbvm.OnItselfDeleted -= evm_OnItselfDeleted;
            this.ExplosionVMs.Remove((ExplosionVM)bbvm);
        }
        private void svm_OnItselfDeleted(UnitVM bbvm)
        {
            bbvm.OnItselfDeleted -= svm_OnItselfDeleted;
            this.SmallSalutesVMs.Remove((SaluteVM)bbvm);
        }
        private void lvm_OnItselfDeleted(UnitVM bbvm)
        {
            bbvm.OnItselfDeleted -= lvm_OnItselfDeleted;
            this.LasersVMs.Remove((AssetVM)bbvm);
        }
    }

    public delegate void AssetExploded();
    public delegate void AmmoCatched(AmmoVM avm);
    public delegate void SaluteStarted();
    public delegate void TheWordToSpeakChanged(PictureBoxVM pbvm);
    public delegate void BlockSpeedChanged(double speed);
}
