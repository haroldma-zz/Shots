using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Shots.Api.Models;

namespace Shots.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordTextBox.PasswordText;

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
