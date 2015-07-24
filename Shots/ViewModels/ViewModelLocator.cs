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

        public WelcomeViewModel Welcome => _kernel.Resolve<WelcomeViewModel>();
        public MainViewModel Main => _kernel.Resolve<MainViewModel>();
        public ProfileViewModel Profile => _kernel.Resolve<ProfileViewModel>();
        public ShotViewModel Shot => _kernel.Resolve<ShotViewModel>();
        public SearchViewModel Search => _kernel.Resolve<SearchViewModel>();
    }
}