using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using Shots.Core.Common;
using Shots.Services.NavigationService;

namespace Shots.Mvvm
{
    public abstract class ViewModelBase : ObservableObject, INavigatable
    {
        public bool IsInDesignMode => DesignMode.DesignModeEnabled;

        public virtual void OnNavigatedTo(object parameter, NavigationMode mode, Dictionary<string, object> state)
        {   
        }

        public virtual void OnNavigatedFrom(bool suspending, Dictionary<string, object> state)
        {
        }
    }
}