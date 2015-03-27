using System;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Shots.Views
{
    public sealed partial class HomePage
    {
        private bool _searchMode;

        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            // Make sure the status bar is set to white when leaving the page
            StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            AutoHideBar.Show();
            StatusBar.GetForCurrentView().ForegroundColor = Colors.Black;
        }

        private void AutoHideBar_OnShowing(object sender, EventArgs e)
        {
            StatusBar.GetForCurrentView().ForegroundColor = Colors.Black;
        }

        private void AutoHideBar_OnHiding(object sender, EventArgs e)
        {
            StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_searchMode) return;

            _searchMode = true;
            App.SupressBackEvent = true;
            App.SupressedBackEvent += AppOnSupressedBackEvent;
            SearchGrid.Visibility = Visibility.Visible;
        }

        private void AppOnSupressedBackEvent(object sender, BackPressedEventArgs backPressedEventArgs)
        {
            SearchBox.Text = "";
            _searchMode = false;
            App.SupressBackEvent = false;
            App.SupressedBackEvent -= AppOnSupressedBackEvent;
            SearchGrid.Visibility = Visibility.Collapsed;
        }
    }
}