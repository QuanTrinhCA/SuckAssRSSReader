using CustomObjects;
using Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
        public static event EventHandler<bool> ChangeStateOfBackButton;

        public static event EventHandler<bool> ChangeStateOfOpenButton;

        public static event EventHandler<object> ListViewDoubleTapped;

        public ObservableCollection<CustomFeedItem> FeedItems;

        private Timer _timer;

        public HomePageContent()
        {
            InitializeComponent();

            Loaded += SetUpPageAsync;

            MainPage.OpenLinkInBrowser += OpenLinkInBrowserAsync;

            listView.DoubleTapped += ListView_DoubleTapped;
            listView.SelectionChanged += ListView_SelectionChanged;

            FeedItems = new ObservableCollection<CustomFeedItem>();
        }

        private async void SetUpPageAsync(object sender, RoutedEventArgs e)
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
            await Task.Run(() =>
            {
                Feeds.GetSavedFeeds();
                GetFeedItemsAsync();
            });

            SetTimer();

            GC.Collect(1);
        }

        private void SetTimer()
        {
            _timer = new Timer
            {
                Interval = Sync.GetSyncInterval(),
                AutoReset = true,
                Enabled = true
            };
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GetFeedItemsAsync();
        }

        private async void GetFeedItemsAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, async () =>
            {
                foreach (CustomFeedItem feedItem in await Task.Run(async () => { return await Feeds.GetFeedItemsAsync(FeedItems.ToList()); }))
                {
                    FeedItems.Insert(0, feedItem);
                }
            });
            GC.Collect(1);
        }

        private async void OpenLinkInBrowserAsync(object sender, Type e)
        {
            if (GetType() == e)
            {
                if (!await Windows.System.Launcher.LaunchUriAsync(new Uri((listView.SelectedItem as CustomFeedItem).Link)))
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri((listView.SelectedItem as CustomFeedItem).Link));
                }
            }
        }

        private void ListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ListViewDoubleTapped(this, listView.SelectedItem);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeStateOfOpenButton(this, true);
        }
    }
}