using System.Collections.Generic;
using System.Reflection;
using Autofac;
using AzureFunctions.Autofac.Configuration;
using MediatR;

namespace OctopusSubscriptionHandler.Core.Utility
{
    public class IoC
    {
        public IoC()
        {
            DependencyInjection.Initialize(
                builder =>
                {
                    RegisterMediatR(builder);

                    builder.RegisterAssemblyTypes(typeof(IoC).GetTypeInfo().Assembly)
                        .AsImplementedInterfaces();
                });
        }

        private static void RegisterMediatR(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

            builder
                .Register<SingleInstanceFactory>(
                    ctx =>
                    {
                        var c = ctx.Resolve<IComponentContext>();

                        return t => c.TryResolve(t, out var o) ? o : null;
                    })
                .InstancePerLifetimeScope();

            builder
                .Register<MultiInstanceFactory>(
                    ctx =>
                    {
                        var c = ctx.Resolve<IComponentContext>();

                        return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
                    })
                .InstancePerLifetimeScope();
        }
    }
}