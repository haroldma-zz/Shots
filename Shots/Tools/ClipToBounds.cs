using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Shots.Tools
{
    public static class ClipToBounds
    {
        public static bool GetClipToBounds(DependencyObject obj)
        {
            return (bool) obj.GetValue(ClipToBoundsProperty);
        }

        public static void SetClipToBounds(DependencyObject obj, bool value)
        {
            obj.SetValue(ClipToBoundsProperty, value);
        }

        private static void OnClipToBoundsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as FrameworkElement;

            if (element != null)
            {
                if ((bool) e.NewValue)
                    element.SizeChanged += Element_SizeChanged;
                else
                    element.SizeChanged -= Element_SizeChanged;
            }
        }

        private static void Element_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var element = sender as FrameworkElement;

            UpdateClipSize(element, e.NewSize);
        }

        public static void UpdateClipSize(FrameworkElement element, Size clipSize)
        {
            if (element != null)
            {
                RectangleGeometry clipRectangle = null;

                if (element.Clip == null)
                {
                    clipRectangle = new RectangleGeometry();
                    element.Clip = clipRectangle;
                }
                else
                {
                    if (element.Clip is RectangleGeometry)
                    {
                        clipRectangle = element.Clip;
                    }
                }

                if (clipRectangle != null)
                {
                    clipRectangle.Rect = new Rect(new Point(0, 0), clipSize);
                }
            }
        }

        public static readonly DependencyProperty ClipToBoundsProperty =
            DependencyProperty.RegisterAttached(
                "ClipToBounds",
                typeof (bool),
                typeof (ClipToBounds),
                new PropertyMetadata(false, OnClipToBoundsChanged)
                );
    }
}