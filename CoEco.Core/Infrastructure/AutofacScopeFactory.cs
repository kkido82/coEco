using Autofac;
using CoEco.Core.Interfaces;

namespace CoEco.Core.Infrastructure
{
    public class AutofacScopeFactory<TRequest, TResponse> : IHandlerScope<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
        where TResponse : class
    {
        private readonly ILifetimeScope scope;

        public AutofacScopeFactory(ILifetimeScope scope)
        {
            this.scope = scope;
        }
        public IHandler<TRequest, TResponse> Create()
        {
            return scope.Resolve<IHandler<TRequest, TResponse>>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }

}
