using System.Collections.Generic;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
        private HomeListResponse _homeList;
        private List<UserInfo> _searchResults;

        public MainViewModel(IShotsService shotsService)
        {
            ShotsService = shotsService;
            ShowingCommand = new Command(ShowingExecute);
            HidingCommand = new Command(HidingExecute);
            KeyDownCommand = new Command<KeyRoutedEventArgs>(KeyDownExecute);

            if (!IsInDesignMode) return;
            OnNavigatedTo(null, NavigationMode.New, null);
            KeyDownExecute(null);
        }

        public Command<KeyRoutedEventArgs> KeyDownCommand { get; set; }
        public Command HidingCommand { get; }
        public Command ShowingCommand { get; }
        public IShotsService ShotsService { get; }

        public HomeListResponse HomeList
        {
            get { return _homeList; }
            private set { Set(ref _homeList, value); }
        }

        public List<UserInfo> SearchResults
        {
            get { return _searchResults; }
            set { Set(ref _searchResults, value); }
        }

        private async void KeyDownExecute(KeyRoutedEventArgs e)
        {
            if (!IsInDesignMode && e.Key != VirtualKey.Enter) return;

            var textBox = IsInDesignMode ? new TextBox() : (TextBox) e.OriginalSource;
            var term = textBox.Text;
            textBox.IsEnabled = false;

            var response = await ShotsService.SearchUsersAsync(term);

            textBox.IsEnabled = true;

            if (response.Status != Status.Success)
            {
                CurtainPrompt.ShowError("Problem executing search.");
                return;
            }

            SearchResults = response.Users;
        }

        public void ShowingExecute()
        {
            StatusBarHelper.Instance.ForegroundColor = Colors.Black;
        }

        public void HidingExecute()
        {
            StatusBarHelper.Instance.ForegroundColor = Colors.White;
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
                HomeList = await ShotsService.GetHomeListAsync();
        }

        public override void OnNavigatedFrom(bool suspending, Dictionary<string, object> state)
        {
            state["homeList"] = HomeList;
        }
    }
}