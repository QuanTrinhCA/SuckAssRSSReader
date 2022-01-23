using CustomObjects;
using Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
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

        private static bool s_isFirstThemeSettingUIUpdate;

        public SettingPageContent()
        {
            InitializeComponent();

            this.Loaded += SetUpPage;
            removeButton.Click += RemoveFeedsAsync;
            addButton.Click += LauchAddFeedDialogAsync;
            acceptButton.Click += SaveFeedsAsync;
            radioButtons.SelectionChanged += SaveAppThemeSettingAsync;
        }

        private void SetUpPage(object sender, RoutedEventArgs e)
        {
            ChangeStateOfBackButton(this, true);
            ChangeStateOfOpenButton(this, false);

            s_isFirstThemeSettingUIUpdate = true;
            UpdateThemeSettingRadioButtonsAsync();

            UpdateFeedsAsync();

            GC.Collect(1);
        }

        private async void SaveAppThemeSettingAsync(object sender, SelectionChangedEventArgs e)
        {
            if (s_isFirstThemeSettingUIUpdate)
            {
                s_isFirstThemeSettingUIUpdate = false;
            }
            else if (radioButtons.SelectedItem != null)
            {
                ElementTheme selectedTheme = ElementTheme.Default;
                switch (radioButtons.SelectedItem.ToString())
                {
                    case "Light":
                        selectedTheme = ElementTheme.Light;
                        break;

                    case "Dark":
                        selectedTheme = ElementTheme.Dark;
                        break;

                    case "Use system setting":
                        selectedTheme = ElementTheme.Default;
                        break;
                }
                try
                {
                    Theme.SaveAppThemeSetting(selectedTheme);
                }
                catch (Exception)
                {
                    await new ContentDialog
                    {
                        Title = "Error",
                        Content = "An error occured while saving app theme",
                        CloseButtonText = "Ok"
                    }.ShowAsync();
                }
                if (await new ContentDialog
                {
                    Title = "Success",
                    Content = "App theme has been successfully changed, please restart to see the changes",
                    PrimaryButtonText = "Restart",
                    CloseButtonText = "Cancel"
                }.ShowAsync() == ContentDialogResult.Primary)
                {
                    await CoreApplication.RequestRestartAsync("");
                }
            }
        }

        private async void UpdateThemeSettingRadioButtonsAsync()
        {
            try
            {
                switch (Theme.GetAppThemeSetting())
                {
                    case ElementTheme.Light:
                        radioButtons.SelectedIndex = 0;
                        break;

                    case ElementTheme.Dark:
                        radioButtons.SelectedIndex = 1;
                        break;

                    case ElementTheme.Default:
                        radioButtons.SelectedIndex = 2;
                        break;
                }
            }
            catch (Exception)
            {
                await new ContentDialog
                {
                    Title = "Error",
                    Content = "An error occured while getting app theme",
                    CloseButtonText = "Ok"
                }.ShowAsync();
            }
        }

        private async void UpdateFeedsAsync()
        {
            try
            {
                foreach (CustomFeed feed in await Task.Run(() => { return Services.Feeds.GetSavedFeeds(); }))
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
                    Title = "Error",
                    Content = "An error occured while getting saved feeds",
                    CloseButtonText = "Ok"
                }.ShowAsync();
            }
        }

        private async void RemoveFeedsAsync(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception)
            {
                await new ContentDialog
                {
                    Title = "Error",
                    Content = "An error occured while removing feeds",
                    CloseButtonText = "Ok"
                }.ShowAsync();
            }
        }

        private async void SaveFeedsAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    Services.Feeds.SaveFeeds(Feeds.ToList());
                });
            }
            catch (Exception)
            {
                await new ContentDialog
                {
                    Title = "Error",
                    Content = "An error occured while saving changes",
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
                    string feedUrl = dialogContent.GetFeedUrl();
                    await Task.Run(async () =>
                    {
                        newFeed = await Services.Feeds.GetFeedAsync(feedUrl);
                    });
                    dialogContent = null;
                }
                catch (Exception)
                {
                    newFeedIsValid = false;
                }
                if (Feeds.Where(x => x.Link == newFeed.Link).Count() == 0 && newFeedIsValid)
                {
                    Feeds.Add(newFeed);
                    await new ContentDialog
                    {
                        Title = "Success",
                        Content = "The feed has been successfully added",
                        CloseButtonText = "Ok"
                    }.ShowAsync();
                }
                else if (Feeds.Where(x => x.Link == newFeed.Link).Count() != 0 && newFeedIsValid)
                {
                    await new ContentDialog
                    {
                        Title = "Error",
                        Content = "The feed has been already added",
                        CloseButtonText = "Ok"
                    }.ShowAsync();
                }
                else
                {
                    await new ContentDialog
                    {
                        Title = "Error",
                        Content = "An error occured while adding the feed, please check the URL again",
                        CloseButtonText = "Ok"
                    }.ShowAsync();
                }
            }
            GC.Collect(1);
        }
    }
}