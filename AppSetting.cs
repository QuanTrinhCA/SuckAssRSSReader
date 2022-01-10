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
    }
}
