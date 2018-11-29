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
using LG.ViewModels;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class ProfileRegister : UserControl
    {
        public event ProfileRegisterClose OnProfileRegisterClose;

        ProfilesManagerVM ProfilesMgrVM { get; set; }
        RegisterMode Mode { get; set; }
        ProfileVM ProfVM { get; set; }

        public ProfileRegister(ProfilesManagerVM pmvm, bool isNew,RegisterMode mode)
        {
            this.InitializeComponent();
            this.ProfilesMgrVM = pmvm;
           
            this.Mode = mode;
            spPassword.Visibility = this.Mode == RegisterMode.Local ? Visibility.Collapsed : Visibility.Visible;
            spContacts.Visibility = this.Mode == RegisterMode.Local ? Visibility.Collapsed : Visibility.Visible;
            if (isNew)
            {
                this.ProfVM = pmvm.NewProfileVM;
                this.DataContext = this.ProfVM;
                imInter.Source = new BitmapImage(new Uri("ms-appx:///Images/NoPhoto.png"));
                btRegister.Content = "Register";
                btRegister.Click += btRegister_Click;
            }
            else
            {
                this.ProfVM = pmvm.EditedProfileVM;
                this.DataContext = this.ProfVM;
                btRegister.Content = "Update";
                btRegister.Click += btUpdate_Click;
            }
            this.DetermineEnable();
        }

        private async void Browse_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".jpg");

            // Get the selected file
            StorageFile storageFile = await picker.PickSingleFileAsync();
            if (storageFile == null)
                return;
            BitmapImage PictureSource = new BitmapImage();
            IRandomAccessStream stream = await storageFile.OpenReadAsync();
            this.ProfVM.PictureStream = stream;
            PictureSource.SetSource(stream);
            imInter.Source = PictureSource;
        }

        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
            btRegister.Click -= btRegister_Click;
            this.ProfilesMgrVM.Register();
            if (OnProfileRegisterClose != null)
            {
                OnProfileRegisterClose();
            }
        }
        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            btRegister.Click -= btUpdate_Click;
            this.ProfilesMgrVM.Update();
            if (OnProfileRegisterClose != null)
            {
                OnProfileRegisterClose();
            }
        }
        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbName.Text.Trim().Length > 21)
            {
                tbName.Text = tbName.Text.Trim().Remove(21);
            }
            this.DetermineEnable();
        }

        private void tbPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (tbPass.Password.Trim().Length > 8)
            {
                tbPass.Password = tbPass.Password.Trim().Remove(8);
            }
            this.DetermineEnable();
        }

        private void tbContacts_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbContacts.Text.Trim().Length > 54)
            {
                tbContacts.Text = tbContacts.Text.Trim().Remove(54);
            }
            this.DetermineEnable();
        }

        private void DetermineEnable()
        {
            switch (this.Mode)
            {
                case RegisterMode.Local:
                    btRegister.IsEnabled = tbName.Text.Trim().Length > 3;
                    break;
                case RegisterMode.Internet:
                    btRegister.IsEnabled = tbName.Text.Trim().Length > 3 && tbPass.Password.Trim().Length > 3;
                    break;
            }

        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            btRegister.Click -= btRegister_Click;
            btRegister.Click -= btUpdate_Click;
            if (OnProfileRegisterClose != null)
            {
                OnProfileRegisterClose();
            }
        }
    }

    public delegate void ProfileRegisterClose();
}
