using Autofac;
using CoEco.Core.Eventing.Events;
using CoEco.Core.Infrastructure;
using CoEco.Core.Ordering;

namespace CoEco.Core
{
    public class CoreIoc : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<Dispatcher>()
                .AsSelf()
                .SingleInstance();


            builder.RegisterModule<OrderingModule>();

            builder.RegisterAutofacEvents();

            base.Load(builder);
        }
    }
}
