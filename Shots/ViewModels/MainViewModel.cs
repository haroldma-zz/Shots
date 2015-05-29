using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shots.Mvvm;
using Shots.Web.Models;
using Shots.Web.Services.Interface;

namespace Shots.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly IShotsService _shotsService;
        private HomeListResponse _homeList;

        public MainViewModel(IShotsService shotsService)
        {
            _shotsService = shotsService;

            if (IsInDesignMode)
                OnNavigatedTo(null, NavigationMode.New, null);
        }

        public HomeListResponse HomeList
        {
            get { return _homeList; }
            set { Set(ref _homeList, value); }
        }

        public override sealed async void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            object homeListState;
            if (mode == NavigationMode.Back && HomeList == null 
                && state.TryGetValue("homeList", out homeListState))
            {
                // The app was supended and terminated, so we resume from the state
                HomeList = homeListState as HomeListResponse;

                // Need to reattach
                _shotsService.AttachLoadMore(HomeList);
            }

            if (HomeList != null)
                return;
            HomeList = await _shotsService.GetHomeListAsync();
        }

        public override void OnNavigatedFrom(bool suspending, Dictionary<string, object> state)
        {
            state["homeList"] = HomeList;
        }
    }
}