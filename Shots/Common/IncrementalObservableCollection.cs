﻿#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

#endregion

namespace Shots.Common
{
    public class IncrementalObservableCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        public Func<bool> HasMoreItemsFunc;
        public Func<uint, IAsyncOperation<LoadMoreItemsResult>> LoadMoreItemsFunc;

        public IncrementalObservableCollection()
        {
        }

        public IncrementalObservableCollection(Func<bool> hasMoreItemsFunc,
            Func<uint, IAsyncOperation<LoadMoreItemsResult>> loadMoreItemsFunc)
        {
            HasMoreItemsFunc = hasMoreItemsFunc;
            LoadMoreItemsFunc = loadMoreItemsFunc;
        }

        #region ISupportIncrementalLoading Members

        public bool HasMoreItems
        {
            get { return HasMoreItemsFunc(); }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadMoreItemsFunc(count);
        }

        #endregion
    }
}