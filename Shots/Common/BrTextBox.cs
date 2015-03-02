using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Shots.Common
{
    public sealed class BrTextBox : TextBox
    {
        private const string StateCollapse = "Collapse";
        private const string StateExpand = "Expand";

        public static readonly DependencyProperty PasswordModeProperty = DependencyProperty.Register(
            "PasswordMode", typeof (bool), typeof (BrTextBox), new PropertyMetadata(false));

        private bool _hasFocus;

        public BrTextBox()
        {
            DefaultStyleKey = typeof (BrTextBox);
            GotFocus += BrTextBox_GotFocus;
            LostFocus += BrTextBox_LostFocus;
            Loaded += BrTextBox_Loaded;
            TextChanged += BrTextBox_TextChanged;
        }

        public bool PasswordMode
        {
            get { return (bool) GetValue(PasswordModeProperty); }
            set { SetValue(PasswordModeProperty, value); }
        }

        public string PasswordText { get; private set; }
        private bool _ignore;

        private async void BrTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // In case viewmodels clear the string
            if (string.IsNullOrWhiteSpace(Text) && _hasFocus == false)
            {
                VisualStateManager.GoToState(this, StateCollapse, true);
            }

            else if (PasswordMode)
            {
                if (_ignore)
                {
                    _ignore = false;
                    return;
                }

                // Mask text
                var newStr = Text.Where(p => p != '*').ToArray();
                var charCount = newStr.Length;
                PasswordText += new string(newStr);

                var str = new string(Text.Where(p => p == '*').ToArray());

                for (var i = 0; i < charCount; i++)
                {
                    str += "*";
                }

                _ignore = true;
                if (charCount == 0)
                {
                    Text = str.Substring(0, str.Length);
                    PasswordText = PasswordText.Substring(0, str.Length);
                }
                else
                {
                    Text = str;
                }
                Select(Text.Length, 0);
            }
        }

        private void BrTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, string.IsNullOrWhiteSpace(Text) ? StateCollapse : StateExpand, true);
        }

        private void BrTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _hasFocus = false;

            if (string.IsNullOrWhiteSpace(Text))
            {
                VisualStateManager.GoToState(this, StateCollapse, true);
            }
        }

        private void BrTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _hasFocus = true;

            if (string.IsNullOrWhiteSpace(Text))
            {
                VisualStateManager.GoToState(this, StateExpand, true);
            }
        }
    }
}