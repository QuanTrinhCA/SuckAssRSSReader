using CodeHollow.FeedReader;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SuckAssRSSReader
{
    public class CustomFeed
    {
        public string Title;
        public string ImageLink;
        public string Link;
        public string Description;
    }
    public class CustomFeedItem
    {
        public string Publisher;
        public string ImageLink;
        public string Link;
        public string Title;
        public DateTime PublishingDate;
        public string PublishingDateString;
        public ICollection<string> Categories;
    }
    public class SuckAssReader
    {
        private static List<CustomFeed> s_feeds = new List<CustomFeed>();
        public static void Initialize()
        {
            _ = GetSavedFeeds();
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
        public static void SaveFeeds(ObservableCollection<CustomFeed> newfeeds)
        {
            s_feeds = newfeeds.ToList();
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
        public static async Task<List<CustomFeedItem>> GetFeedItems(List<CustomFeedItem> oldFeedItems)
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
            newFeedItems = newFeedItems.OrderBy(x => x.PublishingDate).ToList();
            return newFeedItems;
        }
        public static async Task<CustomFeed> GetFeed(string inputUrl)
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
    }
}
