using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SuckAssRSSReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePageContent : Page
    {
        public static event EventHandler<object> ListView_DoubleTappedEvent;
        public static event EventHandler<object> ListView_SelectionChangedEvent;
        private static event EventHandler<object> GetFeeds;
        private static Timer Timer;
        public ObservableCollection<CustomFeed> Feeds = new ObservableCollection<CustomFeed>();
        public HomePageContent()
        {
            InitializeComponent();
            //listView.ItemsSource = Feeds;
            SetTimer();
            Loading += HomePageContent_ContentLoading;
            Unloaded += HomePageContent_Unloaded;
            GetFeeds += HomePageContent_GetFeeds;
            MainPage.OpenLinkInBrowserEvent += OpenLinkInBrowser;
        }
        private async void HomePageContent_GetFeeds(object sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                foreach (var item in await SuckAssReader.GetFeeds())
                {
                    Feeds.Insert(0, item);
                }
            });
        }
        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            Timer = new Timer(10000);
            // Hook up the Elapsed event for the timer. 
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }
        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            GetFeeds(null, null);
        }
        private void HomePageContent_ContentLoading(FrameworkElement sender, object args)
        {
            GetFeeds(null, null);
        }
        private void HomePageContent_Unloaded(object sender, RoutedEventArgs e)
        {
            MainPage.OpenLinkInBrowserEvent -= OpenLinkInBrowser;
        }
        private void ListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ListView_DoubleTappedEvent(this, listView.SelectedItem);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView_SelectionChangedEvent(this, listView.SelectedItem);
        }
        private async void OpenLinkInBrowser(object sender, object e)
        {
            if (GetType() == e as Type)
            {
                if (!await Windows.System.Launcher.LaunchUriAsync(new Uri((listView.SelectedItem as CustomFeed).Link)))
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri((listView.SelectedItem as CustomFeed).Link));
                }
            }
        }
    }
}
