using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Shots.Extensions;

namespace Shots.Controls
{
    /// <summary>
    ///     Just a listview with the added VerticalOffset property.
    /// </summary>
    public class ScrollListView : ListView
    {
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset", typeof (double), typeof (ScrollListView),
                new PropertyMetadata(0, VerticalOffsetPropertyChanged));

        public static readonly DependencyProperty CurrentVisibleIndexProperty =
            DependencyProperty.RegisterAttached("CurrentVisibleIndex", typeof (int), typeof (ScrollListView),
                new PropertyMetadata(-1, FirstVisibleItemPropertyChanged));

        private ScrollViewer _scroll;

        public ScrollListView()
        {
            Loaded += (s, e) =>
            {
                ScrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
                ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            };
        }

        public ScrollViewer ScrollViewer => _scroll ?? (_scroll = this.GetScrollViewer());

        public double VerticalOffset
        {
            get { return GetVerticalOffset(this); }

            set { SetVerticalOffset(this, value); }
        }

        public int CurrentVisibleIndex
        {
            get { return GetFirstVisibleItem(this); }

            set { SetFirstVisibleItem(this, value); }
        }

        private static void FirstVisibleItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var list = (ScrollListView) d;
            var index = e.NewValue as int?;

            RoutedEventHandler handler = null;
            Action action = () =>
            {
                // ReSharper disable AccessToModifiedClosure
                if (handler != null)
                    list.Loaded -= handler;
                // ReSharper restore AccessToModifiedClosure

                if (index != null
                    && index != list.GetFirstVisibleIndex()
                    && list.Items != null
                    && index < list.Items.Count)
                    list.ScrollIntoView(list.Items[index.Value]);
            };
            handler = (s, ee) => action();

            if (list.ScrollViewer == null)
                list.Loaded += handler;
            else
                action();
        }

        private int GetFirstVisibleIndex()
        {
            var panel = ItemsPanelRoot as ItemsStackPanel;
            if (panel != null && Items != null)
            {
                var middleValue = (int) Math.Round((panel.FirstVisibleIndex + panel.LastVisibleIndex)/2.0f);
                var diff = panel.LastVisibleIndex - panel.FirstVisibleIndex;

                return diff == 1 ? panel.FirstVisibleIndex : middleValue;
            }
            return -1;
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            CurrentVisibleIndex = GetFirstVisibleIndex();
            VerticalOffset = ScrollViewer.VerticalOffset;
        }

        private static void VerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var list = (ScrollListView) d;

            RoutedEventHandler handler = null;
            Action action = () =>
            {
                // ReSharper disable AccessToModifiedClosure
                if (handler != null)
                    list.Loaded -= handler;
                // ReSharper restore AccessToModifiedClosure

                if (list.VerticalOffset != list.ScrollViewer.VerticalOffset)
                    list.ScrollViewer.ChangeView(null, (double) e.NewValue, null, true);
            };
            handler = (s, ee) => action();

            if (list.ScrollViewer == null)
                list.Loaded += handler;
            else
                action();
        }

        public static void SetVerticalOffset(DependencyObject element, double value)
        {
            element.SetValue(VerticalOffsetProperty, value);
        }

        public static double GetVerticalOffset(DependencyObject element)
        {
            return (double) element.GetValue(VerticalOffsetProperty);
        }

        public static void SetFirstVisibleItem(DependencyObject element, int value)
        {
            element.SetValue(CurrentVisibleIndexProperty, value);
        }

        public static int GetFirstVisibleItem(DependencyObject element)
        {
            return (int) element.GetValue(CurrentVisibleIndexProperty);
        }
    }
}