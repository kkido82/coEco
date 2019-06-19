using System.Collections.Generic;
using System.Net.Http;

namespace JwtAuth
{
	internal interface ISignIn
	{
		HttpResponseMessage SignIn(IDictionary<string, string> claims);
        HttpResponseMessage Signout();
	}
}
