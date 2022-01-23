using CodeHollow.FeedReader;
using CustomObjects;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Services
{
    public class Sync
    {
        public static int GetSyncInterval()
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values["syncInterval"] != null)
            {
                return (int)Windows.Storage.ApplicationData.Current.LocalSettings.Values["syncInterval"];
            }
            else
            {
                return 300000; //5 mins
            }
        }

        public static void SaveSyncInterval(int syncInterval)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["syncInterval"] = syncInterval;
        }
    }

    public class Feeds
    {
        private static List<CustomFeed> s_feeds = new List<CustomFeed>();

        public static async Task<CustomFeed> GetFeedAsync(string inputUrl)
        {
            var urls = await FeedReader.GetFeedUrlsFromUrlAsync(inputUrl);
            string feedUrl;
            if (urls.Count() == 0)
            {
                feedUrl = inputUrl;
            }
            else
            {
                feedUrl = urls.First().Url;
            }
            var feed = await FeedReader.ReadAsync(feedUrl);
            string imageLink;
            if (feed.ImageUrl != null)
            {
                imageLink = feed.ImageUrl;
            }
            else
            {
                imageLink = "ms-appx:///Assets/Placeholder.png";
            }
            return new CustomFeed()
            {
                Title = feed.Title,
                ImageLink = imageLink,
                Description = feed.Description,
                Link = feedUrl
            };
        }

        public static async Task<List<CustomFeedItem>> GetFeedItemsAsync(List<CustomFeedItem> oldFeedItems)
        {
            var newFeedItems = new List<CustomFeedItem>();
            foreach (CustomFeed savedFeed in s_feeds)
            {
                Feed feed = new Feed();
                bool readSuccessful = true;
                try
                {
                    feed = await FeedReader.ReadAsync(savedFeed.Link);
                }
                catch (Exception)
                {
                    readSuccessful = false;
                }
                if (readSuccessful)
                {
                    foreach (FeedItem feedItem in feed.Items)
                    {
                        if (oldFeedItems.Where(x => x.Link == feedItem.Link).Count() == 0)
                        {
                            var doc = new HtmlDocument();
                            doc.LoadHtml(feedItem.Description);
                            string imageLink;
                            if (doc.DocumentNode.SelectSingleNode("//img[1]") != null)
                            {
                                imageLink = doc.DocumentNode.SelectSingleNode("//img[1]").Attributes["src"].Value;
                            }
                            else if (feed.ImageUrl != null)
                            {
                                imageLink = feed.ImageUrl;
                            }
                            else
                            {
                                imageLink = "ms-appx:///Assets/Placeholder.png";
                            }
                            newFeedItems.Add(new CustomFeedItem()
                            {
                                Publisher = feed.Title,
                                ImageLink = imageLink,
                                Title = feedItem.Title,
                                Link = feedItem.Link,
                                PublishingDate = feedItem.PublishingDate.Value,
                                PublishingDateString = feedItem.PublishingDate.Value.Date.ToLocalTime().ToLongDateString(),
                                Categories = feedItem.Categories
                            });
                        }
                    }
                }
            }
            return newFeedItems.OrderBy(x => x.PublishingDate).ToList();
        }

        public static List<CustomFeed> GetSavedFeeds()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.CreateContainer("feeds", Windows.Storage.ApplicationDataCreateDisposition.Always);
            if (localSettings.Containers.ContainsKey("feeds"))
            {
                foreach (var key in localSettings.Containers["feeds"].Values.Keys)
                {
                    if (localSettings.Containers["feeds"].Values.ContainsKey(key))
                    {
                        var composite = localSettings.Containers["feeds"].Values[key] as Windows.Storage.ApplicationDataCompositeValue;
                        var feed = new CustomFeed
                        {
                            Title = composite["Title"].ToString(),
                            Description = composite["Description"].ToString(),
                            ImageLink = composite["ImageLink"].ToString(),
                            Link = composite["Link"].ToString()
                        };
                        if (s_feeds.Where(x => x.Link == feed.Link).Count() == 0)
                        {
                            s_feeds.Add(feed);
                        }
                    }
                }
                s_feeds = s_feeds.OrderBy(x => x.Title).ToList();
            }
            return s_feeds;
        }

        public static void SaveFeeds(List<CustomFeed> newfeeds)
        {
            s_feeds = newfeeds;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.DeleteContainer("feeds");
            localSettings.CreateContainer("feeds", Windows.Storage.ApplicationDataCreateDisposition.Always);
            int i = 0;
            foreach (var feed in s_feeds)
            {
                var composite = new Windows.Storage.ApplicationDataCompositeValue
                {
                    ["Title"] = feed.Title,
                    ["Description"] = feed.Description,
                    ["ImageLink"] = feed.ImageLink,
                    ["Link"] = feed.Link
                };
                localSettings.Containers["feeds"].Values.Add("feed" + i.ToString(), composite);
                i++;
            }
        }
    }

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