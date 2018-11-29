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
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using LG.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class GameSpaceV : UserControl, IModelV
    {
        public event ShouldBeRemoved OnShouldBeRemoved;

        #region view-model
        private GameVM _gameVModel;
        private GameVM GameVModel
        {
            get
            {
                return _gameVModel;
            }
        }
        #endregion

        public GameSpaceV(GameVM gameVM)
        {
            this.InitializeComponent();

            this._gameVModel = gameVM;
            this.DataContext = this.GameVModel;
            this.ucGameSounds.Initialize(gameVM.GameSoundsVModel);

            ucSpaceView.Initialize(this.GameVModel.SpaceVModel, true);
            ucSpaceViewMini.Initialize(this.GameVModel.SpaceVModel, false);

            this.ucShipControls.Initialize(this.GameVModel.SpaceVModel.StarShipControlsVM);

            this.GameVModel.SpaceVModel.OnAssetExploded += SpaceVModel_OnAssetExploded;
            this.GameVModel.SpaceVModel.StarShipControlsVM.OnPlayChargerEmptySound += StarShipControlsVM_OnPlayChargerEmptySound;
            this.GameVModel.SpaceVModel.StarShipControlsVM.OnShipJittered += StarShipControlsVM_OnShipJittered;
            this.GameVModel.SpaceVModel.OnAmmoCatched += SpaceVModel_OnAmmoCatched;
            this.GameVModel.SpaceVModel.StarShipControlsVM.ShipVM.FiredBulletVMs.CollectionChanged += FiredBulletVMs_CollectionChanged;
            this.GameVModel.SpaceVModel.LasersVMs.CollectionChanged += LasersVMs_CollectionChanged;
            this.GameVModel.SpaceVModel.OnSaluteStarted += SpaceVModel_OnSaluteStarted;
            this.GameVModel.OnShipExploded += GameVModel_OnShipExploded;
            this.GameVModel.OnPlayGuilefulLaugh += GameVModel_OnPlayGuilefulLaugh;
            this.GameVModel.OnStartPlayingBackMusicAndEnginesVM += GameVModel_OnStartPlayingBackMusinAndEnginesVM;

            this.GameVModel.OnItselfDeleted += GameSpaceV_OnItselfDeleted;
            this.GameVModel.SpaceMapVModel.AdvancerVM.OnYChanged += AdvancerVM_OnYChanged;

            this.Loaded += GameSpaceV_Loaded;
        }

        private void GameVModel_OnPlayGuilefulLaugh()
        {
            this.ucGameSounds.PlaySmeh();
            this.ucGameSounds.StopBackMusic();
            this.ucGameSounds.StopEngineMusic();
        }

        private void GameVModel_OnStartPlayingBackMusinAndEnginesVM()
        {
            ucGameSounds.PlayBackMusic();
            ucGameSounds.PlayEngines();
        }

        private void SpaceVModel_OnSaluteStarted()
        {
            // this.ucGameSounds.StopBack
            this.ucGameSounds.PlaySalute();
        }

        private void AdvancerVM_OnYChanged(double y)
        {
            double h = this.GameVModel.SpaceMapVModel.AdvancerVM.StartPosition.Y + y;
            if (h >= 0)
            {
                ArrowOverlay.Height = h;
            }
        }

        private void GameSpaceV_Loaded(object sender, RoutedEventArgs e)
        {
            infoBoardV.Initialize(this.GameVModel);
            CompositionTarget.Rendering += OnCompositionTargetRendering;
        }

        private uint renderCount = 0;

        private void OnCompositionTargetRendering(object sender, object args)
        {
            renderCount++;
            if (renderCount % 2 == 0)
            {
                // Calculate t from 0 to 1 repetitively
                RenderingEventArgs renderingArgs = args as RenderingEventArgs;
                double t = (0.25 * renderingArgs.RenderingTime.TotalSeconds) % 1;
                for (int index = arrBorderGradientBrush.GradientStops.Count - 1; index >= 0; index--)
                    arrBorderGradientBrush.GradientStops[index].Offset = index / 7.0 - t;

                for (int index = 0; index < arrGradientBrush.GradientStops.Count; index++)
                    arrGradientBrush.GradientStops[index].Offset = index / 7.0 - t;

                for (int index = 0; index < theSunGradientBrush.GradientStops.Count; index++)
                    theSunGradientBrush.GradientStops[index].Offset = index / 7.0 - t;
            }
        }

        private void SpaceVModel_OnAssetExploded()
        {
            this.ucGameSounds.PlayExplosion();
        }

        private void StarShipControlsVM_OnShipJittered()
        {
            this.ucGameSounds.PlayVibration();
        }

        private void GameVModel_OnShipExploded()
        {
            this.ucGameSounds.PlayExplosion();
            ucSpaceView.DeleteShip();
        }

        private void StarShipControlsVM_OnPlayChargerEmptySound()
        {
            this.ucGameSounds.PlayChargerEmpty();
        }

        private void SpaceVModel_OnAmmoCatched(AmmoVM avm)
        {
            ucGameSounds.PlayRecharge();
        }

        private void FiredBulletVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ucGameSounds.PlayShoot();
            }
        }

        private void LasersVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ucGameSounds.PlayShoot();
            }
        }

        private void GameSpaceV_OnItselfDeleted(UnitVM uvm)
        {
            DetachEvents();
            if (this.OnShouldBeRemoved != null)
            {
                this.OnShouldBeRemoved(this);
            }
            mainGrid.Children.Clear();
            this.DataContext = null;
            _gameVModel = null;
        }

        private void DetachEvents()
        {
            CompositionTarget.Rendering -= OnCompositionTargetRendering;

            this.GameVModel.SpaceVModel.OnAssetExploded -= SpaceVModel_OnAssetExploded;
            this.GameVModel.SpaceVModel.StarShipControlsVM.OnPlayChargerEmptySound -= StarShipControlsVM_OnPlayChargerEmptySound;
            this.GameVModel.SpaceVModel.StarShipControlsVM.OnShipJittered -= StarShipControlsVM_OnShipJittered;
            this.GameVModel.SpaceVModel.OnAmmoCatched -= SpaceVModel_OnAmmoCatched;
            this.GameVModel.SpaceVModel.StarShipControlsVM.ShipVM.FiredBulletVMs.CollectionChanged -= FiredBulletVMs_CollectionChanged;
            this.GameVModel.SpaceVModel.LasersVMs.CollectionChanged -= LasersVMs_CollectionChanged;
            this.GameVModel.SpaceVModel.OnSaluteStarted -= SpaceVModel_OnSaluteStarted;
            this.GameVModel.OnShipExploded -= GameVModel_OnShipExploded;
            this.GameVModel.OnPlayGuilefulLaugh -= GameVModel_OnPlayGuilefulLaugh;
            this.GameVModel.OnStartPlayingBackMusicAndEnginesVM -= GameVModel_OnStartPlayingBackMusinAndEnginesVM;
            this.GameVModel.OnItselfDeleted -= GameSpaceV_OnItselfDeleted;
            this.GameVModel.SpaceMapVModel.AdvancerVM.OnYChanged -= AdvancerVM_OnYChanged;
        }
    }
}

