using System.Threading.Tasks;
using Windows.UI.Xaml;
using Shots.Common;
using Shots.Core.Extensions;
using Shots.Tools;

namespace Shots.Controls
{
    public sealed partial class VerificationCodeInput : IPopupInput
    {
        private TaskCompletionSource<string> _task;

        public VerificationCodeInput()
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
            var text = VerificationCode.Text.ToDigitsOnly();
            if (text.Length != 4)
            {
                CurtainPrompt.ShowError("Please enter a valid verification code (4 digits).");
            }
            else
            {
                _task.SetResult(text);
            }
        }
    }
}