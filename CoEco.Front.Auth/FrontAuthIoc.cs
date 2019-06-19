using Autofac;
using CoEco.Front.Auth.Data;
using CoEco.Front.Auth.Helpers;
using CoEco.Front.Auth.Services;

namespace CoEco.Front.Auth
{
    public class FrontAuthIoc : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterDbContext<AuthDbContext>();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<VerificationService>().As<IVerificationService>().InstancePerLifetimeScope();
            builder.RegisterType<SmsService>().As<ISmsService>().InstancePerLifetimeScope();
            builder.RegisterType<LoginService>().As<ILoginService>().InstancePerLifetimeScope();
            builder.RegisterType<LoginConnectionPoolService>().As<ILoginConnectionPoolService>().InstancePerLifetimeScope();
            builder.RegisterType<LoginConnectionPoolService>().SingleInstance();
            base.Load(builder);
        }
    }
}
