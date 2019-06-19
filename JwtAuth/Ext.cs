using System.Web.Http;

namespace JwtAuth
{

	public static class Ext
	{
		public static void AddJwtAuth(this HttpConfiguration config, JwtOptions options)
		{
			var serializer = new TokenSerializer(options.Secret, options.Duration);
			var cookie = new JwtCookie("auth-jwt");
			var signIn = new JwtSignIn(serializer, cookie);
			JwtAuthResults.ISignIn = signIn;
			config.Filters.Add(new JwtAuthFilter(serializer, cookie));

		}
	}
}
