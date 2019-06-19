using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace JwtAuth
{
	internal class JwtAuthFilter : ActionFilterAttribute, IAuthenticationFilter
	{
		private readonly TokenSerializer tokenSerializer;
		private readonly JwtCookie cookie;

		internal JwtAuthFilter(TokenSerializer tokenSerializer, JwtCookie cookie)
		{
			this.tokenSerializer = tokenSerializer;
			this.cookie = cookie;
		}


		public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
			var res = Task.CompletedTask;

			var token = cookie.GetToken(context.Request);
			if (string.IsNullOrEmpty(token))
				return res;

			var payload = tokenSerializer.Deserilize(token);

			var claims = payload.Keys
				.Select(key => new Claim(key, payload[key]))
				.ToList();

			var identity = new ClaimsIdentity(claims, "jwt");
			context.Principal = new ClaimsPrincipal(new[] { identity });

			return res;
		}

		public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
		{
			base.OnActionExecuted(actionExecutedContext);
		}

		public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
