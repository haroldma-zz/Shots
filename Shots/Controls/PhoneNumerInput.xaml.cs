using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Shots.Common;
using Shots.Core.Extensions;
using Shots.Tools;

namespace Shots.Controls
{
    public sealed partial class PhoneNumerInput : IPopupInput
    {
        private TaskCompletionSource<string> _task;

        public PhoneNumerInput()
        {
            InitializeComponent();
        }

        public Task<string> WaitForInputAsync()
        {
            _task = new TaskCompletionSource<string>();
            return _task.Task;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var text = PhoneNumberBox.Text;
            if (!text.IsValidPhoneNumber())
            {
                CurtainPrompt.ShowError("Please enter a valid phone number.");
            }
            else
            {
                _task.SetResult(text.ToDigitsOnly());
            }
        }
    }
}