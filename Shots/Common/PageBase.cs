using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Shots.Common
{
    public class PageBase : Page
    {
        public PageBase()
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (!Frame.CanGoBack) return;
            e.Handled = true;
            Frame.GoBack();
        }
    }
}