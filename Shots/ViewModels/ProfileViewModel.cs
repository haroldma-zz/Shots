using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Shots.Common;
using Shots.Core.Common;
using Shots.Mvvm;
using Shots.Web.Models;
using Shots.Web.Services.Interface;

namespace Shots.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private IncrementalObservableCollection<ShotItem> _feed;
        private bool _isLoading;
        private UserInfo _userInfo;
        private string _statusMessage;

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

        public IncrementalObservableCollection<ShotItem> Feed
        {
            get { return _feed; }
            set { Set(ref _feed, value); }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set { Set(ref _statusMessage, value); }
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

        public async void SetUser(UserInfo info)
        {
            StatusMessage = null;
            UserInfo = info;

            // Don't load feeds for private profiles.
            if (UserInfo.Privacy && !UserInfo.IsFriend)
            {
                StatusMessage = "This profile is private.";
                return;
            }

            var resp = await Service.GetUserListAsync(info.Id);

            if (resp.Status != Status.Success)
            {
                CurtainPrompt.ShowError("Problem loading profile.");
                return;
            }
            Feed = resp.Items;
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            SetUser(parameter as string);
        }
    }
}