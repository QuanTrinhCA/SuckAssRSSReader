using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SuckAssRSSReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePageContent : Page
    {
        public static event EventHandler<object> ListViewDoubleTapped;
        public static event EventHandler<bool> ChangeStateOfOpenButton;
        public static event EventHandler<bool> ChangeStateOfBackButton;

        public ObservableCollection<CustomFeedItem> Feeds = new ObservableCollection<CustomFeedItem>();

        private Timer _timer;
        public HomePageContent()
        {
            InitializeComponent();

            Loaded += HomePageContent_Loaded;

            MainPage.OpenLinkInBrowser += OpenLinkInBrowser;

            SyncFeeds();
            SetTimer();
        }
        private async void SyncFeeds()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                foreach (CustomFeedItem feedItem in await SuckAssReader.GetFeedItems(Feeds.ToList()))
                {
                    Feeds.Insert(0, feedItem);
                }
            });
        }
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            _timer = new Timer(10000);
            // Hook up the Elapsed event for the timer. 
            _timer.Elapsed += Timer_OnTimed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
        private void Timer_OnTimed(object sender, ElapsedEventArgs e)
        {
            SyncFeeds();
        }
        private void ListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ListViewDoubleTapped(this, listView.SelectedItem);
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeStateOfOpenButton(this, true);
        }
        private void HomePageContent_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeStateOfBackButton(this, false);
            if (listView.SelectedItem == null)
            {
                ChangeStateOfOpenButton(this, false);
            }
            else
            {
                ChangeStateOfOpenButton(this, true);
            }
        }
        private async void OpenLinkInBrowser(object sender, object e)
        {
            if (GetType() == e as Type)
            {
                if (!await Windows.System.Launcher.LaunchUriAsync(new Uri((listView.SelectedItem as CustomFeedItem).Link)))
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri((listView.SelectedItem as CustomFeedItem).Link));
                }
            }
        }
    }
}
