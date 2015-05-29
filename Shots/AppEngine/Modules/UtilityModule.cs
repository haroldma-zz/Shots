using Autofac;
using Shots.Core.Interfaces.Utilities;
using Shots.Core.Utilities;

namespace Shots.AppEngine.Modules
{
    class UtilityModule : AppModule
    {
        public override void LoadDesignTime(ContainerBuilder builder)
        {
        }

        public override void LoadRunTime(ContainerBuilder builder)
        {
            builder.RegisterType<DispatcherUtility>().As<IDispatcherUtility>();
            builder.RegisterType<CredentialUtility>().As<ICredentialUtility>();
            builder.RegisterType<SettingsUtility>().As<ISettingsUtility>();
            builder.RegisterType<StorageUtility>().As<IStorageUtility>();
        }
    }
}