using GalaSoft.MvvmLight;
using Shots.Api;
using Shots.Api.Models;
using Shots.Common;

namespace Shots.ViewModel
{
    public class ProfileViewModel : ViewModelBase
    {
        private IncrementalObservableCollection<ShotItem> _feed;
        private SimpleUserInfo _userInfo;

        public ProfileViewModel(IShotsService service)
        {
            Service = service;

            if (IsInDesignMode)
                SetUser(service.CurrentUser);
        }

        public SimpleUserInfo UserInfo
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

        public void SetUser(SimpleUserInfo info)
        {
            UserInfo = info;
        }
    }
}