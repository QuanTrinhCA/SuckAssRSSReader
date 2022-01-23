using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SuckAssRSSReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static event EventHandler WebViewGoBack;

        public static event EventHandler<Type> OpenLinkInBrowser;

        public MainPage()
        {
            InitializeComponent();

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;

            Window.Current.Activated += App_Activated;

            HomePageContent.ListViewDoubleTapped += NavigateToWebView;
            HomePageContent.ChangeStateOfOpenButton += ChangeStateOfOpenButton;
            HomePageContent.ChangeStateOfBackButton += ChangeStateOfBackButton;

            WebViewPageContent.WebViewCanNotGoBack += FrameGoBack;
            WebViewPageContent.ChangeStateOfOpenButton += ChangeStateOfOpenButton;
            WebViewPageContent.ChangeStateOfBackButton += ChangeStateOfBackButton;

            SettingPageContent.ChangeStateOfOpenButton += ChangeStateOfOpenButton;
            SettingPageContent.ChangeStateOfBackButton += ChangeStateOfBackButton;

            backButton.Click += GoBack;
            openButton.Click += OpenLink;
            settingButton.Click += NavigateToSetting;

            NavigateToHome();
        }

        private void App_Activated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                appTitleTextBlock.Foreground = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                appTitleTextBlock.ClearValue(ForegroundProperty);
            }
        }

        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            appTitleTextBlock.Height = sender.Height;
        }

        private void ChangeStateOfBackButton(object sender, bool e)
        {
            if (e)
            {
                backButton.IsEnabled = true;
            }
            else
            {
                backButton.IsEnabled = false;
            }
        }

        private void ChangeStateOfOpenButton(object sender, bool e)
        {
            if (e)
            {
                openButton.IsEnabled = true;
            }
            else
            {
                openButton.IsEnabled = false;
            }
        }

        private void NavigateToHome()
        {
            frame.Navigate(typeof(HomePageContent));
        }

        private void NavigateToSetting(object sender, RoutedEventArgs e)
        {
            if (frame.Content.GetType() != typeof(SettingPageContent))
            {
                frame.Navigate(typeof(SettingPageContent));
            }
        }

        private void NavigateToWebView(object sender, object e)
        {
            frame.Navigate(typeof(WebViewPageContent), e);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (frame.Content.GetType() == typeof(WebViewPageContent))
            {
                WebViewGoBack(this, null);
            }
            else if (frame.Content.GetType() == typeof(SettingPageContent))
            {
                FrameGoBack(this, null);
            }
        }

        private void FrameGoBack(object sender, object e)
        {
            frame.GoBack();
        }

        private void OpenLink(object sender, RoutedEventArgs e)
        {
            OpenLinkInBrowser(this, frame.Content.GetType());
        }
    }
}