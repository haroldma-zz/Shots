using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Shots.Api.Models;
using Shots.ViewModel;

namespace Shots.Views
{
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var info = e.Parameter as SimpleUserInfo;
            var vm = (ProfileViewModel) DataContext;
            vm.SetUser(info);
        }
    }
}