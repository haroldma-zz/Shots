using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Shots.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public BitmapImage TrueContent { get; set; }
        public BitmapImage FalseContent { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool) value ? TrueContent : FalseContent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}