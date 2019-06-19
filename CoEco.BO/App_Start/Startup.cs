
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartupAttribute(typeof(CoEco.BO.App_Start.Startup))]
namespace CoEco.BO.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IoCConfig.RegisterDependencies();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MapperConfig.RegisterMaps();
            DescriptorConfig.AddTypeDescriptors();
            ConfigureAuth(app);
            //IoCConfig.RegisterDependencies();
            //AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            ////RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            //MapperConfig.RegisterMaps();
            //DescriptorConfig.AddTypeDescriptors();
            //ConfigureAuth(app);
        }
    }
}