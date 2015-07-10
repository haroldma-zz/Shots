using System;
using Windows.UI.Xaml.Data;

namespace Shots.Tools.Converters
{
    public class BoolToStringConverter : IValueConverter
    {
        public string TrueContent { get; set; }
        public string FalseContent { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? TrueContent : FalseContent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}