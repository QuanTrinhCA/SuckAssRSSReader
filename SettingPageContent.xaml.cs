using AppFeeds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        public static event EventHandler<bool> ChangeStateOfBackButton;

        public static event EventHandler<bool> ChangeStateOfOpenButton;

        public SettingPageContent()
        {
            InitializeComponent();

            this.Loaded += SetUpPage;
            removeButton.Click += RemoveFeeds;
            addButton.Click += LauchAddFeedDialogAsync;
            acceptButton.Click += SaveFeedsAsync;
            radioButtons.SelectionChanged += ChangeTheme;
        }

        private void SetUpPage(object sender, RoutedEventArgs e)
        {
            ChangeStateOfBackButton(this, true);
            ChangeStateOfOpenButton(this, false);

            UpdateThemeSettingRadioButtons();
            UpdateFeedsAsync();
        }

        private void ChangeTheme(object sender, SelectionChangedEventArgs e)
        {
            AppSettings.Theme.SaveAppThemeSetting(radioButtons.SelectedItem as string);
            AppSettings.Theme.SetAppTheme(radioButtons.SelectedItem as string);
        }

        private void UpdateThemeSettingRadioButtons()
        {
            switch (AppSettings.Theme.GetAppThemeSetting())
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

        private async void UpdateFeedsAsync()
        {
            try
            {
                foreach (CustomFeed feed in await Task.Run(() => { return AppFeeds.Feeds.GetSavedFeeds(); }))
                {
                    if (feed != null)
                    {
                        Feeds.Insert(0, feed);
                    }
                }
            }
            catch (Exception)
            {
                await new ContentDialog
                {
                    Title = "Error getting saved feeds",
                    Content = "An error occured while getting saved feeds",
                    CloseButtonText = "Ok"
                }.ShowAsync();
            }
        }

        private void RemoveFeeds(object sender, RoutedEventArgs e)
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

        private async void SaveFeedsAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    AppFeeds.Feeds.SaveFeeds(Feeds.ToList());
                });
            }
            catch (Exception)
            {
                await new ContentDialog
                {
                    Title = "Error saving changes",
                    Content = "An error occured while saving changes, please try again",
                    CloseButtonText = "Ok"
                }.ShowAsync();
            }
        }

        //A way to get the result from the text box inside the dialog content... idk what I'm doing
        private AddFeedDialogContent dialogContent;

        private async void LauchAddFeedDialogAsync(object sender, object e)
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
            addFeedDialog.Closed += CheckFeedUrlAsync;
            await addFeedDialog.ShowAsync();
        }

        private async void CheckFeedUrlAsync(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary)
            {
                CustomFeed newFeed = new CustomFeed();
                bool newFeedIsValid = true;
                try
                {
                    await Task.Run(async () =>
                    {
                        newFeed = await AppFeeds.Feeds.GetFeedAsync(dialogContent.GetFeedUrl());
                        dialogContent = null;
                    });
                }
                catch (Exception)
                {
                    newFeedIsValid = false;
                    await new ContentDialog
                    {
                        Title = "Error adding new feed",
                        Content = "An error occured while adding the feed, please check the URL again",
                        CloseButtonText = "Ok"
                    }.ShowAsync();
                }
                if (Feeds.Where(x => x.Link == newFeed.Link).Count() == 0 && newFeedIsValid)
                {
                    Feeds.Add(newFeed);
                }
            }
        }
    }
}