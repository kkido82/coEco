using Autofac;
using CoEco.Core.Services;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Eventing.Events
{
    public static class Extensions
    {
        public static void RegisterAutofacEvents(this ContainerBuilder builder)
        {
            var typeFinder = new AppDomainTypeFinder();
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            var asyncconsumers = typeFinder.FindClassesOfType(typeof(IAsyncConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
            foreach (var consumer in asyncconsumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IAsyncConsumer<>)))
                    .InstancePerLifetimeScope();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().InstancePerLifetimeScope();
            builder.RegisterType<EventsSubscriptionService>().As<ISubscriptionService>().InstancePerLifetimeScope();
        }
    }
}
