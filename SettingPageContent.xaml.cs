using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SuckAssRSSReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingPageContent : Page
    {
        public ObservableCollection<CustomFeed> Feeds = new ObservableCollection<CustomFeed>();
        public SettingPageContent()
        {
            InitializeComponent();
            Updatefeeds();
            removeButton.Click += RemoveFeed;
            addButton.Click += LauchAddFeedDialog;
            acceptButton.Click += SaveFeeds;
        }

        private void RemoveFeed(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                Feeds.Remove(listView.SelectedItem as CustomFeed);
            }
        }

        private void SaveFeeds(object sender, RoutedEventArgs e)
        {
            SuckAssReader.SaveFeeds(Feeds);
        }

        private void Updatefeeds()
        {
            foreach (CustomFeed feed in SuckAssReader.GetSavedFeeds())
            {
                if (feed != null)
                {
                    Feeds.Insert(0, feed);
                }
            }
        }
        //A way to get the result from the text box inside the dialog content... idk what I'm doing
        private AddFeedDialogContent dialogContent;
        private async void LauchAddFeedDialog(object sender, object e)
        {
            dialogContent = new AddFeedDialogContent();
            var dialog = new ContentDialog
            {
                Title = "Add new feed source",
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = dialogContent
            };
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;
            await dialog.ShowAsync();
            dialogContent = null;
        }
        private async void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var newFeed = await SuckAssReader.GetFeed(dialogContent.GetFeedUrl());
            if (Feeds.Where(x => x.Link == newFeed.Link).Count() == 0)
            {
                Feeds.Add(newFeed);
            }
        }
    }
}
