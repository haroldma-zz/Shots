using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Shots.Views
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void SearchBox_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Only way to lose focus from the search box
            if (e.Key == VirtualKey.Enter)
                AutoHideBar.Focus(FocusState.Programmatic);
        }
    }
}