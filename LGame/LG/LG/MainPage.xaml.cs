using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using LG.Views;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LG
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
      
          
        }

      

        DispatcherTimer dt;

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            grid.Background = new SolidColorBrush(Color.FromArgb(255, 15, 15, 30));
            GameFrameV g = new GameFrameV();
            this.grid.Children.Add(g);
            g.Initialize();


            //dt = new DispatcherTimer();
            //dt.Interval = new TimeSpan(0, 0, 1);
            //dt.Tick += dt_Tick;
            //dt.Start();

        }

        void dt_Tick(object sender, object e)
        {
            dt.Stop();
            GameFrameV g = new GameFrameV();
            this.grid.Children.Add(g);
            g.Initialize();
        }
    }
}