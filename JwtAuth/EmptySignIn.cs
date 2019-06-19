using System;
using System.Collections.Generic;
using System.Net.Http;

namespace JwtAuth
{
	internal class EmptySignIn : ISignIn
	{
		public HttpResponseMessage SignIn(IDictionary<string, string> claims)
		{
			throw new NotImplementedException();
		}

        public HttpResponseMessage Signout()
        {
            throw new NotImplementedException();
        }
    }
}
