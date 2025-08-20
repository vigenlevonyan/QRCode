using Autofac;
using AutoMapper;
using Nc.Images;
using Nc.Images.Abstractions;
using QShirt.Application;
using QShirt.Application.Contracts;
using QShirt.Persistence;
using QShirt.Public.Application.Commands;
using System.Reflection;
using Module = Autofac.Module;

namespace QShirt.Public.Server.AutofacModules;

/// <summary>
/// Module for registering components from Application
/// </summary>
public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(AccessRightChecker)))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        builder
            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(AccessRightChecker)))
            .InstancePerLifetimeScope();

        // Application.Admin
        builder
            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(AddCustomerContentCommand)))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        builder
            .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(AddCustomerContentCommand)))
            .InstancePerLifetimeScope();

        // UserInfoProvider

        // UserInfoProvider
        builder
            .RegisterType<UserInfoProvider>()
            .AsSelf()
            .As<IUserInfoProvider>()
            .As<IUserInfoSetter>()
            .InstancePerLifetimeScope();


        // Image resizer
        builder
            .RegisterType<ImageResizer>()
            .As<IImageResizer>()
            .InstancePerLifetimeScope();


        // image caching
        string imageCacheDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "QShirt", "ImagesCache");
        builder.Register(delegate (IComponentContext c)
        {
            FileProvider fileProvider = new FileProvider(c.Resolve<IRepository<QShirt.Domain.File>>());
            return new LocalImageCacheManager(c.Resolve<IImageResizer>(),
                imageCacheDirectory, fileId =>
                    fileProvider.GetFileById(fileId).Result.Data);
        })
            .As<ILocalImageCacheManager>()
            .InstancePerLifetimeScope();

        // Automapper
        MapperConfiguration mapperConfiguration =
            new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddMaps(typeof(AccessRightChecker).Assembly,      //Application
                            typeof(AddCustomerContentCommand).Assembly,//Application.Backoffice
                            typeof(Program).Assembly                  //Backoffice
                    );
            });
        builder
            .Register(context => mapperConfiguration)
            .AsSelf()
            .As<AutoMapper.IConfigurationProvider>();

        builder.Register<IMapper>(ctx => new Mapper(ctx.Resolve<AutoMapper.IConfigurationProvider>(), ctx.Resolve))
            .InstancePerDependency();
    }
}
