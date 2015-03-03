using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using GalaSoft.MvvmLight;
using Shots.Api;
using Shots.Api.Models;
using Shots.Common;

namespace Shots.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private PageInfo _pageInfo;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IShotsService service)
        {
            Service = service;
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

        public IShotsService Service { get; private set; }
        public IncrementalObservableCollection<ShotItem> Feed { get; set; }
    }
}