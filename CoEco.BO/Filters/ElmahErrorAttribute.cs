using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CoEco.BO.Filters
{
    public class ElmahErrorAttribute: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
                Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);

            base.OnException(actionExecutedContext);
        }
    }
}