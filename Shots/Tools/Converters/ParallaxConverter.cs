using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Shots.Controls;
using Shots.Web.Models;

namespace Shots.Tools.Converters
{
    /// <summary>
    ///     Source: http://w8isms.blogspot.nl/2012/06/metro-parallax-background-in-xaml.html
    ///     Used to create background parallex like on the Win8 start screen
    /// </summary>
    public class ParallaxConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty ListProperty = DependencyProperty.RegisterAttached(
            "List",
            typeof (ScrollListView),
            typeof (ParallaxConverter),
            new PropertyMetadata(null, null));

        public static readonly DependencyProperty ItemProperty = DependencyProperty.RegisterAttached(
            "Item",
            typeof (ShotItem),
            typeof (ParallaxConverter),
            new PropertyMetadata(null, null));

        public ShotItem Item
        {
            get { return (ShotItem) GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public ScrollListView List
        {
            get { return (ScrollListView) GetValue(ListProperty); }
            set { SetValue(ListProperty, value); }
        }

        /// <summary>
        ///     Parallax converter helper
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (List == null) return 0;

            const int scrollExtent = 85;
            var container = List.ContainerFromItem(Item) as UIElement;

            if (container == null) return 0;

            //Get the current point of this List item relative to the top of the parent LongListSelector control
            var relativePoint = container.TransformToVisual(List).TransformPoint(new Point(0, 0));

            //If this point is visible (ie. is within the bounds of the LongListSelector) then we can update its transform

            var imageHeight = Item.Resource.RatioHeight;

            if (((!(relativePoint.Y > (imageHeight*-1))) && imageHeight != 0) ||
                (!(relativePoint.Y <= List.ActualHeight))) return 0;

            var margin = (scrollExtent*-1) + ((relativePoint.Y/List.ActualHeight)*scrollExtent);
            return margin;
        }

        /// <summary>
        ///     NotImplementedException
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}