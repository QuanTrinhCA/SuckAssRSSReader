using SuckAssRSSReader;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Services
{
    public static class Theme
    {
        public static ElementTheme GetAppThemeSetting()
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values["theme"] != null)
            {
                return (ElementTheme)Windows.Storage.ApplicationData.Current.LocalSettings.Values["theme"];
            }
            else
            {
                return ElementTheme.Default;
            }
        }

        public static void SaveAppThemeSetting(ElementTheme theme)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["theme"] = (int)theme;
        }

        public static void SetAppTheme(ElementTheme theme)
        {
            switch (theme)
            {
                case ElementTheme.Light:
                    Application.Current.RequestedTheme = ApplicationTheme.Light;
                    break;

                case ElementTheme.Dark:
                    Application.Current.RequestedTheme = ApplicationTheme.Dark;
                    break;
            }
        }

        public static void SetTitleBarTheme(ElementTheme theme)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            switch (theme)
            {
                case ElementTheme.Light:
                    titleBar.ButtonForegroundColor = Windows.UI.Colors.Black;
                    titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
                    titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(195, 0, 0, 0);
                    titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.LightGray;
                    titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(188, 0, 0, 0);

                    titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
                    titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
                    break;

                case ElementTheme.Dark:
                    titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
                    titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(15, 255, 255, 255);
                    titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.LightGray;
                    titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(08, 255, 255, 255);

                    titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
                    titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
                    break;
            }
        }
    }
}