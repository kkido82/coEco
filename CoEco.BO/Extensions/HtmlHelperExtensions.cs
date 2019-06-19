using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CoEco.BO.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString ValidationResourcesJson(this HtmlHelper html)
        {
            var json = GetResourcesJson();
            return html.Raw(json);
            //var resoucres = Resources.Validation.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            //var rv = new Dictionary<string, string>();

            //var enumartor = resoucres.GetEnumerator();
            //while (enumartor.MoveNext())
            //{
            //    rv.Add(enumartor.Key.ToString(), HttpUtility.HtmlEncode(enumartor.Value.ToString()));
            //}

            //return html.Raw(JsonConvert.SerializeObject(rv, Formatting.None,
            //    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        public static IHtmlString ValidationResourcesJson<T>(this HtmlHelper<T> html)
        {
            var json = GetResourcesJson();

            return html.Raw(json);
        }

        private static string GetResourcesJson()
        {
            var resoucres = Resources.Validation.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            var rv = new Dictionary<string, string>();

            var enumartor = resoucres.GetEnumerator();
            while (enumartor.MoveNext())
            {
                rv.Add(enumartor.Key.ToString(), HttpUtility.HtmlEncode(enumartor.Value.ToString()));
            }
            return JsonConvert.SerializeObject(rv, Formatting.None,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        public static string RenderPartialViewToString(this Controller controller, string viewName, object model)
        {
            //var view = ViewEngines.Engines.FindPartialView(controllerContext, partialView.ViewName).View;
            try
            {
                if (string.IsNullOrEmpty(viewName))
                    viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

                controller.ViewData.Model = model;

                using (var sw = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName).View;
                    var viewContext = new ViewContext(controller.ControllerContext, viewResult, controller.ViewData, controller.TempData, sw);
                    viewResult.Render(viewContext, sw);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }





        public static string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", "Coeco");
                var fakeControllerContext = new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new FakeController());
                var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName, "", false);

                var viewContext = new ViewContext(fakeControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }
        public class FakeController : ControllerBase { protected override void ExecuteCore() { } }
    }
}