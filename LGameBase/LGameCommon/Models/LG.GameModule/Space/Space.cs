using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public class Space : Unit
    {
        public event TheWordPreparedForSpeak OnTheWordPreparedForSpeak;
        public event DelayEndedForPictureToSpeakShow OnDelayEndedForPictureToSpeakShow;
        public event PictureBoxHitted OnPictureBoxHitted;
        public event AssetHitted OnAssetHitted;
        public event ShipHitted OnShipHitted;
        public event BulletRunOutOfSpace OnBulletRunOutOfSpace;
        public event PictureBoxesRunOut OnPictureBoxesRunOut;
        public event ShipCollidedWithAmmo OnShipCollidedWithAmmo;
        public event SaluteStarted OnSaluteStarted;
        public event BlocksSpeedChanged OnBlocksSpeedChanged;
        public event TopMarginChanged OnTopMarginChanged;

        private ShipControls _controls;
        public ShipControls StarShipControls
        {
            get
            {
                if (_controls == null)
                {
                    _controls = new ShipControls();
                }
                return _controls;
            }
        }

        private PictureBoxGrid _picBoxGrid;
        public PictureBoxGrid PicBoxGrid
        {
            get
            {
                if (_picBoxGrid == null)
                {
                    _picBoxGrid = new PictureBoxGrid();
                }
                return _picBoxGrid;
            }
        }

        private AmmoGrid _ammosGrid;
        public AmmoGrid AmmosGrid
        {
            get
            {
                if (_ammosGrid == null)
                {
                    _ammosGrid = new AmmoGrid();
                }
                return _ammosGrid;
            }
        }

        private AsteroidGrid _asteroidsGrid;
        public AsteroidGrid AsteroidsGrid
        {
            get
            {
                if (_asteroidsGrid == null)
                {
                    _asteroidsGrid = new AsteroidGrid();
                }
                return _asteroidsGrid;
            }
        }

        private ExplosionGrid _explosionsGrid;
        public ExplosionGrid ExplosionsGrid
        {
            get
            {
                if (_explosionsGrid == null)
                {
                    _explosionsGrid = new ExplosionGrid();
                }
                return _explosionsGrid;
            }
        }

        private SaluteGrid _smallSalutesGrid;
        public SaluteGrid SmallSalutesGrid
        {
            get
            {
                if (_smallSalutesGrid == null)
                {
                    _smallSalutesGrid = new SaluteGrid(2, 2);
                }
                return _smallSalutesGrid;
            }
        }

        private LaserGrid _lasersGrid;
        public LaserGrid LasersGrid
        {
            get
            {
                if (_lasersGrid == null)
                {
                    _lasersGrid = new LaserGrid();
                }
                return _lasersGrid;
            }
        }

        private Timer _wordPreparedDelayTimer;
        private Timer WordPreparedDelayTimer
        {
            get
            {
                if (_wordPreparedDelayTimer == null)
                {
                    _wordPreparedDelayTimer = new Timer(500);
                    _wordPreparedDelayTimer.OnTickedStatefull += WordPreparedDelayTimer_OnTickedStatefull;
                }
                return _wordPreparedDelayTimer;
            }
        }

        private Timer _delayForPictureToSpeakShowTimer;
        private Timer DelayForPictureToSpeakShowTimer
        {
            get
            {
                int delay = this.GetPictureShowDelayTime();
                if (_delayForPictureToSpeakShowTimer == null)
                {
                    _delayForPictureToSpeakShowTimer = new Timer(delay * 1000);
                }
                else
                {
                    _delayForPictureToSpeakShowTimer.SetInterval(delay * 1000);
                }

                _delayForPictureToSpeakShowTimer.OnTicked -= DelayForPictureToSpeakShowTimer_OnTicked;
                _delayForPictureToSpeakShowTimer.OnTicked += DelayForPictureToSpeakShowTimer_OnTicked;

                return _delayForPictureToSpeakShowTimer;
            }
        }

        private double _blocksSpeed;
        public double BlocksSpeed
        {
            get
            {
                return _blocksSpeed;

            }
            set
            {
                _blocksSpeed = value;
                if (this.OnBlocksSpeedChanged != null)
                {
                    this.OnBlocksSpeedChanged(_blocksSpeed);
                }
            }
        }

        private string TheWordToSpeak { get; set; }
        private double TimeForNewRow { get; set; }

        private int _topMargin;
        public int TopMargin
        {
            get
            {
                return _topMargin;
            }
            set
            {
                _topMargin = value;
                if (this.OnTopMarginChanged != null)
                {
                    this.OnTopMarginChanged(_topMargin);
                }
            }
        }

        private bool IsReached4Handled { get; set; }
        public Space()
        {
            SpaceParams.OnBlockSpeedChanged += SpaceParams_OnBlockSpeedChanged;
        }

        private void SpaceParams_OnBlockSpeedChanged(double ajustedSpeed, double absoluteSpeed, bool isSmallChange)
        {
            BlocksSpeed = ajustedSpeed;
        }

        public async void Initialize()
        {
            await this.PicBoxGrid.Initialize();
            this.PicBoxGrid.Assets.CollectionChanged += PictureBoxes_CollectionChanged;
            this.AmmosGrid.Initialize();
            this.AsteroidsGrid.Initialize();
            this.ExplosionsGrid.Initialize();
            this.LasersGrid.Initialize();
            this.StarShipControls.Initialize();
            List<AssetColumnsGrid> opAreas = new List<AssetColumnsGrid>();
            opAreas.Add(this.PicBoxGrid);
            opAreas.Add(this.AmmosGrid);
            opAreas.Add(this.AsteroidsGrid);
            StarShipControls.TargetCrossHair.Initialize(opAreas);

            int m = GlobalGameParams.WindowHeight - SpaceParams.SpaceHeight - (int)Math.Round(this.StarShipControls.Ship.BlockSize.Height) - GlobalGameParams.GlobalInfoPanelHeight - 10;
            if (m > 0)
            {
                if (m >= (int)(GlobalGameParams.GlobalInfoPanelHeight * 1.3))
                {
                    this.StarShipControls.PanelHeight = (int)(GlobalGameParams.GlobalInfoPanelHeight * 1.3);
                }
                else
                {
                    this.StarShipControls.PanelHeight = m;
                }
            }
            else
            {
                this.TopMargin = m;
                this.StarShipControls.PanelHeight = GlobalGameParams.GlobalInfoPanelHeight;
            }
        }

        #region time elapses related methods

        private DateTime previousTime;

        public void TimeElapses(int roundedDeltaTime, double deltaTime)
        {
            if (!(previousTime.Month == 1))
            {
                TimeSpan ts = DateTime.Now - previousTime;
            }
            previousTime = DateTime.Now;

            this.StarShipControls.TimeElapses(deltaTime);
            this.PicBoxGrid.TimeElapses(deltaTime);
            this.AmmosGrid.TimeElapses(deltaTime);
            this.AsteroidsGrid.TimeElapses(deltaTime);
            this.LasersGrid.TimeElapses(deltaTime);
        }

        #endregion

        #region Picture Boxes creation

        internal void TryCreateNewRow(double roundedDeltaTime)
        {
            this.TimeForNewRow = this.TimeForNewRow + roundedDeltaTime;
            if (this.TimeForNewRow >= ((double)SpaceParams.PictureBlockHeight / SpaceParams.BlockSpeed))
            {
                this.TimeForNewRow = 0;
                this.PicBoxGrid.CreateAssetLine();
                this.TryCreateAmmoAsset();
            }
        }
        private int GetPictureShowDelayTime()
        {
            int delay = 0;
            if (Game.GameTopic == null)
            {
                return delay;
            }
            switch (Game.GameTopic.HierarchyLevel.LevelNo)
            {
                case 1:
                    if (Game.CoveredDistancePercantage < 25)
                    {
                        delay = 1;
                    }
                    else if (Game.CoveredDistancePercantage < 50)
                    {
                        delay = 2;
                    }
                    else if (Game.CoveredDistancePercantage < 75)
                    {
                        delay = 3;
                    }
                    else
                    {
                        delay = 4;
                    }
                    break;
                case 2:
                    if (Game.CoveredDistancePercantage < 25)
                    {
                        delay = 1;
                    }
                    else if (Game.CoveredDistancePercantage < 40)
                    {
                        delay = 2;
                    }
                    else if (Game.CoveredDistancePercantage < 50)
                    {
                        delay = 3;
                    }
                    else if (Game.CoveredDistancePercantage < 65)
                    {
                        delay = 1;
                    }
                    else if (Game.CoveredDistancePercantage < 75)
                    {
                        delay = 2;
                    }
                    else if (Game.CoveredDistancePercantage < 90)
                    {
                        delay = 3;
                    }
                    else
                    {
                        delay = 4;
                    }
                    break;
                case 3:
                    if (Game.CoveredDistancePercantage < 25)
                    {
                        delay = 1;
                    }
                    else if (Game.CoveredDistancePercantage < 40)
                    {
                        delay = 2;
                    }
                    else if (Game.CoveredDistancePercantage < 50)
                    {
                        delay = 3;
                    }
                    else if (Game.CoveredDistancePercantage < 65)
                    {
                        delay = 1;
                    }
                    else if (Game.CoveredDistancePercantage < 75)
                    {
                        delay = 2;
                    }
                    else if (Game.CoveredDistancePercantage < 90)
                    {
                        delay = 3;
                    }
                    else
                    {
                        delay = 4;
                    }
                    break;
                case 4:
                    if (Game.CoveredDistancePercantage < 25)
                    {
                        delay = 1;
                    }
                    else if (Game.CoveredDistancePercantage < 40)
                    {
                        delay = 2;
                    }
                    else if (Game.CoveredDistancePercantage < 50)
                    {
                        delay = 3;
                    }
                    else if (Game.CoveredDistancePercantage < 65)
                    {
                        delay = 1;
                    }
                    else if (Game.CoveredDistancePercantage < 75)
                    {
                        delay = 2;
                    }
                    else if (Game.CoveredDistancePercantage < 90)
                    {
                        delay = 3;
                    }
                    else
                    {
                        delay = 4;
                    }
                    break;
                case 5:
                    if (Game.CoveredDistancePercantage < 25)
                    {
                        delay = 1;
                    }
                    else if (Game.CoveredDistancePercantage < 40)
                    {
                        delay = 2;
                    }
                    else if (Game.CoveredDistancePercantage < 50)
                    {
                        delay = 3;
                    }
                    else if (Game.CoveredDistancePercantage < 65)
                    {
                        delay = 1;
                    }
                    else if (Game.CoveredDistancePercantage < 75)
                    {
                        delay = 2;
                    }
                    else if (Game.CoveredDistancePercantage < 90)
                    {
                        delay = 3;
                    }
                    else
                    {
                        delay = 4;
                    }
                    break;
                case 6:
                    if (Game.CoveredDistancePercantage < 20)
                    {
                        delay = 1;
                    }
                    else if (Game.CoveredDistancePercantage < 40)
                    {
                        delay = 2;
                    }
                    else
                    {
                        delay = 3;
                    }
                    break;
                case 7:
                    if (Game.CoveredDistancePercantage < 20)
                    {
                        delay = 1;
                    }
                    else if (Game.CoveredDistancePercantage < 40)
                    {
                        delay = 2;
                    }
                    else
                    {
                        delay = 3;
                    }

                    break;
            }
            return delay;
        }
        #endregion

        #region Ammo creation

        private void TryCreateAmmoAsset()
        {
            if ((this.PicBoxGrid.AllCreatedLinesCount - this.AmmosGrid.LastAmmoRowNumber) > 5)
            {
                this.AmmosGrid.CreateAssetLine();
                this.AmmosGrid.LastAmmoRowNumber = this.PicBoxGrid.AllCreatedLinesCount;
            }

            int f = GetAsteroidFrequency();
            if ((this.PicBoxGrid.AllCreatedLinesCount - this.AsteroidsGrid.LastAsteroidRowNumber) > f)
            {
                this.AsteroidsGrid.CreateAssetLine();
                this.AsteroidsGrid.LastAsteroidRowNumber = this.PicBoxGrid.AllCreatedLinesCount;
            }
        }
        private int GetAsteroidFrequency()
        {
            if (Game.GameTopic.HierarchyLevel.LevelNo < 4)
            {
                return 2;
            }
            else if (Game.GameTopic.HierarchyLevel.LevelNo < 7)
            {
                return 3;
            }
            else if (Game.GameTopic.HierarchyLevel.LevelNo < 10)
            {
                return 4;
            }
            return 5;
        }
        #endregion

        #region collisions procession
        internal bool DetectBulletCollisionWithPictureBox()
        {
            BulletM bul = this.FindBulletCollidedWithPictureBox();
            if (bul != null)
            {
                this.StarShipControls.BulletCollidedWithPicBox(bul);
                return true;
            }
            return false;
        }
        internal bool DetectBulletCollisionWithAmmo()
        {
            BulletM bul = this.FindBulletCollidedWithAmmo();
            if (bul != null)
            {
                this.StarShipControls.BulletCollidedWithAmmo(bul);
                return true;
            }
            return false;
        }
        internal bool DetectBulletCollisionWithAsteroid()
        {
            BulletM bul = this.FindBulletCollidedWithAsteroid();
            if (bul != null)
            {
                this.StarShipControls.BulletCollidedWithAmmo(bul);
                return true;
            }
            return false;
        }
        internal void DetectStarShipCollisionWithAmmo()
        {
            AssetM ass = this.AmmosGrid.DetectCollisionWithStarShip(this.StarShipControls.Ship);
            if (ass != null)
            {
                this.AmmosGrid.DeleteAsset(ass);
                this.StarShipControls.ShipCollidedWithAmmo((AmmoM)ass);
                if (this.OnShipCollidedWithAmmo != null)
                {
                    this.OnShipCollidedWithAmmo(ass);
                }
            }
        }
        internal void DetectStarShipCollisionLaser()
        {
            foreach (LaserM las in this.LasersGrid.Assets)
            {
                if (this.StarShipControls.DetectCollisionWithLaser(las))
                {
                    this.LasersGrid.DeleteAsset(las);
                    return;
                }
            }
        }
        internal void DetectStarShipCollisionAsteroid()
        {
            foreach (AssetM ass in this.AsteroidsGrid.Assets)
            {
                if (this.StarShipControls.DetectCollisionWithAsteroid(ass))
                {
                    this.AsteroidsGrid.DeleteAsset(ass);
                    this.ExplosionsGrid.CreateExplosion(ass);
                    if (OnAssetHitted != null)
                    {
                        OnAssetHitted(ass);
                    }
                    if (OnShipHitted != null)
                    {
                        OnShipHitted();
                    }
                    return;
                }
            }
        }

        private BulletM FindBulletCollidedWithPictureBox()
        {
            lock (this.StarShipControls.Ship.FiredBullets)
            {
                foreach (BulletM bul in this.StarShipControls.Ship.FiredBullets)
                {
                    AssetM ass = this.PicBoxGrid.DetectCollisionWithBullet(bul);
                    if (ass != null)
                    {
                        if (this.TheWordToSpeak != null && ((PictureBoxM)ass).TextValue == this.TheWordToSpeak)
                        {
                            try
                            {
                                this.PicBoxGrid.DeleteAsset(ass);
                                this.ExplosionsGrid.CreateExplosion(ass);
                                if (this.OnPictureBoxHitted != null)
                                {
                                    this.OnPictureBoxHitted((PictureBoxM)ass);
                                }
                                foreach (AssetM p in PicBoxGrid.Assets)
                                {
                                    (p as PictureBoxM).IsThisPictureSpoken = false;
                                }
                                this.TheWordToSpeak = null;
                            }
                            catch (Exception ex)
                            {

                            }
                            return bul;
                        }
                    }
                }
            }
            return null;
        }
        private BulletM FindBulletCollidedWithAmmo()
        {
            lock (this.StarShipControls.Ship.FiredBullets)
            {
                foreach (BulletM bul in this.StarShipControls.Ship.FiredBullets)
                {
                    AssetM ass = this.AmmosGrid.DetectCollisionWithBullet(bul);
                    if (ass != null)
                    {
                        this.AmmosGrid.DeleteAsset(ass);
                        this.ExplosionsGrid.CreateExplosion(ass);
                        if (OnAssetHitted != null)
                        {
                            OnAssetHitted(ass);
                        }
                        return bul;

                    }
                }
            }
            return null;
        }
        private BulletM FindBulletCollidedWithAsteroid()
        {
            lock (this.StarShipControls.Ship.FiredBullets)
            {
                foreach (BulletM bul in this.StarShipControls.Ship.FiredBullets)
                {
                    AssetM ass = this.AsteroidsGrid.DetectCollisionWithBullet(bul);
                    if (ass != null)
                    {
                        this.AsteroidsGrid.DeleteAsset(ass);
                        this.ExplosionsGrid.CreateExplosion(ass);
                        if (OnAssetHitted != null)
                        {
                            OnAssetHitted(ass);
                        }
                        return bul;
                    }
                }
            }
            return null;
        }
        #endregion

        #region removing things out of scope

        internal PictureBoxM RemovePictureBoxesFromInvisibleSpace()
        {
            this.RemoveBulletsFromInvisibleSpace();
            AssetM pic = this.PicBoxGrid.RemoveAssetsFromInvisibleSpace();
            return (PictureBoxM)pic;
        }
        internal void RemoveAmmosFromInvisibleSpace()
        {
            this.RemoveBulletsFromInvisibleSpace();
            this.AmmosGrid.RemoveAssetsFromInvisibleSpace();
            this.AsteroidsGrid.RemoveAssetsFromInvisibleSpace();
        }
        private void RemoveBulletsFromInvisibleSpace()
        {
            List<BulletM> bulForDelete = new List<BulletM>();
            lock (this.StarShipControls.Ship.FiredBullets)
            {
                foreach (BulletM bul in this.StarShipControls.Ship.FiredBullets)
                {
                    if ((bul.StartPosition.Y + bul.PositionY) < 0)
                    {
                        bulForDelete.Add(bul);
                    }
                }
                foreach (BulletM bul in bulForDelete)
                {
                    this.StarShipControls.Ship.FiredBullets.Remove(bul);
                    bul.DeleteItself();
                    if (this.OnBulletRunOutOfSpace != null)
                    {
                        this.OnBulletRunOutOfSpace();
                    }
                }
            }
        }

        #endregion

        public void StartSalute()
        {
            AnimatedAssetSource aas = new AnimatedAssetSource() { PicNameStubWithoutExt = "ms-appx:///Images/Salutes/Salute1/Sal1_", PictureNumber = 30, FrameChangeTime = 50 };
            AnimatedAssetSource aas1 = new AnimatedAssetSource() { PicNameStubWithoutExt = "ms-appx:///Images/Salutes/Salute2/Sal2_", PictureNumber = 19, FrameChangeTime = 50 };
            AnimatedAssetSource aas2 = new AnimatedAssetSource() { PicNameStubWithoutExt = "ms-appx:///Images/Salutes/Salute3/Sal3_", PictureNumber = 25, FrameChangeTime = 50 };
            List<AnimatedAssetSource> sources = new List<AnimatedAssetSource>();
            sources.Add(aas);
            sources.Add(aas1);
            sources.Add(aas2);
            this.SmallSalutesGrid.Initialize(sources);
            if (this.OnSaluteStarted != null)
            {
                this.SmallSalutesGrid.StartSalute();
                this.OnSaluteStarted();
            }
        }

        #region Word to speak preaprations
        public PictureBoxM PrepareWordToSpeak()
        {
            this.DelayForPictureToSpeakShowTimer.Stop();
            PictureBoxM pic = null;
            if (this.PicBoxGrid.IsAssetsReached4)
            {
                pic = this.PicBoxGrid.GetRandomLastWord();
                if (pic == null)
                {
                    return pic;
                }
                this.TheWordToSpeak = pic.TextValue;
                if (this.TheWordToSpeak != null)
                {
                    this.WordPreparedDelayTimer.Start(pic);
                    return pic;
                }
            }
            else
            {
                if (!this.IsReached4Handled)
                {
                    this.IsReached4Handled = true;
                    this.PicBoxGrid.OnAssetsReached4 += PicBoxGrid_OnAssetsReached4;
                }
            }
            return pic;
        }

        private void PicBoxGrid_OnAssetsReached4()
        {
            this.PicBoxGrid.OnAssetsReached4 -= PicBoxGrid_OnAssetsReached4;
            this.PrepareWordToSpeak();
            this.IsReached4Handled = false;
        }

        private void WordPreparedDelayTimer_OnTickedStatefull(object state)
        {
            this.WordPreparedDelayTimer.Stop();
            if (this.OnTheWordPreparedForSpeak != null)
            {
                this.OnTheWordPreparedForSpeak(state as PictureBoxM);
            }
            this.DelayForPictureToSpeakShowTimer.Start();
        }

        private void DelayForPictureToSpeakShowTimer_OnTicked()
        {
            this.DelayForPictureToSpeakShowTimer.Stop();
            if (this.OnDelayEndedForPictureToSpeakShow != null)
            {
                this.OnDelayEndedForPictureToSpeakShow();
            }
        }

        #endregion

        private void PictureBoxes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.PicBoxGrid.Assets.Count == 0)
            {
                if (this.OnPictureBoxesRunOut != null)
                {
                    this.OnPictureBoxesRunOut();
                }
            }
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            this.PicBoxGrid.DeleteItself();
            this.ExplosionsGrid.DeleteItself();
            this.AmmosGrid.DeleteItself();
            this.AsteroidsGrid.DeleteItself();
            this.SmallSalutesGrid.DeleteItself();
            this.StarShipControls.DeleteItself();
        }
        public override void DetachEvents()
        {
            base.DetachEvents();
            SpaceParams.OnBlockSpeedChanged -= SpaceParams_OnBlockSpeedChanged;
            this.PicBoxGrid.OnAssetsReached4 -= PicBoxGrid_OnAssetsReached4;
            this.PicBoxGrid.Assets.CollectionChanged -= PictureBoxes_CollectionChanged;
            this.WordPreparedDelayTimer.OnTickedStatefull -= WordPreparedDelayTimer_OnTickedStatefull;
            this.DelayForPictureToSpeakShowTimer.OnTicked -= DelayForPictureToSpeakShowTimer_OnTicked;
        }
    }

    public delegate void TheWordPreparedForSpeak(PictureBoxM pic);
    public delegate void DelayEndedForPictureToSpeakShow();
    public delegate void PictureBoxHitted(PictureBoxM pic);
    public delegate void AssetHitted(AssetM ass);
    public delegate void ShipHitted();
    public delegate void BulletRunOutOfSpace();
    public delegate void PictureBoxesRunOut();
    public delegate void ShipCollidedWithAmmo(AssetM ass);
    public delegate void SaluteStarted();
    public delegate void BlocksSpeedChanged(double speed);
    public delegate void TopMarginChanged(int topMarg);

}