using Windows.UI.Xaml;

namespace Shots.Views
{
    public sealed partial class WelcomePage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (LoginPage));
        }
    }
}