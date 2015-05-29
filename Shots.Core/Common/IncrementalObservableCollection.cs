using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Shots.Core.Common
{
    public class IncrementalObservableCollection<T> : OptimizedObservableCollection<T>, ISupportIncrementalLoading
    {
        public Func<uint, IAsyncOperation<LoadMoreItemsResult>> LoadMoreItemsFunc;

        public IncrementalObservableCollection()
        {
        }

        public IncrementalObservableCollection(IEnumerable<T> collection) : base(collection)
        {
            
        }

        public IncrementalObservableCollection(bool hasMoreItems,
            Func<uint, IAsyncOperation<LoadMoreItemsResult>> loadMoreItemsFunc)
        {
            HasMoreItems = hasMoreItems;
            LoadMoreItemsFunc = loadMoreItemsFunc;
        }

        #region ISupportIncrementalLoading Members

        public bool HasMoreItems { get; set; }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count) => LoadMoreItemsFunc?.Invoke(count);

        #endregion
    }
}