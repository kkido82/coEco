using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using CoEco.Core;
using CoEco.Core.Context;
using CoEco.Core.Infrastructure;
using CoEco.Data;
using CoEco.Front.Auth;
using CoEco.Front.Handlers.UnitItems;
using CoEco.Front.Handlers.ProblemReporting;
using CoEco.Front.Services;
using CoEco.Services;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CoEco.Front.App_Start
{
    public class IoCConfig
    {
        public static void Config()
        {
            var builder = new ContainerBuilder();


            builder.RegisterModule(new CoreIoc());
            builder.RegisterModule(new ServicesIoc());
            builder.RegisterModule(new DataIoc());
            builder.RegisterModule(new FrontAuthIoc());

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(r => HttpContext.Current).As<HttpContext>().InstancePerDependency();
            builder.RegisterType<AppUserContext>().As<IUserContext>().InstancePerLifetimeScope();
            builder.RegisterType<AppUserService>().As<Auth.Services.IAppUserService>().InstancePerLifetimeScope();

            builder.AddHandler<SendOrderProblemHandler, SendOrderProblemRequest, OrderProblemSent>();
            builder.AddHandler<ManageItemsHandler, ManageItemRequest, UnitItemUpdated>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

    }
}