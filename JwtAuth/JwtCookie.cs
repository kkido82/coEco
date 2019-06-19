using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace JwtAuth
{
	internal class JwtCookie
	{
		private readonly string cookieName;

		public JwtCookie(string cookieName)
		{
			this.cookieName = cookieName;
		}

		public string GetToken(HttpRequestMessage request)
		{
			return request.Headers
				.GetCookies(cookieName)
				.Select(c => c.Cookies.Select(a => a.Value).FirstOrDefault())
				.FirstOrDefault();
		}

		public void SetToken(HttpResponseMessage response, string value)
		{
			var cookie = new CookieHeaderValue(cookieName, value)
			{
				HttpOnly = true,
				Expires = DateTimeOffset.Now.AddYears(1),
                Path = "/"
			};

			response.Headers.AddCookies(new[] { cookie });
		}

        public void Remove(HttpResponseMessage response)
        {
            var cookie = new CookieHeaderValue(cookieName, "")
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddYears(-1)
            };

            response.Headers.AddCookies(new[] { cookie });

        }
    }
}
