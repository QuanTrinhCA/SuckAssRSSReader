using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SuckAssRSSReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        internal static event EventHandler<object> WebViewGoBack;
        public MainPage()
        {
            InitializeComponent();
            HomePageContent.NavigateToWebView += NavigateToWebView;
            WebViewPageContent.WebViewCanNotGoBack += GoBackToHome;
            NavigateToHome();
        }

        private void NavigateToWebView(object sender, object e)
        {
            backButton.IsEnabled = true;
            frame.Navigate(typeof(WebViewPageContent), e);
        }

        private void NavigateToHome()
        {
            backButton.IsEnabled = false;
            frame.Navigate(typeof(HomePageContent));
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            WebViewGoBack(this, null);
        }
        private void GoBackToHome(object sender, object e)
        {
            backButton.IsEnabled = false;
            frame.GoBack();
        }
    }
}
