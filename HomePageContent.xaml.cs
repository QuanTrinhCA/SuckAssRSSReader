using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        internal static event EventHandler<object> ListView_DoubleTappedEvent;
        internal static event EventHandler<object> ListView_SelectionChangedEvent;
        private static ObservableCollection<CustomFeed> _feeds;
        private static ObservableCollection<CustomFeed> Feeds => _feeds;
        public HomePageContent()
        {
            InitializeComponent();
            Loading += HomePageContent_ContentLoading;
            Unloaded += HomePageContent_Unloaded;
            MainPage.OpenLinkInBrowserEvent += OpenLinkInBrowser;
        }
        private async void HomePageContent_ContentLoading(FrameworkElement sender, object args)
        {
            _feeds = await SuckAssReader.GetFeeds();
            listView.ItemsSource = Feeds;
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
