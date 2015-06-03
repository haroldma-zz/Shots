using System;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
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
        
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            /*if (_searchMode) return;

            _searchMode = true;
            App.Current.BackRequested += AppOnSupressedBackEvent;
            SearchGrid.Visibility = Visibility.Visible;*/
        }

        private void AppOnSupressedBackEvent(object sender, BackPressedEventArgs e)
        {
            /*e.Handled = true;
            SearchBox.Text = "";
            _searchMode = false;
            App.Current.BackRequested -= AppOnSupressedBackEvent;
            SearchGrid.Visibility = Visibility.Collapsed;*/
        }
    }
}