using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Shots.Tools
{
    internal interface IPopupInput
    {
        Task<string> WaitForInputAsync();
    }

    internal static class PopupInputHelper
    {
        public static async Task<string> GetInputAsync<T>(T input) where T : FrameworkElement, IPopupInput
        {
            var size = App.Current.RootFrame;

            input.Width = size.ActualWidth;
            input.Height = size.ActualHeight;

            var popup = new Popup
            {
                IsOpen = true,
                Child = input,
                RenderTransform = new TranslateTransform
                {
                    Y = input.Height
                }
            };

            #region Slide up animation

            var slideAnimation = new DoubleAnimation
            {
                From = input.Height,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(250)),
                EasingFunction = new SineEase()
            };

            var sb = new Storyboard();
            sb.Children.Add(slideAnimation);
            Storyboard.SetTarget(slideAnimation, popup);
            Storyboard.SetTargetProperty(slideAnimation, "(UIElement.RenderTransform).(TranslateTransform.Y)");

            sb.Begin();

            #endregion

            var text = await input.WaitForInputAsync();

            #region Slide down animation

            slideAnimation = new DoubleAnimation
            {
                From = 0,
                To = input.Height,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new SineEase()
            };

            sb = new Storyboard();
            sb.Children.Add(slideAnimation);
            Storyboard.SetTarget(slideAnimation, popup);
            Storyboard.SetTargetProperty(slideAnimation, "(UIElement.RenderTransform).(TranslateTransform.Y)");

            sb.Completed += (sender, o) =>
            {
                popup.IsOpen = false;
                popup.Child = null;
            };

            sb.Begin();

            #endregion

            return text;
        }
    }
}