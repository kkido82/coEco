using System.Collections.Generic;
using System.Net.Http;

namespace JwtAuth
{
	internal class JwtSignIn : ISignIn
	{
		private readonly TokenSerializer serializer;
		private readonly JwtCookie cookie;

		public JwtSignIn(TokenSerializer serializer, JwtCookie cookie)
		{
			this.serializer = serializer;
			this.cookie = cookie;
		}


		public HttpResponseMessage SignIn(IDictionary<string, string> claims)
		{
			var response = new HttpResponseMessage
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Content = new StringContent("ok")
			};

			var token = serializer.Serialize(claims);
			cookie.SetToken(response, token);
			return response;
		}

        public HttpResponseMessage Signout()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("ok")
            };

            cookie.Remove(response);

            return response;
        }
    }
}
