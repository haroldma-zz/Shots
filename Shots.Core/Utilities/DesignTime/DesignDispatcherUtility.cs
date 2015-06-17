using System;
using System.Threading.Tasks;
using Shots.Core.Utilities.Interfaces;

namespace Shots.Core.Utilities.DesignTime
{
    public class DesignDispatcherUtility : IDispatcherUtility
    {
        public Task RunAsync(Action action)
        {
            return Task.FromResult(0);
        }

        public Task<T> RunAsync<T>(Func<T> func)
        {
            return Task.FromResult(default(T));
        }

        public Task<T> RunAsync<T>(Func<Task<T>> func)
        {
            return Task.FromResult(default(T));
        }
    }
}