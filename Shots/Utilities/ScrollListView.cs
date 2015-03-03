using Windows.UI.Xaml.Controls;

namespace Shots.Utilities
{
    /// <summary>
    /// Just a listview with the added ScrollViewer property.
    /// </summary>
    public class ScrollListView : ListView
    {
        private ScrollViewer _scroll;

        public ScrollViewer ScrollViewer
        {
            get { return _scroll ?? (_scroll = this.GetScrollViewer()); }
        }
    }
}