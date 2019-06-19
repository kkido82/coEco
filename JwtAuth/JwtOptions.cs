using System;

namespace JwtAuth
{
	public class JwtOptions
	{
		public JwtOptions(string secret, TimeSpan exp)
		{
			Secret = secret;
			Duration = exp;
		}

		public string Secret { get; }
		public TimeSpan Duration { get; }
	}
}
