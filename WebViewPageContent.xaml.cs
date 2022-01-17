using AppFeeds;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SuckAssRSSReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebViewPageContent : Page
    {
        public static event EventHandler WebViewCanNotGoBack;

        public static event EventHandler<bool> ChangeStateOfBackButton;

        public static event EventHandler<bool> ChangeStateOfOpenButton;

        public WebViewPageContent()
        {
            InitializeComponent();

            Loaded += SetupPage;
            Unloaded += ResetPage;

            MainPage.WebViewGoBack += WebViewGoBack;
            MainPage.OpenLinkInBrowser += OpenLinkInBrowserAsync;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            webView.Navigate(new Uri((e.Parameter as CustomFeedItem).Link));
        }

        private void SetupPage(object sender, RoutedEventArgs e)
        {
            ChangeStateOfBackButton(this, true);
            ChangeStateOfOpenButton(this, true);
        }

        private void ResetPage(object sender, RoutedEventArgs e)
        {
            MainPage.WebViewGoBack -= WebViewGoBack;
            MainPage.OpenLinkInBrowser -= OpenLinkInBrowserAsync;
        }

        private void WebViewGoBack(object sender, object e)
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
            }
            else
            {
                WebViewCanNotGoBack(this, null);
            }
        }

        private async void OpenLinkInBrowserAsync(object sender, object e)
        {
            if (GetType() == e as Type)
            {
                if (!await Windows.System.Launcher.LaunchUriAsync(webView.Source))
                {
                    await Windows.System.Launcher.LaunchUriAsync(webView.Source);
                }
            }
        }
    }
}