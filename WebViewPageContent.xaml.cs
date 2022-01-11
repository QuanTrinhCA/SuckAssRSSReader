using AppFeedReader;
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
        public WebViewPageContent()
        {
            InitializeComponent();

            Loaded += WebViewPageContent_Loaded;
            Unloaded += WebView_ContentUnloaded;

            MainPage.GoBack += GoBack;
            MainPage.OpenLinkInBrowser += OpenLinkInBrowser;
        }

        public static event EventHandler CanNotGoBack;

        public static event EventHandler<bool> ChangeStateOfBackButton;

        public static event EventHandler<bool> ChangeStateOfOpenButton;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            webView.Navigate(new Uri((e.Parameter as CustomFeedItem).Link));
        }

        private void GoBack(object sender, object e)
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
            }
            else
            {
                CanNotGoBack(this, null);
            }
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

        private void WebView_ContentUnloaded(object sender, RoutedEventArgs e)
        {
            MainPage.GoBack -= GoBack;
            MainPage.OpenLinkInBrowser -= OpenLinkInBrowser;
        }

        private void WebViewPageContent_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeStateOfBackButton(this, true);
            ChangeStateOfOpenButton(this, true);
        }
    }
}