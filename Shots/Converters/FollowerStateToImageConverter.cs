using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Shots.Api.Models;

namespace Shots.Converters
{
    public class FollowerStateToImageConverter : IValueConverter
    {
        public BitmapImage NotFollowingContent { get; set; }
        public BitmapImage FollowingContent { get; set; }
        public BitmapImage FollowingRequestedContent { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var state = value as FriendState?;
            switch (state)
            {
                case FriendState.Requested:
                    return FollowingRequestedContent;
                case FriendState.Added:
                    return FollowingContent;
                default:
                    return NotFollowingContent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}