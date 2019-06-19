using CoEco.Front.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CoEco.Front.Controllers
{
	public abstract class BaseApiController : ApiController
	{
		public int MemberId => User.GetMemberId();
	}
}