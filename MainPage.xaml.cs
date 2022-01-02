using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SuckAssRSSReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static event EventHandler WebView_GoBackEvent;
        public static event EventHandler<object> OpenLinkInBrowserEvent;
        public MainPage()
        {
            InitializeComponent();
            HomePageContent.ListView_DoubleTappedEvent += NavigateToWebView;
            HomePageContent.ListView_SelectionChangedEvent += EnableOpenButton;
            WebViewPageContent.WebView_CanNotGoBackEvent += GoBackToHome;
            NavigateToHome();
        }
        private void NavigateToWebView(object sender, object e)
        {
            backButton.IsEnabled = true;
            openButton.IsEnabled = true;
            frame.Navigate(typeof(WebViewPageContent), e);
        }

        private void NavigateToHome()
        {
            backButton.IsEnabled = false;
            openButton.IsEnabled = false;
            frame.Navigate(typeof(HomePageContent));
        }
        private void GoBackToHome(object sender, object e)
        {
            backButton.IsEnabled = false;
            openButton.IsEnabled = true;
            frame.GoBack();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            WebView_GoBackEvent(this, null);
        }
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenLinkInBrowserEvent(this, frame.Content.GetType());
        }
        private void EnableOpenButton(object sender, object e)
        {
            if (e != null)
            {
                openButton.IsEnabled = true;
            }
            else
            {
                openButton.IsEnabled = false;
            }
        }
    }
}
