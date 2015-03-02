using Windows.UI.Xaml.Controls;

namespace Shots.Utilities
{
    public class ScrollListView : ListView
    {
        private ScrollViewer _scroll;

        public ScrollViewer ScrollViewer
        {
            get { return _scroll ?? (_scroll = this.GetScrollViewer()); }
        }
    }
}