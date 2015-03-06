using System;
using Windows.UI.Xaml.Data;
using Shots.Api.Models;

namespace Shots.Converters
{
    public class FollowerStateToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var profile = value as SimpleUserInfo;
            if (profile == null) return "Add";

            if (profile.IsRequested) return "Requested";
            return profile.IsFriend ? "Added" : "Add";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}