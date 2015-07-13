using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Navigation;
using Shots.Common;
using Shots.Helpers;
using Shots.Mvvm;
using Shots.Web.Models;
using Shots.Web.Services.Interface;

namespace Shots.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private int _currentVisibleIndex;
        private HomeListResponse _homeList;
        private bool _isLoading;

        public MainViewModel(IShotsService shotsService)
        {
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

        private bool TryToRestoreState(NavigationMode mode, IReadOnlyDictionary<string, object> state)
        {
            object homeListState, currentVisibleIndexState;

            if (mode == NavigationMode.Back && HomeList == null
                && state.TryGetValue("homeList", out homeListState)
                && state.TryGetValue("currentVisibleIndex", out currentVisibleIndexState))
            {
                // The app was supended and terminated, so we resume from the state
                HomeList = homeListState as HomeListResponse;

                // Need to reattach
                if (HomeList != null)
                    ShotsService.AttachLoadMore(HomeList);

                // Scroll the list were we left off
                // JSON.NET serializes ints as int64; Have to covert it to int32
                CurrentVisibleIndex = Convert.ToInt32(currentVisibleIndexState);
            }

            return HomeList != null;
        }

        public override sealed async void OnNavigatedTo(object parameter, NavigationMode mode,
            Dictionary<string, object> state)
        {
            StatusBarHelper.Instance.ForegroundColor = Colors.Black;
            if (!TryToRestoreState(mode, state))
            {
                IsLoading = true;
                HomeList = await ShotsService.GetHomeListAsync();
                IsLoading = false;

                if (HomeList.Status != Status.Success)
                    CurtainPrompt.ShowError(HomeList.Message);
            }
        }

        public override void OnNavigatedFrom(bool suspending, Dictionary<string, object> state)
        {
            StatusBarHelper.Instance.ForegroundColor = Colors.White;
            state["homeList"] = HomeList;
            state["currentVisibleIndex"] = CurrentVisibleIndex;
        }
    }
}