using System.Collections.Generic;
using Windows.Phone.UI.Input;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Shots.Common;
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
        private HomeListResponse _homeList;
        private bool _isBusy;

        public MainViewModel(IShotsService shotsService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            ShotsService = shotsService;

            if (!IsInDesignMode) return;
            OnNavigatedTo(null, NavigationMode.New, null);
        }
        
        public IShotsService ShotsService { get; }

        public HomeListResponse HomeList
        {
            get { return _homeList; }
            private set { Set(ref _homeList, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        private void GoToProfileExecute()
        {
            _navigationService.Navigate(typeof (ProfilePage), ShotsService.CurrentUser.Username);
        }
        
        public override sealed async void OnNavigatedTo(object parameter, NavigationMode mode,
            Dictionary<string, object> state)
        {
            object homeListState;
            if (mode == NavigationMode.Back && HomeList == null
                && state.TryGetValue("homeList", out homeListState))
            {
                // The app was supended and terminated, so we resume from the state
                HomeList = homeListState as HomeListResponse;

                // Need to reattach
                ShotsService.AttachLoadMore(HomeList);
            }

            if (HomeList == null)
            {
                HomeList = await ShotsService.GetHomeListAsync();

                if (HomeList.Status != Status.Success)
                    CurtainPrompt.ShowError(HomeList.Message);
            }
        }

        public override void OnNavigatedFrom(bool suspending, Dictionary<string, object> state)
        {
            state["homeList"] = HomeList;
            StatusBarHelper.Instance.ForegroundColor = Colors.White;
        }
    }
}