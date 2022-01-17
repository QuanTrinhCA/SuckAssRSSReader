using AppSettings;
using System;
using Windows.ApplicationModel.Core;
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
        public MainPage()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;

            InitializeComponent();

            Theme.SetAppTheme(Theme.GetAppThemeSetting());

            HomePageContent.ListViewDoubleTapped += NavigateToWebView;
            HomePageContent.ChangeStateOfOpenButton += ChangeStateOfOpenButton;
            HomePageContent.ChangeStateOfBackButton += ChangeStateOfBackButton;

            WebViewPageContent.CanNotGoBack += FrameGoBack;
            WebViewPageContent.ChangeStateOfOpenButton += ChangeStateOfOpenButton;
            WebViewPageContent.ChangeStateOfBackButton += ChangeStateOfBackButton;

            SettingPageContent.ChangeStateOfOpenButton += ChangeStateOfOpenButton;
            SettingPageContent.ChangeStateOfBackButton += ChangeStateOfBackButton;

            backButton.Click += FireGoBackEvent;
            openButton.Click += FireOpenLinkInBrowserEvent;
            settingButton.Click += NavigateToSetting;

            NavigateToHome();
        }

        public static event EventHandler GoBack;

        public static event EventHandler<object> OpenLinkInBrowser;
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

        private void FireGoBackEvent(object sender, RoutedEventArgs e)
        {
            if (frame.Content.GetType() == typeof(WebViewPageContent))
            {
                GoBack(this, null);
            }
            else if (frame.Content.GetType() == typeof(SettingPageContent))
            {
                FrameGoBack(this, null);
            }
        }

        private void FireOpenLinkInBrowserEvent(object sender, RoutedEventArgs e)
        {
            OpenLinkInBrowser(this, frame.Content.GetType());
        }

        private void FrameGoBack(object sender, object e)
        {
            frame.GoBack();
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

        private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            appTitleTextBlock.Height = sender.Height;
        }
    }
}