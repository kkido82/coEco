using System.Web.Http;
using CoEco.BO.Formatters;
using CoEco.BO.Filters;

namespace CoEco.BO.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.Formatters.Insert(0, new FileMediaFormatter());

            //config.Routes.MapHttpRoute(
            //    name: "test1",
            //    routeTemplate: "api1/test11",
            //    defaults: new { controller = "test", action = "get" }
            //);


            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new ElmahErrorAttribute());
            config.Filters.Add(new CoEcoExceptionHttpAttribute());
        }
    }
}