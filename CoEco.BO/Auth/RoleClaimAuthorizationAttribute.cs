using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using CoEco.BO.Models;
using CoEco.BO.Config;

namespace CoEco.BO.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RoleClaimAuthorizationAttribute : AuthorizeAttribute
    {
        private readonly string[] _roles;
        private readonly Dictionary<string, AccessRight> _dic = new Dictionary<string, AccessRight>(StringComparer.InvariantCultureIgnoreCase);
        public string ClaimType { get; set; }

        public RoleClaimAuthorizationAttribute(params string[] roles)
        {
            this.ClaimType = AppAuthConfiguration.Config.RolesClaimName;

            _roles = roles;
            var splitted =
                roles.Select(
                    x => x.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(y => y.Trim().ToLower()).ToArray());

            foreach (var r in splitted)
            {
                if (r.Length == 0 || _dic.ContainsKey(r[0]))
                    continue;

                if (r.Length == 1)
                {
                    this._dic.Add(r[0], AccessRight.ReadOnly);
                    continue;
                }


                AccessRight ar = AccessRight.None;
                if (Enum.TryParse<AccessRight>(r[1], true, out ar))
                {
                    this._dic.Add(r[0], ar);
                }


            }
        }

        public bool TestAuthorize(HttpActionContext actionContext)
        {
            return IsAuthorized(actionContext);
        }


        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var principal = actionContext.ControllerContext.RequestContext.Principal;

            var identity = principal?.Identity as ClaimsIdentity;
            if (identity == null)
                return false;

            var roles = identity.GetUserRoles();
            foreach (var rolesKey in roles.Keys)
            {
                if (_dic.ContainsKey(rolesKey) && roles[rolesKey] >= _dic[rolesKey])
                    return true;
            }



            return false;

        }
    }
}