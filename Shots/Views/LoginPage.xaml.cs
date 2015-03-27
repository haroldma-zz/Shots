using Windows.UI.Popups;
using Windows.UI.Xaml;
using Shots.Api.Models;

namespace Shots.Views
{
    public sealed partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordTextBox.Password;

            // TODO validate username and pass

            var resp = await App.Locator.ShotsService.LoginAsync(username, password);
            if (resp.Status == Status.Success)
            {
                Frame.Navigate(typeof (HomePage));
            }
            else
            {
                new MessageDialog(resp.Message, "Problem loging in.").ShowAsync();
            }
        }
    }
}