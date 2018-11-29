using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using LG.ViewModels;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class StartPageV : UserControl, IModelV
    {
        public event ShouldBeRemoved OnShouldBeRemoved;

        #region View Models
        private readonly ModuleVM _moduleVModel;

        private StartPageVM StartPageVModel
        {
            get
            {
                return (StartPageVM)this._moduleVModel;
            }
        }
        #endregion

        public StartPageV(StartPageVM startPageVM)
        {
            this.InitializeComponent();
            this._moduleVModel = startPageVM;
            this.DataContext = startPageVM;
            this.Loaded += StartPageV_Loaded;
            this.StartPageVModel.OnItselfDeleted += StartPageVModel_OnItselfDeleted;
            ProfilesC.Initialize(startPageVM.ProfilesMgrVM);
        }

        private void StartPageVModel_OnItselfDeleted(UnitVM uvm)
        {
            if (this.OnShouldBeRemoved != null)
            {
                this.OnShouldBeRemoved(this);
            }
        }
      
        private void StartPageV_Loaded(object sender, RoutedEventArgs e)
        {
            imPlay.Initialize("Play.png", "PlayPressed.png", "PlayOver.png");
            imSelectTheme.Initialize("SelectTheme.png", "SelectThemePressed.png", "SelectThemeOver.png");
            imHierarchy.Initialize("Hierarchy.png", "Hierarchy.png", "HierarchyOver.png");
        }
        
        private void imPlay_OnClick(ImageButton sender)
        {
            StartPageVModel.NavigateTo(NavigationCommand.Play);
        }

        private void imSelectTheme_OnClick(ImageButton sender)
        {

            StartPageVModel.NavigateTo(NavigationCommand.SelectTheme);
        }
        private void imHierarchy_OnClick(ImageButton sender)
        {
            StartPageVModel.NavigateTo(NavigationCommand.Hierarchy);
        }

    
    }
}
