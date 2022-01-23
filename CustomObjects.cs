using System;
using System.Collections.Generic;

namespace CustomObjects
{
    public class CustomFeed
    {
        public string Description;
        public string ImageLink;
        public string Link;
        public string Title;
    }

    public class CustomFeedItem
    {
        public ICollection<string> Categories;
        public string ImageLink;
        public string Link;
        public string Publisher;
        public DateTime PublishingDate;
        public string PublishingDateString;
        public string Title;
    }
}