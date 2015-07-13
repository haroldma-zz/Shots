using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Advertising.Mobile.UI;

namespace Shots.Controls
{
    public sealed partial class AdSpaceControl
    {
        private AdControl _control;

        public AdSpaceControl()
        {
            InitializeComponent();
            Loaded += AdSpaceControl_Loaded;
        }

        private void AdSpaceControl_Loaded(object sender, RoutedEventArgs e)
        {
            var width = Window.Current.Bounds.Width;
            var height = width * 0.167;

            _control = new AdControl("6b3f70cb-c3e4-4293-89cc-e0be101919c8", "206550", true)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = height,
                Width = width
            };
            _control.AdRefreshed += _control_AdRefreshed;
            _control.ErrorOccurred += Control_ErrorOccurred;
            RootGrid.Children.Add(_control);
        }

        private void _control_AdRefreshed(object sender, RoutedEventArgs e)
        {
            _control.Visibility = Visibility.Visible;
        }

        private void Control_ErrorOccurred(object sender, Microsoft.Advertising.Mobile.Common.AdErrorEventArgs e)
        {
            _control.Visibility = Visibility.Collapsed;
            Debug.WriteLine(e.Error.Message);
        }
    }
}