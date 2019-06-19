using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Breeze.WebApi2;

namespace CoEco.BO.Formatters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class PrepareFileExportAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //When time comes and we expect to export to excel, then we should not let breeze query filter page, or wrap the returned data
            if (actionContext.Request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue("text/html")))
            {
                var query = actionContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                if (query.ContainsKey("$inlinecount"))
                    query.Remove("$inlinecount");
                if (query.ContainsKey("$skip"))
                    query.Remove("$skip");
                if (query.ContainsKey("$top"))
                    query.Remove("$top");

                actionContext.Request.Properties["MS_QueryNameValuePairs"] = query.ToList();
            }


            base.OnActionExecuting(actionContext);
        }

    }
}