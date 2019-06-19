using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace JwtAuth
{
	public class JwtAuthResults
	{
		internal static ISignIn ISignIn = new EmptySignIn();

		public static IHttpActionResult SignIn(IDictionary<string, string> claims)
		{
			var response = ISignIn.SignIn(claims);
			return new ConstResult(response);
		}

        public static IHttpActionResult Signout()
        {
            var response = ISignIn.Signout();
            return new ConstResult(response);
        }

		class ConstResult : IHttpActionResult
		{
			private readonly HttpResponseMessage response;

			public ConstResult(HttpResponseMessage response)
			{
				this.response = response;
			}
			public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
			{
				return Task.FromResult(response);
			}
		}
	}
}
