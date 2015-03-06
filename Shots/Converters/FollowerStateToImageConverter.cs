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
            var profile = value as SimpleUserInfo;
            if (profile == null) return NotFollowingContent;

            if (profile.IsRequested) return FollowingRequestedContent;
            return profile.IsFriend ? FollowingContent : NotFollowingContent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}