using Shots.AppEngine;
using Shots.Factories;

namespace Shots.ViewModels
{
    internal class ViewModelLocator
    {
        private readonly AppKernel _kernel;

        public ViewModelLocator()
        {
            _kernel = App.Current?.Kernel ?? AppKernelFactory.Create();
        }

        public MainViewModel Main => _kernel.Resolve<MainViewModel>();
    }
}