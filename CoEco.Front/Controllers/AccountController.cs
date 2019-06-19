using CoEco.Core.Eventing;
using CoEco.Core.Ordering.Domain;
using CoEco.Front.Auth.Domain;
using CoEco.Front.Auth.Services;
using CoEco.Front.Helpers;
using CoEco.Front.Models.Account;
using CoEco.Services.Services;
using JwtAuth;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace CoEco.Front.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        int expireInMinutes = int.Parse(ConfigurationManager.AppSettings["auth:ExpireInMinutes"]);
        private readonly ILoginConnectionPoolService loginConnectionPoolService;
        private readonly IAuthenticationService authenticationService;
        private readonly IAppUserService userService;
        private readonly ILogger logger;

        public AccountController(
            ILoginConnectionPoolService loginConnectionPoolService,
            IAuthenticationService authenticationService,
            IAppUserService userService,
            ILogger logger,
            IDataService<Member> dataService)
        {
            this.loginConnectionPoolService = loginConnectionPoolService;
            this.authenticationService = authenticationService;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet, Route("test")]
        public IHttpActionResult Test()
        {
            return Ok();
        }

        [Route("login"), HttpPost]
        public async Task<IHttpActionResult> Login(LoginRequestModel model)
        {
            var authRes = await authenticationService.Authenticate(model.TZ, model.Code);

            if (authRes.Success)
            {
                var claims = authRes.Value.GetClaims();

                return JwtAuthResults.SignIn(claims);
            }

            return BadRequest(authRes.Error.GetErrorMessage());
        }


        [Route("logoff"), HttpPost]
        public IHttpActionResult LogOff()
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;

            if (HttpContext.Current.Request.Cookies["XSRF-TOKEN"] != null)
            {
                var c = new HttpCookie("XSRF-TOKEN");
                c.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(c);
            }
            return JwtAuthResults.Signout();
        }

        [Route("user"), HttpGet, Authorize]
        public async Task<IHttpActionResult> GetUser()
        {
            var user = await userService.GetUser(User.Identity.Name);
            var model = Map(user);
            model.RequestToken = SetAntiForgeryToken();
            return Ok(model);
        }

        public string SetAntiForgeryToken()
        {
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            var cookie = new HttpCookie("XSRF-TOKEN", cookieToken);
            cookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Add(cookie);
            return formToken;
        }

        [Route("sendtz"), HttpPost]
        public async Task<IHttpActionResult> SendTz(SendTzModel model)
        {
            var tz = model.TZ;
            var userExists = await userService.UserExists(tz);
            if (!userExists.Success)
                return BadRequest(userExists.Error.GetErrorMessage());

            var res = await authenticationService.CreateCode(tz);
            if (!res.Success)
            {
                return BadRequest(res.Error.GetErrorMessage());
            }
            return Ok();
        }

        private UserModel Map(User user)
        {
            var model = new UserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                TZ = user.Username,
                Permissions = user.Permissions,
                UnitId = user.UnitId
            };

            return model;

        }
    }
}