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
using Windows.UI.Popups;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
#if DEBUG
using CurrentAppStore = Windows.ApplicationModel.Store.CurrentAppSimulator;
#else
using CurrentAppStore = Windows.ApplicationModel.Store.CurrentApp;
#endif
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace LG
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Windows.UI.Xaml.Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += App_Resuming;
            this.UnhandledException += App_UnhandledException;
        }

        private void DisplayPrivacyPolicy(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand privacyPolicyCommand = new SettingsCommand("privacyPolicy", "Privacy Policy", (uiCommand) => { LaunchPrivacyPolicyUrl(); });
            args.Request.ApplicationCommands.Add(privacyPolicyCommand);
        }

        async void LaunchPrivacyPolicyUrl()
        {
            Uri privacyPolicyUrl = new Uri("http://learnenglishgames.net/privacy.html");
            var result = await Windows.System.Launcher.LaunchUriAsync(privacyPolicyUrl);
        }


        void App_Resuming(object sender, object e)
        {
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
            //#if DEBUG
            // var proxyDataFolder = await Package.Current.InstalledLocation.GetFolderAsync("Licence"); 
            // var proxyFile = await proxyDataFolder.GetFileAsync("LicInfo.xml");
            // await CurrentAppStore.ReloadSimulatorAsync(proxyFile);

            //#endif

            // Add the main call to the privacy policy
            //SettingsPane.GetForCurrentView().CommandsRequested += DisplayPrivacyPolicy;
        }
        //public static async Task LoadAppSimulatorProxyFileAsync()
        //{
        //    // StorageFolder proxyDataFolder = await Package.Current.InstalledLocation.GetFolderAsync("License");

        //    StorageFile proxyFile = await ApplicationData.Current.LocalFolder.GetFileAsync("WindowsStoreProxy.xml");
        //    // Windows.ApplicationModel.Store.CurrentApp.r
        //    await CurrentAppStore.ReloadSimulatorAsync(proxyFile);
        //}
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
        private void OnActivated(object sender, IActivatedEventArgs e)
        {
            //var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            //deferral.Complete();
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageDialog msgdlg = new MessageDialog(e.Message, "Error");
            msgdlg.Commands.Add(new UICommand("Ok", null, null));

            IAsyncOperation<IUICommand> asyncOp = msgdlg.ShowAsync();
            asyncOp.Completed = OnMessageDialogShowAsyncCompleted;
        }
        void OnMessageDialogShowAsyncCompleted(IAsyncOperation<IUICommand> asyncInfo, AsyncStatus asyncStatus)
        {
            //
        }
    }
}
