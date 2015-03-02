using Windows.UI.Xaml;
using Shots.Api.Models;
using Shots.Utilities;

namespace Shots.Controls
{
    public sealed partial class ShotItemViewer
    {
        public static readonly DependencyProperty ScrollListViewProperty = DependencyProperty.RegisterAttached(
            "ScrollListView",
            typeof (ScrollListView),
            typeof (ShotItemViewer),
            new PropertyMetadata(null, null));

        public ShotItemViewer()
        {
            InitializeComponent();
        }

        public ScrollListView ScrollListView
        {
            get { return (ScrollListView) GetValue(ScrollListViewProperty); }
            set { SetValue(ScrollListViewProperty, value); }
        }

        public static ScrollListView GetScrollListView(DependencyObject element)
        {
            return (ScrollListView) element.GetValue(ScrollListViewProperty);
        }

        public static void SetScrollListView(DependencyObject element, ScrollListView value)
        {
            element.SetValue(ScrollListViewProperty, value);
        }

        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the current like state
            ShotItem.Resource.IsLike = !ShotItem.Resource.IsLike;

            // Now toggle the like over at the api
            var resp = await App.Locator.ShotsService.LikeShotItemAsync(ShotItem.Resource.Id, ShotItem.Resource.IsLike);

            // Something happened
            if (resp.Status != Status.Success)
            {
                // Reset back
                ShotItem.Resource.IsLike = !ShotItem.Resource.IsLike;

                // TODO report error to user
            }
        }

        public ShotItem ShotItem { get { return DataContext as ShotItem; } }
    }
}