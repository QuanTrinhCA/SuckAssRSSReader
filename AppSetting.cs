using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace AppSettings
{
    public static class AppTheme
    {
        public static void SaveAppThemeSetting(string theme)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["theme"] = theme;
        }
        public static string GetAppThemeSetting()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["theme"] == null)
            {
                return "Use system setting";
            }
            else
            {
                return localSettings.Values["theme"].ToString();
            }
        }
        public static void SetAppTheme(string theme)
        {
            (Window.Current.Content as FrameworkElement).ActualThemeChanged -= SetTitleBarTheme;
            (Window.Current.Content as FrameworkElement).ActualThemeChanged += SetTitleBarTheme;
            switch (theme)
            {
                case "Light":
                    (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Light;
                    break;
                case "Dark":
                    (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Dark;
                    break;
                case "Use system setting":
                    (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Default;
                    break;
            }
        }
        public static void SetTitleBarTheme(FrameworkElement sender, object args)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            switch (sender.ActualTheme)
            {
                case ElementTheme.Light:
                    titleBar.ButtonForegroundColor = Windows.UI.Colors.Black;
                    titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
                    titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(205, 0, 0, 0);
                    titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.LightGray;
                    titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(210, 0, 0, 0);

                    titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
                    titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
                    break;
                case ElementTheme.Dark:
                    titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
                    titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(15, 255, 255, 255);
                    titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.LightGray;
                    titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(10, 255, 255, 255);

                    titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
                    titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
                    break;
            }
        }
    }
}
