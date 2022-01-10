using AppFeedReader;
using AppSettings;
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
        public static event EventHandler<bool> ChangeStateOfOpenButton;
        public static event EventHandler<bool> ChangeStateOfBackButton;

        public ObservableCollection<CustomFeed> Feeds = new ObservableCollection<CustomFeed>();
        public SettingPageContent()
        {
            InitializeComponent();

            Loaded += SettingPageContent_Loaded;
            removeButton.Click += RemoveFeed;
            addButton.Click += LauchAddFeedDialog;
            acceptButton.Click += SaveFeeds;
            radioButtons.SelectionChanged += ChangeTheme;

            UpdateFeeds();
            InitialUpdateOfThemeSetting();
        }
        private void ChangeTheme(object sender, SelectionChangedEventArgs e)
        {
            AppTheme.SaveAppThemeSetting(radioButtons.SelectedItem as string);
            AppTheme.SetAppTheme(radioButtons.SelectedItem as string);
        }
        private void InitialUpdateOfThemeSetting()
        {
            switch (AppTheme.GetAppThemeSetting())
            {
                case "Light":
                    radioButtons.SelectedIndex = 0;
                    break;
                case "Dark":
                    radioButtons.SelectedIndex = 1;
                    break;
                case "Use system setting":
                    radioButtons.SelectedIndex = 2;
                    break;
            }
        }
        private void SettingPageContent_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeStateOfBackButton(this, true);
            ChangeStateOfOpenButton(this, false);
        }
        private void RemoveFeed(object sender, RoutedEventArgs e)
        {
            var removeList = listView.SelectedItems.ToList();
            if (removeList.Count != 0)
            {
                foreach (CustomFeed feed in removeList)
                {
                    Feeds.Remove(feed);
                }
            }
        }
        private async void SaveFeeds(object sender, RoutedEventArgs e)
        {
            try
            {
                SuckAssReader.SaveFeeds(Feeds);
            }
            catch (Exception)
            {
                ContentDialog savingFeedsErrorDialog = new ContentDialog
                {
                    Title = "Error saving changes",
                    Content = "An error occured while saving changes, please try again",
                    CloseButtonText = "Ok"
                };
                await savingFeedsErrorDialog.ShowAsync();
            }

        }
        private async void UpdateFeeds()
        {
            try
            {
                foreach (CustomFeed feed in SuckAssReader.GetSavedFeeds())
                {
                    if (feed != null)
                    {
                        Feeds.Insert(0, feed);
                    }
                }
            }
            catch (Exception)
            {
                ContentDialog gettingSavedFeedsErrorDialog = new ContentDialog
                {
                    Title = "Error getting saved feeds",
                    Content = "An error occured while getting saved feeds",
                    CloseButtonText = "Ok"
                };
                await gettingSavedFeedsErrorDialog.ShowAsync();
            }

        }
        //A way to get the result from the text box inside the dialog content... idk what I'm doing
        private AddFeedDialogContent dialogContent;
        private async void LauchAddFeedDialog(object sender, object e)
        {
            dialogContent = new AddFeedDialogContent();
            var addFeedDialog = new ContentDialog
            {
                Title = "Add new feed source",
                PrimaryButtonText = "Ok",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = dialogContent
            };
            addFeedDialog.Closed += AddFeedDialog_Closed;
            await addFeedDialog.ShowAsync();
            dialogContent = null;
        }
        private async void AddFeedDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary)
            {
                CustomFeed newFeed = new CustomFeed();
                bool newFeedIsValid = true;
                try
                {
                    newFeed = await SuckAssReader.GetFeed(dialogContent.GetFeedUrl());
                }
                catch (Exception)
                {
                    newFeedIsValid = false;
                    ContentDialog feedUrlErrorDialog = new ContentDialog
                    {
                        Title = "Error adding new feed",
                        Content = "An error occured while adding the feed, please check the URL again",
                        CloseButtonText = "Ok"
                    };
                    await feedUrlErrorDialog.ShowAsync();
                }
                if (Feeds.Where(x => x.Link == newFeed.Link).Count() == 0 && newFeedIsValid)
                {
                    Feeds.Add(newFeed);
                }
            }
        }
    }
}
