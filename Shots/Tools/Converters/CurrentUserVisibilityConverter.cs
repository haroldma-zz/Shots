using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Shots.Web.Services.Interface;

namespace Shots.Tools.Converters
{
    public class CurrentUserVisibilityConverter : IValueConverter
    {
        public bool HideIfUser { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var id = value as string;
            var service = App.Current.Kernel.Resolve<IShotsService>();

            if (id == service.CurrentUser.Id)
                return HideIfUser ? Visibility.Collapsed : Visibility.Visible;
            return HideIfUser ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}