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
            SuckAssReader.Initialize();
            HomePageContent.ListView_DoubleTappedEvent += NavigateToWebView;
            HomePageContent.ListView_SelectionChangedEvent += EnableOpenButtonOnSelectionChangedEvent;
            WebViewPageContent.WebView_CanNotGoBackEvent += FrameGoBack;
            backButton.Click += FireGoBackEvent;
            openButton.Click += FireOpenLinkInBrowserEvent;
            settingButton.Click += NavigateToSetting;
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
        private void NavigateToSetting(object sender, RoutedEventArgs e)
        {
            backButton.IsEnabled = true;
            openButton.IsEnabled = false;
            frame.Navigate(typeof(SettingPageContent));
        }
        private void FrameGoBack(object sender, object e)
        {
            frame.GoBack();
            if (frame.Content.GetType() == typeof(WebViewPageContent))
            {
                backButton.IsEnabled = true;
                openButton.IsEnabled = true;
            }
            else if (frame.Content.GetType() == typeof(HomePageContent))
            {
                backButton.IsEnabled = false;
                openButton.IsEnabled = true;
            }
        }
        private void FireGoBackEvent(object sender, RoutedEventArgs e)
        {
            if (frame.Content.GetType() == typeof(WebViewPageContent))
            {
                WebView_GoBackEvent(this, null);
            } 
            else if (frame.Content.GetType() == typeof(SettingPageContent))
            {
                FrameGoBack(this, null);
            }
        }
        private void FireOpenLinkInBrowserEvent(object sender, RoutedEventArgs e)
        {
            OpenLinkInBrowserEvent(this, frame.Content.GetType());
        }
        private void EnableOpenButtonOnSelectionChangedEvent(object sender, object e)
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
