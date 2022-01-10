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
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            switch (theme)
            {
                case "Light":
                    (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Light;

                    // Set active window colors
                    titleBar.ForegroundColor = Windows.UI.Colors.Black;
                    titleBar.BackgroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonForegroundColor = Windows.UI.Colors.Black;
                    titleBar.ButtonBackgroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(255, 47, 47, 47);
                    titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.LightGray;
                    titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(255, 45, 45, 45);

                    // Set inactive window colors
                    titleBar.InactiveForegroundColor = Windows.UI.Colors.Gray;
                    titleBar.InactiveBackgroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
                    titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.White;
                    break;
                case "Dark":
                    (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Dark;

                    // Set active window colors
                    titleBar.ForegroundColor = Windows.UI.Colors.White;
                    titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 41, 41, 41);
                    titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 41, 41, 41);
                    titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
                    titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(255, 47, 47, 47);
                    titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.Gray;
                    titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(255, 45, 45, 45);

                    // Set inactive window colors
                    titleBar.InactiveForegroundColor = Windows.UI.Colors.Gray;
                    titleBar.InactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 41, 41, 41);
                    titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
                    titleBar.ButtonInactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 41, 41, 41);
                    break;
                case "Use system setting":
                    (Window.Current.Content as FrameworkElement).RequestedTheme = ElementTheme.Default;
                    if ((Window.Current.Content as FrameworkElement).ActualTheme == ElementTheme.Light)
                    {
                        // Set active window colors
                        titleBar.ForegroundColor = Windows.UI.Colors.Black;
                        titleBar.BackgroundColor = Windows.UI.Colors.White;
                        titleBar.ButtonForegroundColor = Windows.UI.Colors.Black;
                        titleBar.ButtonBackgroundColor = Windows.UI.Colors.White;
                        titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
                        titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(255, 47, 47, 47);
                        titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.LightGray;
                        titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(255, 45, 45, 45);

                        // Set inactive window colors
                        titleBar.InactiveForegroundColor = Windows.UI.Colors.Gray;
                        titleBar.InactiveBackgroundColor = Windows.UI.Colors.White;
                        titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
                        titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.White;
                    }
                    else
                    {
                        // Set active window colors
                        titleBar.ForegroundColor = Windows.UI.Colors.White;
                        titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 41, 41, 41);
                        titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
                        titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 41, 41, 41);
                        titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
                        titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(255, 47, 47, 47);
                        titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.Gray;
                        titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(255, 45, 45, 45);

                        // Set inactive window colors
                        titleBar.InactiveForegroundColor = Windows.UI.Colors.Gray;
                        titleBar.InactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 41, 41, 41);
                        titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
                        titleBar.ButtonInactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 41, 41, 41);
                    }
                    break;
            }
        }
    }
}
