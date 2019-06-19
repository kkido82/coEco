using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoEco.Core.Eventing.Events
{
    class EventsSubscriptionService : ISubscriptionService
    {
        private readonly ILifetimeScope _lifetimeScope;

        public EventsSubscriptionService(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public IList<IConsumer<T>> GetSubscriptions<T>() where T : class
        {
            return _lifetimeScope.Resolve<IEnumerable<IConsumer<T>>>().ToList();
        }

        public IList<IAsyncConsumer<T>> GetAyncSubscriptions<T>() where T : class
        {
            return _lifetimeScope.Resolve<IEnumerable<IAsyncConsumer<T>>>().ToList();
        }
    }
}
