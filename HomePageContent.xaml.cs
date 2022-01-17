using AppFeeds;
using System;
using System.Collections.Generic;
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
        public ObservableCollection<CustomFeedItem> Feeds = new ObservableCollection<CustomFeedItem>();

        public static event EventHandler<bool> ChangeStateOfBackButton;

        public static event EventHandler<bool> ChangeStateOfOpenButton;

        public static event EventHandler<object> ListViewDoubleTapped;

        private Timer _timer;

        public HomePageContent()
        {
            InitializeComponent();

            this.Loaded += SetUpPageAsync;

            MainPage.OpenLinkInBrowser += OpenLinkInBrowserAsync;
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
                AppFeeds.Feeds.GetSavedFeeds();
                GetFeedItemsAsync();
            });

            SetTimer();
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
            GetFeedItemsAsync();
        }

        private async void GetFeedItemsAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, async () =>
            {
                foreach (CustomFeedItem feedItem in await Task.Run(async () =>
                {
                    return await AppFeeds.Feeds.GetFeedItemsAsync(Feeds.ToList());
                }))
                {
                    Feeds.Insert(0, feedItem);
                }
            });
        }

        private async void OpenLinkInBrowserAsync(object sender, object e)
        {
            if (GetType() == e as Type)
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