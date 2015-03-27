using System;
using System.IO;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Shots.Api.Models;
using Shots.Utilities;
using Shots.Views;
using System.Net.Http;
using Windows.Storage;

namespace Shots.Controls
{
    public sealed partial class ShotItemViewer
    {
        public static readonly DependencyProperty ScrollListViewProperty = DependencyProperty.RegisterAttached(
            "ScrollListView",
            typeof (ScrollListView),
            typeof (ShotItemViewer),
            new PropertyMetadata(null, null));

        public ShotItemViewer()
        {
            InitializeComponent();
        }

        public ScrollListView ScrollListView
        {
            get { return (ScrollListView) GetValue(ScrollListViewProperty); }
            set { SetValue(ScrollListViewProperty, value); }
        }

        public static ScrollListView GetScrollListView(DependencyObject element)
        {
            return (ScrollListView) element.GetValue(ScrollListViewProperty);
        }

        public static void SetScrollListView(DependencyObject element, ScrollListView value)
        {
            element.SetValue(ScrollListViewProperty, value);
        }

        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the current like state
            ShotItem.Resource.IsLike = !ShotItem.Resource.IsLike;

            // Now toggle the like over at the api
            var resp = await App.Locator.ShotsService.LikeShotItemAsync(ShotItem.Resource.Id, ShotItem.Resource.IsLike);

            // Something happened
            if (resp.Status != Status.Success)
            {
                // Reset back
                ShotItem.Resource.IsLike = !ShotItem.Resource.IsLike;

                // TODO report error to user
            }
        }

        public ShotItem ShotItem { get { return DataContext as ShotItem; } }

        private void Image_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            LoadingProgress.Begin();
            image.Visibility = Visibility.Visible;
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            LoadingProgress.Stop();
            image.Visibility = Visibility.Collapsed;
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            LoadingProgress.Stop();
            image.Visibility = Visibility.Collapsed;
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            App.RootFrame.Navigate(typeof (ProfilePage), ShotItem.User);
        }

        private void ShareMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += OnDataRequested;
            DataTransferManager.ShowShareUI();
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= OnDataRequested;
            var package = new DataPackage();

            package.Properties.Title = string.Format("Check out this Shot by @{0}", ShotItem.User.Username);
            package.SetUri(new Uri(ShotItem.Resource.Url));
            package.Properties.Description = ShotItem.Resource.Description;

            args.Request.Data = package;
        }

        private async void SaveMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            StatusBarHelper.ShowStatus("Saving...");
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(ShotItem.Resource.Url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            var file = await StorageHelper.CreateFileAsync(Guid.NewGuid().ToString("n") + ".jpg", KnownFolders.SavedPictures);

                            using (var fileStream = await file.OpenStreamForWriteAsync())
                            {
                                await stream.CopyToAsync(fileStream);
                            }
                        }
                    }
                    else
                    {
                        // TODO: report error
                    }
                }
            }
            StatusBarHelper.HideStatus();
        }
    }
}