using Autofac;
using Shots.AppEngine;
using Shots.AppEngine.Bootstrppers;
using Shots.AppEngine.Modules;

namespace Shots.Factories
{
    internal static class AppKernelFactory
    {
        public static Module[] GetModules() =>
            new Module[]
            {
                new UtilityModule(),
                new ServiceModule(),
                new ViewModelModule()
            };

        public static IBootStrapper[] GetBootStrappers() =>
            new IBootStrapper[]
            {
                // None, atm
            };

        public static AppKernel Create() => new AppKernel(GetBootStrappers(), GetModules());
    }
}