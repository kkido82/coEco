using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;


namespace CoEco.BO.Filters
{
    public class TraceManagerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {

            var currentURL = filterContext.Request.RequestUri != null ? filterContext.Request.RequestUri.OriginalString.ToLower() : "";
            if (currentURL != null && HttpContext.Current.User.Identity.IsAuthenticated && !currentURL.Contains("metadata") && !currentURL.Contains("priveleges") && !currentURL.Contains("values"))
            {
                //Logger.InsertTraceLog(currentURL, "", HttpContext.Current.User.Identity.Name);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}