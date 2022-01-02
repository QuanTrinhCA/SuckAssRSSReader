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
        private static ObservableCollection<CustomFeed> oldFeeds = new ObservableCollection<CustomFeed>();
        public static async Task<ObservableCollection<CustomFeed>> GetFeeds()
        {
            var _feed = await FeedReader.ReadAsync("https://lorem-rss.herokuapp.com/feed?unit=second&interval=3");
            ObservableCollection<CustomFeed> newFeeds = new ObservableCollection<CustomFeed>();
            foreach (var item in _feed.Items)
            {
                if (oldFeeds.Where(x => x.Link == item.Link).Count() == 0)
                {
                    var _doc = new HtmlDocument();
                    _doc.LoadHtml(item.Description);
                    var _imageUri = "ms-appx:///Assets/Placeholder.png";
                    if (_doc.DocumentNode.SelectSingleNode("//img[1]") != null)
                    {
                        _imageUri = _doc.DocumentNode.SelectSingleNode("//img[1]").Attributes["src"].Value;
                    }
                    else if (_feed.ImageUrl != null)
                    {
                        _imageUri = _feed.ImageUrl;
                    }
                    oldFeeds.Insert(0, new CustomFeed()
                    {
                        Publisher = _feed.Title,
                        ImageLink = _imageUri,
                        Title = item.Title,
                        Link = item.Link,
                        PublishingDate = item.PublishingDate.Value,
                        PublishingDateString = item.PublishingDate.Value.Date.ToLocalTime().ToLongDateString(),
                        Categories = item.Categories
                    });
                    newFeeds.Insert(0, new CustomFeed()
                    {
                        Publisher = _feed.Title,
                        ImageLink = _imageUri,
                        Title = item.Title,
                        Link = item.Link,
                        PublishingDate = item.PublishingDate.Value,
                        PublishingDateString = item.PublishingDate.Value.Date.ToLocalTime().ToLongDateString(),
                        Categories = item.Categories
                    });
                }
            }
            //feeds = feeds.OrderByDescending(x => x.PublishingDate).ToList();
            return newFeeds;
        }
    }
}
