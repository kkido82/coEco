using CoEco.Core;
using System.Web;
using System.Web.Mvc;


namespace CoEco.BO.Filters
{
    public class CoEcoExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception == null)
                return;

            var originController = filterContext.RouteData.Values["controller"].ToString();
            var originAction = filterContext.RouteData.Values["action"].ToString();
            var exp = filterContext.Exception.Message;
            var innerExp = string.Empty;

            if (filterContext.Exception.InnerException != null)
            {
                innerExp = filterContext.Exception.InnerException.Message;
            }

        }
    }

    public class CoEcoExceptionHttpAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        public override void OnException(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                var originController = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
                var originAction = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                var exp = actionExecutedContext.Exception.Message;
                var innerExp = string.Empty;

                if (actionExecutedContext.Exception.InnerException != null)
                {
                    innerExp = actionExecutedContext.Exception.InnerException.Message;
                }

            }

            base.OnException(actionExecutedContext);
        }
    }

}