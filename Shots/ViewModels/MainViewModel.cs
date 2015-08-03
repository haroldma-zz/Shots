using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Shots.Helpers;
using Shots.Mvvm;
using Shots.Services.NavigationService;
using Shots.Views;
using Shots.Web.Models;
using Shots.Web.Services.Interface;

namespace Shots.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private int _currentVisibleIndex;
        private string _errorMessage;
        private HomeListResponse _homeList;
        private bool _isLoading;

        public MainViewModel(IShotsService shotsService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            ShotsService = shotsService;
            LogoutCommand = new Command(LogoutExecute);
            ReviewCommand = new Command(ReviewExecute);
            ContactCommand = new Command(ContactExecute);
            PeopleCommand = new Command(PeopleExecute);

            if (!IsInDesignMode) return;
            OnNavigatedTo(null, NavigationMode.New, null);
        }

        public Command PeopleCommand { get; set; }
        public Command ContactCommand { get; set; }
        public Command ReviewCommand { get; set; }
        public Command LogoutCommand { get; set; }
        public IShotsService ShotsService { get; }

        public HomeListResponse HomeList
        {
            get { return _homeList; }
            private set { Set(ref _homeList, value); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        public int CurrentVisibleIndex
        {
            get { return _currentVisibleIndex; }
            set { Set(ref _currentVisibleIndex, value); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { Set(ref _errorMessage, value); }
        }

        private void PeopleExecute()
        {
            _navigationService.Navigate(typeof (SearchPage));
        }

        private async void ContactExecute()
        {
            await Launcher.LaunchUriAsync(new Uri("mailto:help@zumicts.com"));
        }

        private async void ReviewExecute()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

        private void LogoutExecute()
        {
            ShotsService.Logout();
            _navigationService.Navigate(typeof (WelcomePage));
        }

        private bool TryToRestoreState(NavigationMode mode, IReadOnlyDictionary<string, object> state)
        {
            object homeListState, currentVisibleIndexState;

            if (mode == NavigationMode.Back && HomeList == null
                && state.TryGetValue("homeList", out homeListState)
                && state.TryGetValue("currentVisibleIndex", out currentVisibleIndexState))
            {
                // The app was supended and terminated, so we resume from the state
                HomeList = homeListState as HomeListResponse;

                if (HomeList != null)
                {
                    // Need to reattach
                    ShotsService.AttachLoadMore(HomeList);

                    // Scroll the list were we left off
                    // JSON.NET serializes ints as int64; Have to covert it to int32
                    CurrentVisibleIndex = Convert.ToInt32(currentVisibleIndexState);
                }
            }

            return HomeList != null;
        }

        public override sealed async void OnNavigatedTo(object parameter, NavigationMode mode,
            Dictionary<string, object> state)
        {
            ErrorMessage = null;
            StatusBarHelper.ForegroundColor = Colors.Black;

            if (!TryToRestoreState(mode, state))
            {
                IsLoading = true;
                HomeList = await ShotsService.GetHomeListAsync();
                IsLoading = false;

                if (HomeList.Status != Status.Success)
                    ErrorMessage = HomeList.Message;
            }
            else
            {
                CurrentOnSizeChanged(null, null);
            }

            Window.Current.SizeChanged += CurrentOnSizeChanged;
        }

        public override void OnNavigatedFrom(bool suspending, Dictionary<string, object> state)
        {
            Window.Current.SizeChanged -= CurrentOnSizeChanged;
            StatusBarHelper.ForegroundColor = Colors.White;
            state["homeList"] = HomeList;
            state["currentVisibleIndex"] = CurrentVisibleIndex;
        }

        private void CurrentOnSizeChanged(object sender, WindowSizeChangedEventArgs windowSizeChangedEventArgs)
        {
            foreach (var shotItem in HomeList.Items)
            {
                shotItem.Resource?.RefreshRatio();
            }
        }
    }
}