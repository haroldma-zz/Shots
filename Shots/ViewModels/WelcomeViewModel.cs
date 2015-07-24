using System;
using Shots.Common;
using Shots.Controls;
using Shots.Core.Extensions;
using Shots.Mvvm;
using Shots.Services.NavigationService;
using Shots.Tools;
using Shots.Views;
using Shots.Web.Models;
using Shots.Web.Services.Interface;

namespace Shots.ViewModels
{
    internal class WelcomeViewModel : ViewModelBase
    {
        public enum WelcomeMode
        {
            Selection,
            Login,
            SignUp
        }

        private readonly INavigationService _navigationService;
        private readonly IShotsService _shotsService;
        private WelcomeMode _currentMode;
        private bool _isBusy;
        private string _signUpToken;

        public WelcomeViewModel(IShotsService shotsService, INavigationService navigationService)
        {
            _shotsService = shotsService;
            _navigationService = navigationService;
            LoginCommand = new Command(ExecuteLogin);
            SignUpCommand = new Command(ExecuteSignUp);
        }

        public Command SignUpCommand { get; set; }
        public Command LoginCommand { get; }

        public WelcomeMode CurrentMode
        {
            get { return _currentMode; }
            set { Set(ref _currentMode, value); }
        }

        public string LoginUsername { get; set; }
        public string LoginPassword { get; set; }
        public string SignUpUsername { get; set; }
        public string SignUpPassword { get; set; }
        public string SignUpName { get; set; }
        public string SignUpEmail { get; set; }

        // The default birthdate is set for 18 years ago. Should make it easier for the average user to select their date.
        public DateTimeOffset SignUpBirthDate { get; set; } = new DateTime(DateTimeOffset.Now.Year - 18,
            DateTimeOffset.Now.Month, DateTimeOffset.Now.Day);

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        private async void ExecuteSignUp()
        {
            if (CurrentMode == WelcomeMode.SignUp)
            {
                if (StringExtensions.IsAnyNullOrEmpty(SignUpUsername, SignUpPassword, SignUpEmail, SignUpName))
                {
                    CurtainPrompt.ShowError("Make sure to fill up everything.");
                    return;
                }

                if (string.IsNullOrEmpty(_signUpToken))
                {
                    // Get phone number
                    var phone = await PopupInputHelper.GetInputAsync(new PhoneNumerInput());
                    if (string.IsNullOrEmpty(phone)) return;

                    // Send verification code
                    IsBusy = true;
                    var smsResults = await _shotsService.SendSmsVerificationCode("", phone);
                    IsBusy = false;
                    if (smsResults.Status != Status.Success)
                    {
                        CurtainPrompt.ShowError(smsResults.Message);
                        return;
                    }

                    // get the code from user
                    var code = await PopupInputHelper.GetInputAsync(new VerificationCodeInput());
                    if (string.IsNullOrEmpty(code)) return;

                    if (smsResults.Status != Status.Success)
                    {
                        CurtainPrompt.ShowError(smsResults.Message);
                        return;
                    }

                    // verify it
                    IsBusy = true;
                    var verifyResults = await _shotsService.VerifyCode(code, smsResults.SignUpToken);

                    if (verifyResults.Status != Status.Success)
                    {
                        IsBusy = false;
                        CurtainPrompt.ShowError(smsResults.Message);
                        return;
                    }

                    _signUpToken = smsResults.SignUpToken;
                }

                IsBusy = true;
                var results =
                    await
                        _shotsService.RegisterAsync(_signUpToken, SignUpUsername, SignUpPassword, SignUpEmail, SignUpName,
                            SignUpBirthDate.Date, null);
                IsBusy = false;

                if (results.Status == Status.Success)
                    _navigationService.Navigate(typeof (MainPage));
                else
                    CurtainPrompt.ShowError(results.Message ?? "Problem signing up.");
            }
            else
                CurrentMode = WelcomeMode.SignUp;
        }

        private async void ExecuteLogin()
        {
            if (CurrentMode == WelcomeMode.Login)
            {
                if (StringExtensions.IsAnyNullOrEmpty(LoginUsername, LoginPassword))
                {
                    CurtainPrompt.ShowError("Make sure to fill up everything.");
                    return;
                }

                IsBusy = true;
                var results = await _shotsService.LoginAsync(LoginUsername, LoginPassword);
                IsBusy = false;

                if (results.Status == Status.Success)
                    _navigationService.Navigate(typeof (MainPage));
                else
                    CurtainPrompt.ShowError(results.Message ?? "Problem logging you in.");
            }
            else
                CurrentMode = WelcomeMode.Login;
        }
    }
}