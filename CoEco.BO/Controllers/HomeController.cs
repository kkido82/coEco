using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using CoEco.BO.Models;
using CoEco.BO.Auth;

namespace CoEco.BO.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IDataAccessService _dataAccessService;
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly IAuthenticationManager _authenticationManager;
        //public HomeController(IDataAccessService dataAccessService)
        //{
        //    _dataAccessService = dataAccessService;
        //}
        public HomeController(
                ApplicationUserManager applicationUserManager,
                IAuthenticationManager authenticationManager)
        {
            _applicationUserManager = applicationUserManager;
            _authenticationManager = authenticationManager;
        }
        public async Task<ActionResult> Index()
        {
            var model = new HomeModel
            {
                Title = "כלכלה שיתופית"
            };

            var username = GetUsername();
            if (!string.IsNullOrEmpty(username))
            {
                //var user = _applicationUserManager.Users.FirstOrDefault(u => u.UserName == username);
                var user = await _applicationUserManager.FindByNameAsync(username);
                if (user != null)
                {
                    model.User = new Models.HomeModel.UserModel
                    {
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailAddress = user.Email
                    };
                }


            }

            return View(model);
        }

        protected string GetUsername()
        {
            return HttpContext.User.Identity.Name;

        }

        //public ActionResult Index()
        //{
        //    ViewBag.MemberMI = _dataAccessService.GetAllMemers().FirstOrDefault().MemberMI;
        //    return View();
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}