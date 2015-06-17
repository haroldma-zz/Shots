using System;
using System.Threading.Tasks;

namespace Shots.Core.Utilities.Interfaces
{
    public interface IDispatcherUtility
    {
        Task RunAsync(Action action);
        Task<T> RunAsync<T>(Func<T> func);
        Task<T> RunAsync<T>(Func<Task<T>> func);
    }
}