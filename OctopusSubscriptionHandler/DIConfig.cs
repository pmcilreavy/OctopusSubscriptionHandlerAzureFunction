using System.Collections.Generic;
using System.Reflection;
using Autofac;
using AzureFunctions.Autofac.Configuration;
using MediatR;
using OctopusSubscriptionHandler.Functions;

namespace OctopusSubscriptionHandler
{
    public class DIConfig
    {
        public DIConfig()
        {
            DependencyInjection.Initialize(builder =>
            {
                builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

                builder
                    .Register<SingleInstanceFactory>(ctx =>
                    {
                        var c = ctx.Resolve<IComponentContext>();
                        return t => c.TryResolve(t, out var o) ? o : null;
                    })
                    .InstancePerLifetimeScope();

                builder
                    .Register<MultiInstanceFactory>(ctx =>
                    {
                        var c = ctx.Resolve<IComponentContext>();
                        return t => (IEnumerable<object>) c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
                    })
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(typeof(HandleOctopusSubscriptionEventFunction).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan

                //builder.RegisterType<SlackHook>().As<IHook>();

                ////Implicity registration
                //builder.RegisterType<Sample>().As<ISample>();
                ////Explicit registration
                //builder.Register<Example>(c => new Example(c.Resolve<ISample>())).As<IExample>();
                ////Registration by autofac module
                //builder.RegisterModule(new TestModule());
                ////Named Instances are supported
                //builder.RegisterType<Thing1>().Named<IThing>("OptionA");
                //builder.RegisterType<Thing2>().Named<IThing>("OptionB");
            });
        }
    }
}
