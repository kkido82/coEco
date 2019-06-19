using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoEco.Front.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return File("~/app/index.html", "text/html");
        }
    }
}
