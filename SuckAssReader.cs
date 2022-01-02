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
        public static async Task<ObservableCollection<CustomFeed>> GetFeeds(ObservableCollection<CustomFeed> oldFeeds)
        {
            var feed = await FeedReader.ReadAsync("https://lorem-rss.herokuapp.com/feed?unit=second&interval=3");
            var newFeeds = new ObservableCollection<CustomFeed>();
            var tempFeedCollection = new List<CustomFeed>();
            foreach (FeedItem item in feed.Items)
            {
                if (oldFeeds.Where(x => x.Link == item.Link).Count() == 0)
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(item.Description);
                    var imageUri = "ms-appx:///Assets/Placeholder.png";
                    if (doc.DocumentNode.SelectSingleNode("//img[1]") != null)
                    {
                        imageUri = doc.DocumentNode.SelectSingleNode("//img[1]").Attributes["src"].Value;
                    }
                    else if (feed.ImageUrl != null)
                    {
                        imageUri = feed.ImageUrl;
                    }
                    tempFeedCollection.Add(new CustomFeed()
                    {
                        Publisher = feed.Title,
                        ImageLink = imageUri,
                        Title = item.Title,
                        Link = item.Link,
                        PublishingDate = item.PublishingDate.Value,
                        PublishingDateString = item.PublishingDate.Value.Date.ToLocalTime().ToLongDateString(),
                        Categories = item.Categories
                    });
                }
            }
            tempFeedCollection = tempFeedCollection.OrderByDescending(x => x.PublishingDate).ToList();
            foreach (CustomFeed item in tempFeedCollection)
            {
                newFeeds.Insert(0, item);
            }
            return newFeeds;
        }
    }
}
