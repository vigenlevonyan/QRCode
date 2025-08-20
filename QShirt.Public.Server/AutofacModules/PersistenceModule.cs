using Autofac;
using Module = Autofac.Module;
using QShirt.Persistence;

namespace QShirt.Public.Server.AutofacModules;

/// <summary>
/// Module for registering components from Persistence
/// </summary>
public class PersistenceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterGeneric(typeof(Repository<>))
            .As(typeof(IRepository<>))
            .InstancePerLifetimeScope();

        builder
            .RegisterType<ChangesSaver<QShirtContext>>()
            .As<IChangesSaver>()
            .InstancePerLifetimeScope();
    }
}
