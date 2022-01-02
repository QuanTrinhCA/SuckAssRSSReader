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
        public static event EventHandler<object> WebView_CanNotGoBackEvent;

        public WebViewPageContent()
        {
            InitializeComponent();
            Unloaded += WebViewPage_ContentUnloaded;
            MainPage.WebView_GoBackEvent += WebView_GoBack;
            MainPage.OpenLinkInBrowserEvent += OpenLinkInBrowser;
        }

        private void WebViewPage_ContentUnloaded(object sender, RoutedEventArgs e)
        {
            MainPage.WebView_GoBackEvent -= WebView_GoBack;
            MainPage.OpenLinkInBrowserEvent -= OpenLinkInBrowser;
        }
        private void WebView_GoBack(object sender, object e)
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
            }
            else
            {
                WebView_CanNotGoBackEvent(this, null);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            webView.Navigate(new Uri((e.Parameter as CustomFeed).Link));
        }
        private async void OpenLinkInBrowser(object sender, object e)
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
