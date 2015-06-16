using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Shots.ViewModels;

namespace Shots.Views
{
    public sealed partial class WelcomePage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter) return;

            var textBox = sender as TextBox;
            const string suffix = "TextBox";

            if (textBox == null)
            {
                DatePicker.Focus(FocusState.Programmatic);
                return;
            }

            if (textBox.Name == $"LoginUsername{suffix}")
                LoginPasswordTextBox.Focus(FocusState.Programmatic);
            else if (textBox.Name == $"FirstName{suffix}")
                LastNameTextBox.Focus(FocusState.Programmatic);
            else if (textBox.Name == $"LastName{suffix}")
                UsernameTextBox.Focus(FocusState.Programmatic);
            else if (textBox.Name == $"Username{suffix}")
                EmailTextBox.Focus(FocusState.Programmatic);
            else if (textBox.Name == $"Email{suffix}")
                PasswordBox.Focus(FocusState.Programmatic);
        }

        private void LoginPasswordTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter) return;

            (DataContext as WelcomeViewModel)?.LoginCommand.Execute(null);
        }
    }
}