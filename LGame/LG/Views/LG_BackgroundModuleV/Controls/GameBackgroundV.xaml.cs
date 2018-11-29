using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using LG.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class GameBackgroundV : UserControl
    {
        public event ShouldBangStart OnShouldBangStart;
        AssetVM AvaImage;
        AssetVM AvaName;
        Size SpaceSize;
        private GameBackgroundVM GameBackVM { get; set; }

        public GameBackgroundV(GameBackgroundVM gameBackVM)
        {
            this.InitializeComponent();
            this.GameBackVM = gameBackVM;
            bigSpiralV.Initialize(gameBackVM.BigSpiralVModel);
            gameBackVM.OnAvatarStartShowBeginingVM += gameBackVM_OnAvatarShowBeginingVM;
            gameBackVM.OnAvatarFinishShowBeginingVM += gameBackVM_OnAvatarFinishShowBeginingVM;
            backSpaceView.Initialize(gameBackVM.BackSpaceGridVModel);
            backSpaceView.Margin = new Thickness(0, -gameBackVM.UpperStarCellsMargin, 0, 0);
            gameBackVM.OnUpperStarCellsMarginChanged += gameBackVM_OnUpperStarCellsMarginChanged;
        }

        void gameBackVM_OnUpperStarCellsMarginChanged(double m)
        {
            backSpaceView.Margin = new Thickness(0, -m, 0, 0);
        }

        async public Task ClearAvatarShow()
        {
            this.ucAvaShow.StopAndClear();
        }
        private void gameBackVM_OnAvatarShowBeginingVM(AssetVM avaImage, AssetVM avaName, Size spaceSize)
        {
            ucAvaShow.StartAvatarBeginShow(avaImage, avaName, spaceSize);
        }
        public void BeginAvatarShow()
        {
            ucAvaShow.StartAvatarBeginShow(AvaImage, AvaName, SpaceSize);
        }
        private void gameBackVM_OnAvatarFinishShowBeginingVM(AssetVM avImage, AssetVM avaName, Size spaceSize)
        {
            if (this.OnShouldBangStart != null)
            {
                OnShouldBangStart();
            }
            ucAvaShow.StartAvatarFinishShow(avImage, avaName, spaceSize);
        }

        private void AvaShow_AvatarShowEnded()
        {
            this.GameBackVM.AvatarShowEnded();
        }


    }

    public delegate void ShouldBangStart();

}
