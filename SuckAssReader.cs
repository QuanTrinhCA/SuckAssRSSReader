using CodeHollow.FeedReader;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SuckAssRSSReader
{
    internal class CustomFeed
    {
        internal string Publisher;
        internal string ImageLink;
        internal string Link;
        internal string Title;
        internal DateTime PublishingDate;
        internal string PublishingDateString;
        internal ICollection<string> Categories;
    }
    internal class SuckAssReader
    {
        internal static ObservableCollection<CustomFeed> CustomFeeds;
        internal static async Task<ObservableCollection<CustomFeed>> GetFeeds()
        {
            var _feed = await FeedReader.ReadAsync("https://9to5mac.com/feed");
            var _customfeeds = new ObservableCollection<CustomFeed>();
            _feed.Items = _feed.Items.OrderByDescending(x => x.PublishingDate).ToList();
            foreach (var item in _feed.Items)
            {
                var _doc = new HtmlDocument();
                _doc.LoadHtml(item.Description);
                var _descriptionImage = _doc.DocumentNode.SelectSingleNode("//img[1]").Attributes["src"].Value;
                var _imageUri = "ms-appx:///Assets/Placeholder.png";
                if (_descriptionImage != null)
                {
                    _imageUri = _descriptionImage;
                } else if (_feed.ImageUrl != null)
                {
                    _imageUri = _feed.ImageUrl;
                }
                _customfeeds.Add(new CustomFeed()
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
            return _customfeeds;
        }
    }
}
