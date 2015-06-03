using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Shots.Helpers;
using Shots.Mvvm;
using Shots.Web.Models;
using Shots.Web.Services.Interface;

namespace Shots.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private HomeListResponse _homeList;

        public MainViewModel(IShotsService shotsService)
        {
            ShotsService = shotsService;
            ShowingCommand = new Command(ShowingExecute);
            HidingCommand = new Command(HidingExecute);
            KeyDownCommand = new Command<KeyRoutedEventArgs>(KeyDownExecute);

            if (IsInDesignMode)
                OnNavigatedTo(null, NavigationMode.New, null);
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

        private void KeyDownExecute(KeyRoutedEventArgs e)
        {
        }

        private void ShowingExecute()
        {
            StatusBarHelper.Instance.ForegroundColor = Colors.Black;
        }

        private void HidingExecute()
        {
            StatusBarHelper.Instance.ForegroundColor = Colors.White;
        }

        public override sealed async void OnNavigatedTo(string parameter, NavigationMode mode,
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

            if (HomeList != null)
                return;
            HomeList = await ShotsService.GetHomeListAsync();
        }

        public override void OnNavigatedFrom(bool suspending, Dictionary<string, object> state)
        {
            state["homeList"] = HomeList;
        }
    }
}