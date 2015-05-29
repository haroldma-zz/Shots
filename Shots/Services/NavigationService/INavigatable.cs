﻿using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Shots.Services.NavigationService
{
    public interface INavigatable
    {
        void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state);
        void OnNavigatedFrom(bool suspending, Dictionary<string, object> state);
    }
}
