using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace AppSettings
{
    public static class Theme
    {
        public static string GetAppThemeSetting()
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values["theme"] == null)
            {
                return "Use system setting";
            }
            else
            {
                return Windows.Storage.ApplicationData.Current.LocalSettings.Values["theme"].ToString();
            }
        }

        public static void SaveAppThemeSetting(string theme)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["theme"] = theme;
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
            SetTitleBarTheme(Window.Current.Content as FrameworkElement, null);
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