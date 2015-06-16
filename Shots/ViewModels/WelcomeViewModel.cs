using System;
using Shots.Common;
using Shots.Mvvm;
using Shots.Services.NavigationService;
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

        private readonly NavigationService _navigationService;
        private readonly IShotsService _shotsService;
        private WelcomeMode _currentMode;
        private bool _isBusy;

        public WelcomeViewModel(IShotsService shotsService, NavigationService navigationService)
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
        public string SignUpFirstName { get; set; }
        public string SignUpLastName { get; set; }
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
                IsBusy = true;
                var results = await _shotsService.RegisterAsync(SignUpUsername, SignUpPassword, SignUpEmail, SignUpFirstName,
                        SignUpLastName, SignUpBirthDate.Date, null);
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