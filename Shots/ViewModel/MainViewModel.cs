using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Shots.Api;
using Shots.Api.Models;
using Shots.Common;

namespace Shots.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private PageInfo _pageInfo;
        private List<UserInfo> _searchResults;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IShotsService service)
        {
            Service = service;

            if (IsInDesignMode)
                KeyDownExecute(null);

            KeyDownCommand = new RelayCommand<KeyRoutedEventArgs>(KeyDownExecute);

            _pageInfo = new PageInfo {EntryCount = -1};
            Feed = new IncrementalObservableCollection<ShotItem>(
                // If the page response had zero entries, then stop loading
                () => _pageInfo != null && _pageInfo.EntryCount != 0,
                u =>
                {
                    Func<Task<LoadMoreItemsResult>> taskFunc = async () =>
                    {
                        var resp =
                            await Service.GetHomeListAsync(Feed.Count != 0 ? Feed.LastOrDefault().Resource.Id : "");

                        if (resp.Status != Status.Success) return new LoadMoreItemsResult {Count = 0};

                        _pageInfo = resp.PageInfo;

                        foreach (var item in resp.Items)
                        {
                            Feed.Add(item);
                        }
                        return new LoadMoreItemsResult {Count = (uint) resp.Items.Count};
                    };
                    var loadMorePostsTask = taskFunc();
                    return loadMorePostsTask.AsAsyncOperation();
                });
        }

        private async void KeyDownExecute(KeyRoutedEventArgs e)
        {
            if (!IsInDesignMode && e.Key != VirtualKey.Enter) return;

            var textBox = IsInDesignMode ? null : e.OriginalSource as TextBox;
            var term = textBox == null ? "" : textBox.Text;

            if (textBox != null)
                textBox.IsEnabled = false;

            var response = await Service.SearchUsersAsync(term);

            if (textBox != null)
                textBox.IsEnabled = true;

            if (response.Status != Status.Success) return;
            // TODO: report error

            SearchResults = response.Users;
        }

        public RelayCommand<KeyRoutedEventArgs> KeyDownCommand { get; set; }

        public IShotsService Service { get; private set; }

        public List<UserInfo> SearchResults
        {
            get { return _searchResults; }
            set { Set(ref _searchResults, value); }
        }

        public IncrementalObservableCollection<ShotItem> Feed { get; set; }
    }
}