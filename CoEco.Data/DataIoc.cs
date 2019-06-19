using Autofac;
using CoEco.Data.Services;

namespace CoEco.Data
{
    public class DataIoc : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CoEcoEntities>().InstancePerLifetimeScope();
            builder.RegisterType<AppQueryService>().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
