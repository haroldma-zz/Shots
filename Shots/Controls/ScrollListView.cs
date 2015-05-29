using Windows.UI.Xaml.Controls;
using Shots.Extensions;

namespace Shots.Controls
{
    /// <summary>
    /// Just a listview with the added ScrollViewer property.
    /// </summary>
    public class ScrollListView : ListView
    {
        private ScrollViewer _scroll;

        public ScrollViewer ScrollViewer => _scroll ?? (_scroll = this.GetScrollViewer());
    }
}