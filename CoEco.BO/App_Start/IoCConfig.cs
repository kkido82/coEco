using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Breeze.ContextProvider.EF6;
using Autofac.Integration.WebApi;
using System.Reflection;
using CoEco.Data;
using Microsoft.AspNet.Identity.Owin;
using CoEco.BO.Auth;
using System.Web.Mvc;
using System.Web.Http;
using CoEco.Core.Eventing.Events;
using CoEco.BO.Provider;
using Microsoft.Owin.Security;
using CoEco.BO.Services;
using CoEco.Core.Context;
using CoEco.Core.Services;
using CoEco.Core.Eventing;
using CoEco.BO.ExcelUploadFiles;
using CoEco.Core;
using CoEco.Services;

namespace CoEco.BO.App_Start
{
    public class IoCConfig
    {
        private static IContainer _current;
        public static IContainer Current
        {
            get { return _current; }
        }
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<CoreIoc>();
            builder.RegisterModule<ServicesIoc>();
            builder.RegisterModule<DataIoc>();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(r => HttpContext.Current).As<HttpContext>().InstancePerDependency();
            builder.RegisterType<CoEcoEFContextProvider>()
                .As<EFContextProvider<CoEcoEntities>>().PropertiesAutowired().InstancePerLifetimeScope();
            
            builder.RegisterType<AuthRepository>().As<IAuthRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ExportXslService>().As<IExportXslService>().InstancePerLifetimeScope();
            builder.RegisterType<FileHandle>().As<IFileHandle>().InstancePerLifetimeScope();
            builder.RegisterType<FileValidation>().As<IFileValidation>().InstancePerLifetimeScope();
            builder.RegisterType<DistanceService>().InstancePerLifetimeScope();
            builder.RegisterType<BoUserContext>().As<IUserContext>().InstancePerLifetimeScope();
            builder.Register(c => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationDbContext>())
                .As<ApplicationDbContext>()
                .InstancePerLifetimeScope();
            builder.Register(c => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>())
                 .As<ApplicationUserManager>()
                 .InstancePerLifetimeScope();
            builder.Register(c => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationSignInManager>())
               .As<ApplicationSignInManager>()
               .InstancePerLifetimeScope();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication)
                .As<IAuthenticationManager>()
                .InstancePerLifetimeScope();


            builder.RegisterFilterProvider();

            var container = builder.Build();
            _current = container;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            // Set up the FluentValidation provider factory and add it as a Model validator
            //var fluentValidationModelValidatorProvider = new FluentValidationModelValidatorProvider(new AutofacValidatorFactory(container));
            //DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            //fluentValidationModelValidatorProvider.AddImplicitRequiredValidator = false;
            //ModelValidatorProviders.Providers.Add(fluentValidationModelValidatorProvider);

            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

        }
    }
}