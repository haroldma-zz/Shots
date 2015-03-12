using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Shots.Api;
using Shots.Api.Models;
using Shots.Common;

namespace Shots.ViewModel
{
    public class ProfileViewModel : ViewModelBase
    {
        private IncrementalObservableCollection<ShotItem> _feed;
        private UserInfo _userInfo;
        private PageInfo _pageInfo;

        public ProfileViewModel(IShotsService service, UserInfo info) : this(service)
        {
            SetUser(info);
        }

        public ProfileViewModel(IShotsService service, string name)
            : this(service)
        {
            SetUser(name);
        }

        [PreferredConstructor]
        public ProfileViewModel(IShotsService service)
        {
            Service = service;

            FollowCommand = new RelayCommand(FollowExecute);

            if (IsInDesignMode)
                SetUser(service.CurrentUser);
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

            // TODO: Report error here
            if (UserInfo.Privacy)
                UserInfo.IsRequested = !UserInfo.IsRequested;
            else
                UserInfo.IsFriend = !UserInfo.IsFriend;
        }

        public RelayCommand FollowCommand { get; set; }

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

        public async void SetUser(string name)
        {
            var resp = await Service.GetUserByNameAsync(name);
            if (resp.Status != Status.Success)
            {
                // TODO: report
                return;
            }

            SetUser(resp.UserInfo);
        }

        public void SetUser(UserInfo info)
        {
            UserInfo = info;

            _pageInfo = new PageInfo { EntryCount = -1 };
            Feed = new IncrementalObservableCollection<ShotItem>(
                // If the page response had zero entries, then stop loading
                () => _pageInfo != null && _pageInfo.EntryCount != 0,
                u =>
                {
                    Func<Task<LoadMoreItemsResult>> taskFunc = async () =>
                    {
                        var resp =
                            await Service.GetUserListAsync(UserInfo.Id, Feed.Count != 0 ? Feed.LastOrDefault().Resource.Id : "");

                        if (resp.Status != Status.Success) return new LoadMoreItemsResult { Count = 0 };

                        _pageInfo = resp.PageInfo;

                        foreach (var item in resp.Items)
                        {
                            Feed.Add(item);
                        }
                        return new LoadMoreItemsResult { Count = (uint)resp.Items.Count };
                    };
                    var loadMorePostsTask = taskFunc();
                    return loadMorePostsTask.AsAsyncOperation();
                });
        }
    }
}