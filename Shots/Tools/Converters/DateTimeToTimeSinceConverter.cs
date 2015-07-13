using System;
using Windows.UI.Xaml.Data;
using Shots.Core.Extensions;

namespace Shots.Tools.Converters
{
    public class DateTimeToTimeSinceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTime) value;
            return date.ToTimeSince();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}