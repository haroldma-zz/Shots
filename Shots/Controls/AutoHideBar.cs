using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Shots.Utilities;

namespace Shots.Controls
{
    [TemplateVisualState(GroupName = CommonGroupStateName, Name = ShowStateName)]
    [TemplateVisualState(GroupName = CommonGroupStateName, Name = HideStateName)]
    public class AutoHideBar : ContentControl
    {
        internal const string CommonGroupStateName = "CommonStates";
        internal const string ShowStateName = "Show";
        internal const string HideStateName = "Hide";
        private const double TopListOffsetDisplay = 10;
        private const double MinimumOffsetScrollingUp = 50;
        private const double MinimumOffsetScrollingDown = 10;
        private double _firstOffsetValue;
        private DoubleAnimation _hideAnimation;
        private double _lastOffsetValue;
        private UIElement _scroller;

        public AutoHideBar()
        {
            DefaultStyleKey = typeof (AutoHideBar);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var hideState = (GetTemplateChild(HideStateName) as VisualState);
            if (hideState != null)
                _hideAnimation = hideState.Storyboard.Children[0] as DoubleAnimation;
        }

        private void FindAndAttachScrollViewer(UIElement start)
        {
            _scroller = FindScroller(start);
            AttachScroller(_scroller);
        }


        private UIElement FindScroller(UIElement start)
        {
            UIElement target = null;

            if (IsScroller(start))
            {
                target = start;
            }
            else
            {
                var childCount = VisualTreeHelper.GetChildrenCount(start);

                for (var i = 0; i < childCount; i++)
                {
                    var el = VisualTreeHelper.GetChild(start, i) as UIElement;

                    if (IsScroller(start))
                    {
                        target = el;
                    }
                    else
                    {
                        target = FindScroller(el);
                    }

                    if (target != null)
                        break;
                }
            }

            return target;
        }

        private bool IsScroller(UIElement el)
        {
            return ((el is ScrollBar && ((ScrollBar) el).Orientation == Orientation.Vertical));
        }

        private void AttachScroller(UIElement scroller)
        {
            if (scroller is ScrollBar)
            {
                ((ScrollBar) scroller).ValueChanged += scrollbar_ValueChanged;
            }
        }

        private void DetachScroller(UIElement scroller)
        {
            if (scroller is ScrollBar)
            {
                ((ScrollBar) scroller).ValueChanged -= scrollbar_ValueChanged;
            }
        }

        private void scrollbar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            UpdateVisualState(e.NewValue);
        }

        private void UpdateVisualState(double value)
        {
            if (ShowOnTop && value <= TopListOffsetDisplay)
            {
                _firstOffsetValue = value;
                Show();
            }
            else if (_firstOffsetValue - value < -MinimumOffsetScrollingDown) // scrolling down
            {
                _firstOffsetValue = value;
                Hide();
            }
            else // scrolling up
            {
                if (_firstOffsetValue - value > MinimumOffsetScrollingUp)
                {
                    _firstOffsetValue = value;
                    Show();
                }
            }

            _lastOffsetValue = value;
        }

        private void Show()
        {
            RaiseEvent(Showing);
            VisualStateManager.GoToState(this, ShowStateName, true);
        }

        private void Hide()
        {
            RaiseEvent(Hiding);

            if (_hideAnimation != null)
                _hideAnimation.To = -ActualHeight;

            VisualStateManager.GoToState(this, HideStateName, true);
        }

        public event EventHandler<EventArgs> Showing;
        public event EventHandler<EventArgs> Hiding;

        #region ScrollControl (DependencyProperty)

        /// <summary>
        ///     A description of the property.
        /// </summary>
        public FrameworkElement ScrollControl
        {
            get { return (FrameworkElement) GetValue(ScrollControlProperty); }
            set { SetValue(ScrollControlProperty, value); }
        }

        public static readonly DependencyProperty ScrollControlProperty =
            DependencyProperty.Register("ScrollControl", typeof (FrameworkElement), typeof (AutoHideBar),
                new PropertyMetadata(null, OnScrollControlChanged));

        private static void OnScrollControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoHideBar) d).OnScrollControlChanged(e);
        }

        protected virtual void OnScrollControlChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_scroller != null)
            {
                DetachScroller(_scroller);
            }

            if (e.NewValue != null)
            {
                var el = e.NewValue as UIElement;
                FindAndAttachScrollViewer(el);
            }
        }

        #endregion

        #region ShowOnTop (DependencyProperty)

        /// <summary>
        ///     Shows the navigation bar when on top of the list
        /// </summary>
        public bool ShowOnTop
        {
            get { return (bool) GetValue(ShowOnTopProperty); }
            set { SetValue(ShowOnTopProperty, value); }
        }

        public static readonly DependencyProperty ShowOnTopProperty =
            DependencyProperty.Register("ShowOnTop", typeof (bool), typeof (AutoHideBar),
                new PropertyMetadata(true));

        #endregion

        protected virtual void RaiseEvent(EventHandler<EventArgs> handler)
        {
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}