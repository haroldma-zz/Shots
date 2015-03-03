using System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Navigation;

namespace Shots.Views
{
    public sealed partial class HomePage
    {
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
    }
}