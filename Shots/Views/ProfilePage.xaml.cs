using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Shots.Api.Models;
using Shots.ViewModel;

namespace Shots.Views
{
    public sealed partial class ProfilePage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var info = e.Parameter as UserInfo;

            if (info == null)
            {
                var userInfo = e.Parameter as SimpleUserInfo;
                DataContext = userInfo != null
                    ? new ProfileViewModel(App.Locator.ShotsService, userInfo.Username)
                    : new ProfileViewModel(App.Locator.ShotsService, e.Parameter as string);
            }
            else
                DataContext = new ProfileViewModel(App.Locator.ShotsService, info);
        }
    }
}