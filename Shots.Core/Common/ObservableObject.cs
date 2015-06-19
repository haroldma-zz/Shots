using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Shots.Core.Common
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(string propertyName,
            ref T field,
            T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;

            field = newValue;
            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChanged(propertyName);

            return true;
        }

        protected bool Set<T>(
            ref T field,
            T newValue,
            [CallerMemberName] string propertyName = null)
        {
            return Set(propertyName, ref field, newValue);
        }
    }
}