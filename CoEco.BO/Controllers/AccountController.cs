using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using CoEco.BO.Config;
using CoEco.BO.Models;
using System.Configuration;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Jose;
using CoEco.BO.Services;
using System.Web.Http.Results;

namespace CoEco.BO.Controllers
{
    [RoutePrefix("api/Account")]
    [Authorize]
    public class AccountController : BaseApiController
    {
        private readonly IAuthRepository _repo;
        // GET: Account
        public AccountController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repo.Login(model.Username, model.Password, true, model.RememberMe);
            if (result.Succeeded)
            {
                var user = await _repo.FindUser(model.Username, model.Password);
                return Ok(new
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.Email
                });
            }


            return GetErrorResult(result);


        }


        [HttpPost]
        [Route("SignOut")]
        public async Task<IHttpActionResult> Logout()
        {
            var result = await _repo.SignOut();

            if (result.Succeeded)
                return Ok(result);

            return GetErrorResult(result);
        }


        [Route("Priveleges")]
        [HttpGet]
        public IHttpActionResult GetPrivileges()
        {

            var identity = User.Identity as ClaimsIdentity;
            if (identity != null && identity.Claims.Any(x => x.Type == AppAuthConfiguration.Config.RolesClaimName))
                return
                    Ok(identity.GetUserRoles());

            return Ok();
        }

        //////////////////////////////////////////////////////////////////////CRM/////////////////////////////////////////////
        private static readonly string rsaPrivateXmlString = ConfigurationManager.AppSettings["RSAPrivateKey"];

        public class TokenValues
        {

            public string userName;
            public string malshabId;
            public string pageType;
            public string pageId;
            public string exp;
        };

        private string GetTokenValues(string token)
        {

            var rsaPrivate = new RSACryptoServiceProvider();
            rsaPrivate.FromXmlString(rsaPrivateXmlString);
            string values = JWT.Decode(token, rsaPrivate, JwsAlgorithm.RS512);



            return values;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> SSOLogIn(string token)
        {

            string jsonValues = GetTokenValues(token);
            TokenValues tokenValues = JsonConvert.DeserializeObject<TokenValues>(jsonValues);
            var baseUrl = ConfigurationManager.AppSettings["BASE_URL"];
            TimeSpan ts = DateTime.FromBinary(long.Parse(tokenValues.exp)).AddSeconds(60) - DateTime.Now.ToUniversalTime();
            if (ts.TotalSeconds < 0)
            {
                return Redirect(baseUrl);
            }

            var user = await _repo.SSOCRMLogIn(tokenValues.userName);


            if (tokenValues.pageId == "salaries")
            {
                return Redirect(baseUrl + "/#/meshartim/salaries/" + tokenValues.malshabId);
            }
            else
            {
                return Redirect(baseUrl);
            }
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpGet]
        public async Task<System.Web.Mvc.JsonResult> Register()
        {
            var username = "777777777";
            var password = "Aa_1234";


            var res = await _repo.CreateUser(new Models.UpdateUserModel
            {
                FirstName = "מושית",
                LastName = "דודסון",
                Email = "moshit.davidson@ngsoft.com",
                Password = password,
                UserName = username,
                PhoneNumber = "123456789"
            });
            return new System.Web.Mvc.JsonResult()
            {
                Data = res,
                JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
            };
            //return Json(res, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Route("register1")]
        [HttpGet]
        public async Task<System.Web.Mvc.JsonResult> Register2()
        {
            var username = "888888888";
            var password = "Aa_12345";


            var res = await _repo.CreateUser(new Models.UpdateUserModel
            {
                FirstName = "בדיקה",
                LastName = "בדיקה",
                Email = "daniel.hasson@ngsoft.com",
                Password = password,
                UserName = username,
                PhoneNumber = "123456789"
            });
            return new System.Web.Mvc.JsonResult()
            {
                Data = res,
                JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
            };
            //return Json(res, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }
    }
}