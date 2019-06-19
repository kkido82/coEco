using CoEco.BO.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using CoEco.Core.Services;
using Microsoft.AspNet.Identity;

namespace CoEco.BO.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected virtual IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("errors", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }



        protected IHttpActionResult OnError(Exception ex)
        {
            //Logger.InsertMofetLog(LogType.Error, $"{ex.Message}", System.Web.HttpContext.Current.User.Identity.Name);
            return InternalServerError(ex);
        }
    }
}