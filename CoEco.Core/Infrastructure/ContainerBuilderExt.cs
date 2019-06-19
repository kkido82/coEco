using Autofac;
using CoEco.Core.Interfaces;

namespace CoEco.Core.Infrastructure
{
    public static class ContainerBuilderExt
    {
        public static ContainerBuilder AddHandler<THandler, TRequest, TResponse>(this ContainerBuilder builder)
            where THandler : IHandler<TRequest, TResponse>
            where TRequest : class, IRequest<TResponse>
            where TResponse : class
        {
            builder.RegisterType<THandler>().As<IHandler<TRequest, TResponse>>().InstancePerLifetimeScope();

            builder.RegisterBuildCallback(c =>
            {
                var dispatcher = c.Resolve<Dispatcher>();
                dispatcher.AddScopedHandler(() =>
                {
                    var scope = c.BeginLifetimeScope();
                    return new AutofacScopeFactory<TRequest, TResponse>(scope);
                });
            });
            return builder;
        }
    }
}
