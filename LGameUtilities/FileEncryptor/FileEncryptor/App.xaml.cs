using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
using Windows.Storage.BulkAccess;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.Web.AtomPub;
using Windows.ApplicationModel;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography.Certificates;
using Windows.Security.Cryptography.DataProtection;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace FileEncryptor
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
  
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();

            EncryptText();
          /// EncryptMedia();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
        string PublicKey = "L9abXpi3w2eu9ye6";
        async private void EncryptText()
        {
            StorageFolder sfInput = await ApplicationData.Current.LocalFolder.GetFolderAsync("Input");
            IReadOnlyList<StorageFolder> isfs = await sfInput.GetFoldersAsync();
            QueryOptions qopt = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string>() { ".xml"});

            StorageFolder sfOutput = await ApplicationData.Current.LocalFolder.GetFolderAsync("Output");

            foreach (StorageFolder isf in isfs)
            {
                StorageFolder osf = await sfOutput.CreateFolderAsync(isf.Name, CreationCollisionOption.OpenIfExists);
                StorageFileQueryResult r = isf.CreateFileQueryWithOptions(qopt);
                IReadOnlyList<StorageFile> iFiles = await r.GetFilesAsync();
                foreach (StorageFile iFile in iFiles)
                {
                    IRandomAccessStream ras = await iFile.OpenAsync(FileAccessMode.Read);
                    byte[] b = new byte[ras.Size];
                    IBuffer ibuf = await ras.ReadAsync(CryptographicBuffer.CreateFromByteArray(b), (uint)ras.Size, InputStreamOptions.None);
        
                    CryptographicBuffer.CopyToByteArray(ibuf, out b);
                    string s = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, ibuf);
                    IBuffer  buf= LG.Data.EncryptionProvider.Encrypt(s);
                    StorageFile fileEncrypted = await osf.CreateFileAsync(iFile.Name, CreationCollisionOption.ReplaceExisting);
                    IRandomAccessStream rasw = await fileEncrypted.OpenAsync(FileAccessMode.ReadWrite);
                    IOutputStream outs = rasw.GetOutputStreamAt(0);
                    await outs.WriteAsync(buf);
                    await rasw.FlushAsync();
                }
            }
        }
          async private void DecryptText()
        {
            StorageFolder sfOutput = await ApplicationData.Current.LocalFolder.GetFolderAsync("Output");
            IReadOnlyList<StorageFolder> isfs = await sfOutput.GetFoldersAsync();
            QueryOptions qopt = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string>() {".xml" });

            StorageFolder sfInput= await ApplicationData.Current.LocalFolder.GetFolderAsync("Input");

            foreach (StorageFolder osf in isfs)
            {
                StorageFolder isf = await sfInput.CreateFolderAsync(osf.Name, CreationCollisionOption.OpenIfExists);
                StorageFileQueryResult r = osf.CreateFileQueryWithOptions(qopt);
                IReadOnlyList<StorageFile> iFiles = await r.GetFilesAsync();
                foreach (StorageFile iFile in iFiles)
                {
                    IRandomAccessStream ras = await iFile.OpenAsync(FileAccessMode.Read);
                    byte[] b = new byte[ras.Size];
                    IBuffer ibuf = await ras.ReadAsync(CryptographicBuffer.CreateFromByteArray(b), (uint)ras.Size, InputStreamOptions.None);
                    ibuf = LG.Data.EncryptionProvider.Decrypt(ibuf);
                    StorageFile fileEncrypted = await isf.CreateFileAsync(iFile.Name, CreationCollisionOption.ReplaceExisting);
                    IRandomAccessStream rasw = await fileEncrypted.OpenAsync(FileAccessMode.ReadWrite);

                    await rasw.WriteAsync(ibuf);
                    rasw.Seek(0);
                    await rasw.FlushAsync();
                }
            }
        }
    
        async private void EncryptMedia()
        {
            StorageFolder sfInput = await ApplicationData.Current.LocalFolder.GetFolderAsync("Input");
            IReadOnlyList<StorageFolder> isfs = await sfInput.GetFoldersAsync();
            QueryOptions qopt = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string>() { ".jpg", ".mp3" });

            StorageFolder sfOutput = await ApplicationData.Current.LocalFolder.GetFolderAsync("Output");

            foreach (StorageFolder isf in isfs)
            {
                StorageFolder osf = await sfOutput.CreateFolderAsync(isf.Name, CreationCollisionOption.OpenIfExists);
                StorageFileQueryResult r = isf.CreateFileQueryWithOptions(qopt);
                IReadOnlyList<StorageFile> iFiles = await r.GetFilesAsync();
                foreach (StorageFile iFile in iFiles)
                {
                    IRandomAccessStream ras = await iFile.OpenAsync(FileAccessMode.Read);
                    byte[] b = new byte[ras.Size];
                    IBuffer ibuf = await ras.ReadAsync(CryptographicBuffer.CreateFromByteArray(b), (uint)ras.Size, InputStreamOptions.None);
                    ibuf = LG.Data.EncryptionProvider.Encrypt(ibuf);
                    StorageFile fileEncrypted = await osf.CreateFileAsync(iFile.Name, CreationCollisionOption.ReplaceExisting);
                    IRandomAccessStream rasw = await fileEncrypted.OpenAsync(FileAccessMode.ReadWrite);

                    await rasw.WriteAsync(ibuf);
                    rasw.Seek(0);
                    await rasw.FlushAsync();
                }
            }
        }
        async private void DecryptMedia()
        {
            StorageFolder sfOutput = await ApplicationData.Current.LocalFolder.GetFolderAsync("Output");
            IReadOnlyList<StorageFolder> isfs = await sfOutput.GetFoldersAsync();
            QueryOptions qopt = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string>() {".jpg", ".mp3" });

            StorageFolder sfInput= await ApplicationData.Current.LocalFolder.GetFolderAsync("Input");

            foreach (StorageFolder osf in isfs)
            {
                StorageFolder isf = await sfInput.CreateFolderAsync(osf.Name, CreationCollisionOption.OpenIfExists);
                StorageFileQueryResult r = osf.CreateFileQueryWithOptions(qopt);
                IReadOnlyList<StorageFile> iFiles = await r.GetFilesAsync();
                foreach (StorageFile iFile in iFiles)
                {
                    IRandomAccessStream ras = await iFile.OpenAsync(FileAccessMode.Read);
                    byte[] b = new byte[ras.Size];
                    IBuffer ibuf = await ras.ReadAsync(CryptographicBuffer.CreateFromByteArray(b), (uint)ras.Size, InputStreamOptions.None);
                    ibuf = LG.Data.EncryptionProvider.Decrypt(ibuf);
                    StorageFile fileEncrypted = await isf.CreateFileAsync(iFile.Name, CreationCollisionOption.ReplaceExisting);
                    IRandomAccessStream rasw = await fileEncrypted.OpenAsync(FileAccessMode.ReadWrite);

                    await rasw.WriteAsync(ibuf);
                    rasw.Seek(0);
                    await rasw.FlushAsync();
                }
            }
        }
    }
}
