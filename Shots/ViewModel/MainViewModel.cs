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
        private readonly IShotsService _service;
        private PageInfo _pageInfo;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IShotsService service)
        {
            _service = service;
            _pageInfo = new PageInfo{LastId = ""};
            Feed = new IncrementalObservableCollection<ShotItem>(
                () => _pageInfo != null && _pageInfo.LastId != null,
                u =>
                {
                    Func<Task<LoadMoreItemsResult>> taskFunc = async () =>
                    {
                        var resp = await _service.GetHomeListAsync(Feed.Count != 0 ? Feed.LastOrDefault().Resource.Id : "");

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

        public IncrementalObservableCollection<ShotItem> Feed { get; set; }
    }
}