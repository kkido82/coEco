using Autofac;
using Autofac.Core;
using CoEco.Core.Eventing;
using CoEco.Core.Ordering.Repositories;
using CoEco.Core.Services;
using CoEco.Services.Services;
using CoEco.Services.Services.SmsProviders;
using System;
using System.Configuration;
using System.Net.Configuration;

namespace CoEco.Services
{
    public class ServicesIoc : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<DataAccessService>().As<IDataAccessService>().InstancePerLifetimeScope();
            builder.RegisterType<DownloadFileService>().As<IDownloadFileService>().InstancePerLifetimeScope();
            builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();
            builder.RegisterType<LoggerService>().As<ILoggerService>().InstancePerLifetimeScope();
            builder.RegisterType<FakeLogger>().As<ILogger>().InstancePerLifetimeScope();
            builder.RegisterType<OrderingService>().As<IOrderingService>().InstancePerLifetimeScope();
            builder.RegisterType<MessagesService>().As<IMessagesService>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(DataService<>))
              .As(typeof(IDataService<>))
              .WithParameter(new ResolvedParameter(
                  (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "userId",
                  (pi, ctx) => System.Web.HttpContext.Current.User.Identity.Name)).InstancePerLifetimeScope();

            RegisterSmsProvider(builder);

            Register(builder);

            base.Load(builder);
        }

        private void Register(ContainerBuilder builder)
        {
            var smtp = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            var network = smtp.Network;
            var options = new SmtpOptions(network.Host, network.Port, network.UserName, network.Password, network.EnableSsl);
            builder.Register(_ => options).SingleInstance();
            builder.RegisterType<SmtpMailService>().As<IMailService>().InstancePerLifetimeScope();

        }

        private static void RegisterSmsProvider(ContainerBuilder builder)
        {
            switch (ConfigurationManager.AppSettings["SMS-Provider"])
            {
                case "inforu":
                    builder.RegisterType<InforuSender>().As<ISmsSender>().InstancePerLifetimeScope();
                    break;
                case "cellcom":
                    builder.RegisterType<CellcomSender>().As<ISmsSender>().InstancePerLifetimeScope();
                    break;
                default:
                    builder.RegisterType<MockSender>().As<ISmsSender>().InstancePerLifetimeScope();
                    break;

            }
        }
    }
}
