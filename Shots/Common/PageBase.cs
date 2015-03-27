using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Shots.Common
{
    public class PageBase : Page
    {
        public PageBase()
        {
            NavigationCacheMode = NavigationCacheMode.Required;
        }
    }
}