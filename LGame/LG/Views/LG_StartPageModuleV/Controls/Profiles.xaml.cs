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
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using LG.ViewModels;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class Profiles : UserControl
    {
        private ProfilesManagerVM _profilesMgrVM;
        private ProfilesManagerVM ProfilesMgrVM
        {
            get
            {
                return _profilesMgrVM;
            }
        }

        public Profiles()
        {
            this.InitializeComponent();
        }

        public void Initialize(ProfilesManagerVM pmvm)
        {
            _profilesMgrVM = pmvm;
            this.DataContext = pmvm;
        }


        private void btLocal_Click(object sender, RoutedEventArgs e)
        {
            ProfilesMgrVM.StartingNewProfileRegister(RegisterMode.Local);
            ProfileRegister preg = new ProfileRegister(ProfilesMgrVM, true, RegisterMode.Local);
            preg.OnProfileRegisterClose += preg_OnProfileRegisterClose;
            spProfileForm.Children.Add(preg);
            btLocal.Visibility = Visibility.Collapsed;
            btInternet.Visibility = Visibility.Collapsed;
            btDelete.Visibility = Visibility.Collapsed;
            btEdit.Visibility = Visibility.Collapsed;
        }

        private void btInternet_Click(object sender, RoutedEventArgs e)
        {
            ProfilesMgrVM.StartingNewProfileRegister(RegisterMode.Internet);
            ProfileRegister preg = new ProfileRegister(ProfilesMgrVM, true, RegisterMode.Internet);
            preg.OnProfileRegisterClose += preg_OnProfileRegisterClose;
            spProfileForm.Children.Add(preg);
            btLocal.Visibility = Visibility.Collapsed;
            btInternet.Visibility = Visibility.Collapsed;
            btDelete.Visibility = Visibility.Collapsed;
            btEdit.Visibility = Visibility.Collapsed;
        }
        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
            tbProfile.Text = "Update profile:";

            ProfileRegister preg;
            if (ProfilesMgrVM.SelectedProfileVM.ProfileRegisterMode == RegisterMode.Local)
            {
                ProfilesMgrVM.StartingProfileEdit(RegisterMode.Local);
                preg = new ProfileRegister(ProfilesMgrVM, false, RegisterMode.Local);
            }
            else
            {
                ProfilesMgrVM.StartingProfileEdit(RegisterMode.Internet);
                preg = new ProfileRegister(ProfilesMgrVM, false, RegisterMode.Internet);
            }

            preg.OnProfileRegisterClose += preg_OnProfileRegisterClose;
            spProfileForm.Children.Add(preg);
            btLocal.Visibility = Visibility.Collapsed;
            btInternet.Visibility = Visibility.Collapsed;
            btDelete.Visibility = Visibility.Collapsed;
            btEdit.Visibility = Visibility.Collapsed;

        }

       async private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msgdlg = new MessageDialog("Delete profile?");
            msgdlg.Commands.Add(new UICommand("Ok", null, 1));
            msgdlg.Commands.Add(new UICommand("Cancel", null, 2));
            IUICommand com = await msgdlg.ShowAsync();
            if ((int)com.Id == 1)
            {
                this.ProfilesMgrVM.DeleteProfile();
            }
        }
        private void OnDeleteMessageDialogShowAsyncCompleted(IAsyncOperation<IUICommand> asyncInfo, AsyncStatus asyncStatus)
        {
            if ((int)asyncInfo.GetResults().Id == 1)
            {
                this.ProfilesMgrVM.DeleteProfile();
            }
        }
        private void preg_OnProfileRegisterClose()
        {
            tbProfile.Text = "Register new profile:";
            (spProfileForm.Children[0] as ProfileRegister).OnProfileRegisterClose -= preg_OnProfileRegisterClose;
            spProfileForm.Children.Clear();
            btLocal.Visibility = Visibility.Visible;
            btInternet.Visibility = Visibility.Visible;
            btDelete.Visibility = Visibility.Visible;
            btEdit.Visibility = Visibility.Visible;
            this.ProfilesMgrVM.CloseRegister();
        }



    }
}
