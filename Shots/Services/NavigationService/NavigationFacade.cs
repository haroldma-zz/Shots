﻿using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Shots.Services.NavigationService
{
    public class NavigationFacade
    {
        public NavigationFacade(Frame frame)
        {
            _frame = frame;

            // setup animations
            var c = new TransitionCollection { };
            var t = new NavigationThemeTransition { };
            //var i = new EntranceNavigationTransitionInfo();
            //t.DefaultNavigationTransitionInfo = i;
            c.Add(t);
            _frame.ContentTransitions = c;
        }

        #region frame facade

        readonly Frame _frame;

        public bool Navigate(Type page, object parameter) => _frame.Navigate(page, parameter);

        public void SetNavigationState(string state) => _frame.SetNavigationState(state);

        public string GetNavigationState() => _frame.GetNavigationState();

        public int BackStackDepth => _frame.BackStackDepth;

        public bool CanGoBack => _frame.CanGoBack;

        public void GoBack() { _frame.GoBack(); }

        public void Refresh()
        {
            var page = this.CurrentPageType;
            var param = this.CurrentPageParam;
            this._frame.BackStack.Remove(this._frame.BackStack.Last());
            this.Navigate(page, param);
        }

        public bool CanGoForward => _frame.CanGoForward;

        public void GoForward() { _frame.GoForward(); }

        public object Content => _frame.Content;

        public Type CurrentPageType { get; internal set; }

        public object CurrentPageParam { get; internal set; }

        public object GetValue(DependencyProperty dp) => _frame.GetValue(dp);

        public void SetValue(DependencyProperty dp, object value) => _frame.SetValue(dp, value);

        public void ClearValue(DependencyProperty dp) => _frame.ClearValue(dp);

        #endregion
        

        readonly List<EventHandler> _navigatingEventHandlers = new List<EventHandler>();
        public event EventHandler Navigating
        {
            add
            {
                if (!_navigatingEventHandlers.Contains(value))
                {
                    _navigatingEventHandlers.Add(value);
                    if (_navigatingEventHandlers.Count == 1)
                        _frame.Navigating += FacadeNavigatingCancelEventHandler;
                }
            }
            remove
            {
                if (_navigatingEventHandlers.Contains(value))
                {
                    _navigatingEventHandlers.Remove(value);
                    if (_navigatingEventHandlers.Count == 0)
                        _frame.Navigating -= FacadeNavigatingCancelEventHandler;
                }
            }
        }

        private void FacadeNavigatingCancelEventHandler(object sender, NavigatingCancelEventArgs e)
        {
            foreach (var handler in _navigatingEventHandlers)
            {
                handler(this, new EventArgs());
            }
        }
    }

}
