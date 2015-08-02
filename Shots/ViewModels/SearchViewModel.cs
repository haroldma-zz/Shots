using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Shots.Common;
using Shots.Mvvm;
using Shots.Services.NavigationService;
using Shots.Views;
using Shots.Web.Models;
using Shots.Web.Services.Interface;

namespace Shots.ViewModels
{
    internal class SearchViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IShotsService _shotsService;
        private bool _isLoading;
        private List<UserInfo> _searchResults;
        private string _searchText;

        public SearchViewModel(IShotsService shotsService, INavigationService navigationService)
        {
            _shotsService = shotsService;
            _navigationService = navigationService;
            KeyDownCommand = new Command<KeyRoutedEventArgs>(KeyDownExecute);
            ItemClickCommand = new Command<ItemClickEventArgs>(ItemClickExecute);

            if (IsInDesignMode)
                KeyDownExecute(null);
        }

        public Command<ItemClickEventArgs> ItemClickCommand { get; set; }
        public Command<KeyRoutedEventArgs> KeyDownCommand { get; set; }

        public List<UserInfo> SearchResults
        {
            get { return _searchResults; }
            set { Set(ref _searchResults, value); }
        }

        public string SearchText
        {
            get { return _searchText; }
            set { Set(ref _searchText, value); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        private void ItemClickExecute(ItemClickEventArgs e)
        {
            var user = (UserInfo) e.ClickedItem;
            _navigationService.Navigate(typeof (ProfilePage), user.Username);
        }

        private async void KeyDownExecute(KeyRoutedEventArgs e)
        {
            if (!IsInDesignMode && e.Key != VirtualKey.Enter) return;

            IsLoading = true;
            var response = await _shotsService.SearchUsersAsync(SearchText);
            IsLoading = false;

            if (response.Status != Status.Success)
                CurtainPrompt.ShowError(response.Message);
            else
                SearchResults = response.Users;
        }
    }
}