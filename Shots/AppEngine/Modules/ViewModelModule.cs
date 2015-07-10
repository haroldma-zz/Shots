using Autofac;
using Shots.ViewModels;

namespace Shots.AppEngine.Modules
{
    internal class 
        ViewModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WelcomeViewModel>();
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<ProfileViewModel>();
            builder.RegisterType<ShotViewModel>();
        }
    }
}