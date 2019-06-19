using CoEco.Front.Auth.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace CoEco.Front.Helpers
{
	public static class UserClaimsExt
	{
		const string memberIdType = ClaimTypes.PrimarySid;
		public static IDictionary<string, string> GetClaims(this User user)
		{
			var claims = new Dictionary<string, string>
			{
				{ ClaimTypes.Name, user.Username },
				{ ClaimTypes.NameIdentifier, user.Username },
				{ memberIdType, user.Id.ToString() },
				{ "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", user.Username }
			};
			return claims;
		}

		public static int GetMemberId(this IPrincipal principal)
		{
			var identity = principal.Identity as ClaimsIdentity;
			var value = identity.Claims
				.Where(a => a.Type == memberIdType)
				.Select(a=>a.Value)
				.FirstOrDefault();
			return int.Parse(value);
		}
	}
}