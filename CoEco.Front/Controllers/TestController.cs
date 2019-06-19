using CoEco.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CoEco.Front.Controllers
{
	public class TestController : ApiController
	{
		private readonly CoEcoEntities db;

		public TestController(CoEcoEntities db)
		{
			this.db = db;
		}

		public IHttpActionResult Get()
		{
            return Ok("ok");
            //try
            //{
            //    var items = db.DistanceUnits.ToArray().Select(a => new { a.ID, a.FirstUnitID, a.SecondUnitID, a.Distance });
            //    return Ok(items);
            //}
            //catch (Exception ex)
            //{
            //    var errMessage = "";
            //    Exception cur = ex;
            //    while (cur != null)
            //    {
            //        errMessage+=ex.Message + "\n";
            //        cur = cur.InnerException;
            //    }
            //    return BadRequest(errMessage);
            //}
			
		}
	}
}