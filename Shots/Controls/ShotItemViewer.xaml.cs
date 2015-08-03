using System;
using System.IO;
using System.Net.Http;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Shots.Common;
using Shots.Core.Helpers;
using Shots.Helpers;
using Shots.Views;
using Shots.Web.Models;
using Shots.Web.Services.Interface;

namespace Shots.Controls
{
    public sealed partial class ShotItemViewer
    {
        public ShotItemViewer()
        {
            InitializeComponent();
        }
        
        public ShotItem ShotItem => DataContext as ShotItem;

        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the current like state
            ShotItem.Resource.IsLike = !ShotItem.Resource.IsLike;

            var shotsService = App.Current.Kernel.Resolve<IShotsService>();

            // Now toggle the like over at the api
            var resp = await shotsService.LikeShotItemAsync(ShotItem.Resource.Id, ShotItem.Resource.IsLike);

            // Something happened
            if (resp.Status == Status.Success) return;

            // Reset back
            ShotItem.Resource.IsLike = !ShotItem.Resource.IsLike;
            CurtainPrompt.ShowError("Problem liking shot.");
        }

        private void Image_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (ShotItem?.Resource?.Type == "video")
            {
                MuteButton.Visibility = Visibility.Visible;
                MediaElement.Visibility = Visibility.Visible;
            }
            else
            {
                MuteButton.Visibility = Visibility.Collapsed;
                MediaElement.Visibility = Visibility.Collapsed;
            }
            MediaElement.IsMuted = true;

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
            App.Current.NavigationService.Navigate(typeof (ProfilePage), ShotItem.User.Username);
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

            package.Properties.Title = $"Check out this Shot by @{ShotItem.User.Username}";
            package.SetWebLink(new Uri(ShotItem.Resource.Url));
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
                            var file =
                                await
                                    StorageHelper.CreateFileAsync(Guid.NewGuid().ToString("n") + ".jpg",
                                        KnownFolders.SavedPictures);

                            using (var fileStream = await file.OpenStreamForWriteAsync())
                            {
                                await stream.CopyToAsync(fileStream);
                            }
                        }
                        CurtainPrompt.Show("Shot saved.");
                    }
                    else
                        CurtainPrompt.ShowError("Problem saving shot.");
                }
            }
            StatusBarHelper.HideStatus();
        }

        private void ReactTo_Click(object sender, RoutedEventArgs e)
        {
            App.Current.NavigationService.Navigate(typeof (ShotPage), ShotItem.ReactTo.Resource.Id);
        }
    }
}