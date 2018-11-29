using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Graphics.Display;
using LG.ViewModels;
#if DEBUG
using CurrentAppStore = Windows.ApplicationModel.Store.CurrentAppSimulator;
#else
using CurrentAppStore = Windows.ApplicationModel.Store.CurrentApp;
#endif
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public partial class GameFrameV : UserControl
    {
        private GameFrameVM _gameFrameVModel;
        private GameFrameVM GameFrameVModel
        {
            get
            {
                if (_gameFrameVModel == null)
                {
                    _gameFrameVModel = new GameFrameVM();
                }
                return _gameFrameVModel;
            }
        }
        private double previousMs = 0.0;
        private double remainder = 0.0;
        private bool FirstTimeRender = true;
        private double FirstTimeRenderTimeMark = 0.0;
        IRandomAccessStream ras;
        public GameFrameV()
        {
            this.InitializeComponent();
            GameFrameVModel.OnMessageShowVM += GameFrameVModel_OnMessageShowVM;
            GameFrameVModel.OnProgressShowStartVM += GameFrameVModel_OnProgressShowStartVM;
            GameFrameVModel.OnProgressShowEndVM += GameFrameVModel_OnProgressShowEndVM;
            GameFrameVModel.PreInitialize((int)Window.Current.Bounds.Width, (int)Window.Current.Bounds.Height, DisplayProperties.CurrentOrientation);
            this.Loaded += GameFrameV_Loaded;
            GameFrameVModel.ChildModulesVM.CollectionChanged += ChildModulesVM_CollectionChanged;
            GameFrameVModel.BackgroundModulesVM.CollectionChanged += BackgroundModulesVM_CollectionChanged;
            GameFrameVModel.Initialize();
            this.DataContext = GameFrameVModel;
            this.InitMedia();
        }

        public void Initialize()
        {
            AppSoundsV.Initialize(GameFrameVModel.AppSoundsVModel);
        }

        private void GameFrameV_Loaded(object sender, RoutedEventArgs e)
        {
           // int c = CurrentAppStore.LicenseInformation.ProductLicenses.Count;
            //if (!CurrentAppStore.LicenseInformation.ProductLicenses["Theme5_en"].IsActive)
            //{
            //    Windows.ApplicationModel.Store.CurrentAppSimulator.LicenseInformation.LicenseChanged += LicenseInformation_LicenseChanged;
            //    //try
            //    //{
            //        string w = await Windows.ApplicationModel.Store.CurrentAppSimulator.RequestAppPurchaseAsync(true);
            //    //}
            //    //catch (Exception e2)
            //    //{

            //    //}
            //    try
            //    {
            //        string a = await Windows.ApplicationModel.Store.CurrentAppSimulator.RequestProductPurchaseAsync("Theme5_en", true);
            //    }
            //    catch (Exception e1)
            //    {
            //        MessageDialog msgdlg = new MessageDialog(e1.Message, "Error");
            //        msgdlg.Commands.Add(new UICommand("Ok", null, null));

            //        IAsyncOperation<IUICommand> asyncOp = msgdlg.ShowAsync();
            //    }
            //}
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            DisplayProperties.AutoRotationPreferences = DisplayOrientations.Landscape;
            this.SizeChanged += GameFrameV_SizeChanged;
        }

        private void LicenseInformation_LicenseChanged()
        {

        }

        private void GameFrameV_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.GameFrameVModel.DisplayOrientationChanged(new Size(Window.Current.Bounds.Width, Window.Current.Bounds.Height), DisplayProperties.CurrentOrientation);
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            RenderingEventArgs renderArgs = e as RenderingEventArgs;
            if (this.FirstTimeRender)
            {
                this.FirstTimeRender = false;
                this.FirstTimeRenderTimeMark = renderArgs.RenderingTime.TotalMilliseconds;
            }
            double ms = renderArgs.RenderingTime.TotalMilliseconds - this.FirstTimeRenderTimeMark;
            double r = ms - previousMs + remainder;
            int tInt = (int)(r / 20);
            remainder = r - 20 * tInt;
            double d = ms - previousMs;
            this.GameFrameVModel.TimeElapses(tInt * 20, d);
            previousMs = ms;
        }
        async private void ChildModulesVM_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ModuleVM mod = (ModuleVM)e.NewItems[0];
                if (mod is StartPageVM)
                {
                    StartPageV spv = new StartPageV(mod as StartPageVM);
                    this.AddModule(spv);
                }
                else if (mod is ThemesVM)
                {
                    ThemesV tv = new ThemesV(mod as ThemesVM);
                    tv.OnShouldBeRemoved += ModuleV_OnShouldBeRemoved;
                    this.AddModule(tv);
                }
                else if (mod is GameVM)
                {
                    // ((GameBackgroundV)this.gridBackFrame.Children[0]).PlayBang();
                    if (this.BangPlayer.CurrentState == MediaElementState.Closed)
                    {
                        await this.InitMedia();
                        this.BangPlayer.MediaOpened += BangPlayer_MediaOpened;
                    }
                    else
                    {
                        this.PlayBang();
                    }
                    GameSpaceV gv = new GameSpaceV(mod as GameVM);
                    gv.OnShouldBeRemoved += ModuleV_OnShouldBeRemoved;
                    this.AddModule(gv);
                }
                else if (mod is HierarchyVM)
                {
                    HierarchyV hv = new HierarchyV(mod as HierarchyVM);
                    hv.OnShouldBeRemoved += ModuleV_OnShouldBeRemoved;
                    this.AddModule(hv);
                    vbGobalInfoTop.Visibility = Visibility.Visible;
                    vbGobalInfoBottom.Visibility = Visibility.Collapsed;
                }
            }
        }

        void BangPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            this.BangPlayer.MediaOpened -= BangPlayer_MediaOpened;
            this.PlayBang();
        }

    

        private void BackgroundModulesVM_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ModuleVM mod = (ModuleVM)e.NewItems[0];
                GameBackgroundV gbv = new GameBackgroundV(mod as GameBackgroundVM);
                this.gridBackFrame.Children.Insert(0, gbv);
                gbv.OnShouldBangStart += gbv_OnShouldBangStart;
            }
        }

        private void gbv_OnShouldBangStart()
        {
            this.PlayBang();
        }

        private void ModuleV_OnShouldBeRemoved(UIElement sender)
        {
            ((IModelV)sender).OnShouldBeRemoved -= ModuleV_OnShouldBeRemoved;
            if (sender is GameSpaceV)
            {
                (this.gridBackFrame.Children[0] as GameBackgroundV).ClearAvatarShow();
                BangPlayer.Stop();
            }
            this.RemoveModule((UserControl)sender);
            if (sender is HierarchyV)
            {
                vbGobalInfoTop.Visibility = Visibility.Collapsed;
                vbGobalInfoBottom.Visibility = Visibility.Visible;
            }
        }
        async private Task InitMedia()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Sounds/BangDramatic.mp3"));
            ras = await file.OpenReadAsync();
            this.BangPlayer.SetSource(ras, "audio/mpeg");
        }
        public void PlayBang()
        {
            BangPlayer.Play();
        }
        private void AddModule(UserControl mod)
        {
            GC.Collect();
            if (this.gridFrame.Children.Count > 0)
            {
                ((UserControl)this.gridFrame.Children[this.gridFrame.Children.Count - 1]).IsEnabled = false;
            }
            btFocusTaker.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            Storyboard stor = new Storyboard();

            if (this.gridFrame.Children.Count > 0)
            {
                DoubleAnimation daColapse = new DoubleAnimation()
                {
                    From = 0,
                    To = -Window.Current.Bounds.Width,
                    AutoReverse = false,
                    Duration = new TimeSpan(0, 0, 0, 0, 500)
                };

                Storyboard.SetTarget(daColapse, (TranslateTransform)this.gridFrame.Children[this.gridFrame.Children.Count - 1].RenderTransform);
                Storyboard.SetTargetProperty(daColapse, "X");
                stor.Children.Add(daColapse);
            }

            TranslateTransform tt = new TranslateTransform();
            tt.X = Window.Current.Bounds.Width;
            mod.RenderTransform = tt;
            DoubleAnimation daExpand = new DoubleAnimation()
            {
                From = tt.X,
                To = 0,
                AutoReverse = false,
                Duration = new TimeSpan(0, 0, 0, 0, 500)
            };
            Storyboard.SetTarget(daExpand, tt);
            Storyboard.SetTargetProperty(daExpand, "X");
            stor.Children.Add(daExpand);

            gridFrame.Children.Add(mod);
            btBack.IsEnabled = gridFrame.Children.Count > 1;
            stor.Begin();
        }

        private void RemoveModule(UserControl mod)
        {
            ((UserControl)this.gridFrame.Children[this.gridFrame.Children.Count - 2]).IsEnabled = true;

            Storyboard storyboardColapse = new Storyboard();

            DoubleAnimation daColapse = new DoubleAnimation()
            {
                From = 0,
                To = (int)Window.Current.Bounds.Width,
                AutoReverse = false,
                Duration = new TimeSpan(0, 0, 0, 0, 500)
            };

            Storyboard.SetTarget(daColapse, (TranslateTransform)this.gridFrame.Children[this.gridFrame.Children.Count - 1].RenderTransform);
            Storyboard.SetTargetProperty(daColapse, "X");
            storyboardColapse.Children.Add(daColapse);
            storyboardColapse.Completed += storyboardColapse_Completed;

            if (this.gridFrame.Children.Count > 1)
            {
                DoubleAnimation daExpand = new DoubleAnimation()
                {
                    From = -Window.Current.Bounds.Width,
                    To = 0,
                    AutoReverse = false,
                    Duration = new TimeSpan(0, 0, 0, 0, 500)
                };

                Storyboard.SetTarget(daExpand, (TranslateTransform)this.gridFrame.Children[this.gridFrame.Children.Count - 2].RenderTransform);
                Storyboard.SetTargetProperty(daExpand, "X");
                storyboardColapse.Children.Add(daExpand);
            }
            storyboardColapse.Begin();
        }

        private void storyboardColapse_Completed(object sender, object e)
        {
            this.gridFrame.Children.Remove(this.gridFrame.Children[this.gridFrame.Children.Count - 1]);
            btBack.IsEnabled = gridFrame.Children.Count > 1;
            GC.Collect();
        }

        private void btBack_Clicked(object sender, RoutedEventArgs e)
        {
            this.GameFrameVModel.NavigateBack();
        }

        private void GameFrameVModel_OnMessageShowVM(string mes)
        {
            MessageDialog msgdlg = new MessageDialog(mes, "Error");
            msgdlg.Commands.Add(new UICommand("Ok", null, null));
            msgdlg.ShowAsync();
        }
        private void GameFrameVModel_OnProgressShowStartVM(string mes)
        {
            gridProgress.Opacity = 1;
            gridProgress.IsHitTestVisible = true;
            tb.Text = mes;
            tb.Visibility = Windows.UI.Xaml.Visibility.Visible;
            pb.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        void GameFrameVModel_OnProgressShowEndVM()
        {
            gridProgress.Opacity = 0;
            gridProgress.IsHitTestVisible = false;
            tb.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            pb.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void gridProgress_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //e.Handled = true;
        }

    }
}
