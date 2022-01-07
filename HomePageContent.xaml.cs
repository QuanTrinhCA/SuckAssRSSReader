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
        public static event EventHandler<object> ListView_DoubleTappedEvent;
        public static event EventHandler<object> ListView_SelectionChangedEvent;
        private static event EventHandler GetFeedsEvent;
        private Timer _timer;
        public ObservableCollection<CustomFeedItem> Feeds = new ObservableCollection<CustomFeedItem>();
        public HomePageContent()
        {
            InitializeComponent();

            GetFeedsEvent += HomePageContent_GetFeedsEvent;
            MainPage.OpenLinkInBrowserEvent += OpenLinkInBrowser;

            GetFeedsEvent(this, null);
            SetTimer();
        }
        private async void HomePageContent_GetFeedsEvent(object sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                foreach (var item in await SuckAssReader.GetFeedItems(Feeds.ToList()))
                {
                    Feeds.Insert(0, item);
                }
            });
        }
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            _timer = new Timer(10000);
            // Hook up the Elapsed event for the timer. 
            _timer.Elapsed += Timer_OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
        private static void Timer_OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            GetFeedsEvent(sender, null);
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
                if (!await Windows.System.Launcher.LaunchUriAsync(new Uri((listView.SelectedItem as CustomFeedItem).Link)))
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri((listView.SelectedItem as CustomFeedItem).Link));
                }
            }
        }
    }
}
