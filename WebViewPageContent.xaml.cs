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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SuckAssRSSReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebViewPageContent : Page
    {
        internal static event EventHandler<object> WebViewCanNotGoBack;
        public WebViewPageContent()
        {
            InitializeComponent();
            MainPage.WebViewGoBack += WebViewGoBack;
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
                MainPage.WebViewGoBack -= WebViewGoBack;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            webView.Navigate(new Uri((e.Parameter as CustomFeed).Link));
        }
    }
}
