using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Shots.Common;
using Shots.Mvvm;
using Shots.Web.Models;
using Shots.Web.Services.Interface;

namespace Shots.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private int _currentVisibleIndex;
        private UserListWithSuggestionResponse _feed;
        private bool _isLoading;
        private string _statusMessage;
        private UserInfo _userInfo;

        public ProfileViewModel(IShotsService service)
        {
            Service = service;

            FollowCommand = new Command(FollowExecute);

            if (IsInDesignMode)
                SetUser(service.CurrentUser);
        }

        public Command FollowCommand { get; set; }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        public UserInfo UserInfo
        {
            get { return _userInfo; }
            set { Set(ref _userInfo, value); }
        }

        public IShotsService Service { get; set; }

        public UserListWithSuggestionResponse Feed
        {
            get { return _feed; }
            set { Set(ref _feed, value); }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set { Set(ref _statusMessage, value); }
        }

        public int CurrentVisibleIndex
        {
            get { return _currentVisibleIndex; }
            set { Set(ref _currentVisibleIndex, value); }
        }

        private async void FollowExecute()
        {
            bool add;

            if (UserInfo.Privacy)
            {
                UserInfo.IsRequested = !UserInfo.IsRequested;
                add = UserInfo.IsRequested;
            }
            else
            {
                UserInfo.IsFriend = !UserInfo.IsFriend;
                add = UserInfo.IsFriend;
            }

            var resp = await Service.ToggleFriend(UserInfo.Id, add);

            if (resp.Status == Status.Success) return;

            CurtainPrompt.ShowError(resp.Message);
            if (UserInfo.Privacy)
                UserInfo.IsRequested = !UserInfo.IsRequested;
            else
                UserInfo.IsFriend = !UserInfo.IsFriend;
        }

        public async void SetUser(string name)
        {
            IsLoading = true;
            var resp = await Service.GetUserByNameAsync(name);
            IsLoading = false;
            if (resp.Status != Status.Success)
            {
                CurtainPrompt.ShowError(resp.Message);
                return;
            }

            SetUser(resp.UserInfo);
        }

        public async void SetUser(UserInfo info, bool loadFeed = true)
        {
            StatusMessage = null;
            UserInfo = info;

            // Don't load feeds for private profiles.
            if (UserInfo.Privacy && !UserInfo.IsFriend)
            {
                StatusMessage = "This profile is private.";
                return;
            }

            if (!loadFeed) return;

            Feed = await Service.GetUserListAsync(info.Id);

            if (Feed.Status != Status.Success)
                CurtainPrompt.ShowError(Feed.Message);
        }

        private bool TryToRestoreState(NavigationMode mode, IReadOnlyDictionary<string, object> state)
        {
            object feedState, currentVisibleIndexState, userState;

            if (mode == NavigationMode.Back && Feed == null
                && state.TryGetValue("userInfo", out userState)
                && state.TryGetValue("feedList", out feedState)
                && state.TryGetValue("currentVisibleIndex", out currentVisibleIndexState))
            {
                // The app was supended and terminated, so we resume from the state
                Feed = feedState as UserListWithSuggestionResponse;
                SetUser(userState as UserInfo, false);

                // Need to reattach
                if (Feed != null)
                    Service.AttachLoadMore(Feed);

                // Scroll the list were we left off
                // JSON.NET serializes ints as int64; Have to covert it to int32
                CurrentVisibleIndex = Convert.ToInt32(currentVisibleIndexState);
            }

            return UserInfo != null;
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            if (!TryToRestoreState(mode, state))
                SetUser(parameter as string);
        }

        public override void OnNavigatedFrom(bool suspending, Dictionary<string, object> state)
        {
            state["userInfo"] = UserInfo;
            state["feedList"] = Feed;
            state["currentVisibleIndex"] = CurrentVisibleIndex;
        }
    }
}