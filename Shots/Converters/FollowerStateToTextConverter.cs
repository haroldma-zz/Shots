using System;
using Windows.UI.Xaml.Data;
using Shots.Api.Models;

namespace Shots.Converters
{
    public class FollowerStateToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var state = value as FriendState?;

            switch (state)
            {
                case FriendState.Requested:
                    return "Requested";
                case FriendState.Added:
                    return "Added";
                case FriendState.Private:
                    return "Request";
                default:
                    return "Add";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}