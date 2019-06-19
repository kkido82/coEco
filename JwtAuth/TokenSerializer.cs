using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JwtAuth
{
	internal class TokenSerializer
	{
		private readonly string secret;
		private readonly TimeSpan duration;

		public TokenSerializer(string secret, TimeSpan duration)
		{
			this.secret = secret;
			this.duration = duration;
		}

		public string Serialize(IDictionary<string, string> claims)
		{
			var exp = (DateTimeOffset.UtcNow + duration).ToUnixTimeSeconds();
			var builder = new JwtBuilder()
			  .WithAlgorithm(new HMACSHA256Algorithm())
			  .WithSecret(secret)
			  .AddClaim("exp", exp);

			foreach (var claim in claims)
			{
				builder = builder.AddClaim(claim.Key, claim.Value);
			}

			return builder.Build();
		}

		public IDictionary<string, string> Deserilize(string token)
		{
			var dic = new JwtBuilder()
				.WithSecret(secret)
				.MustVerifySignature()
				.Decode<IDictionary<string, string>>(token);

			return dic.Where(kv => kv.Key != "exp")
				.ToDictionary(kv => kv.Key, kv => kv.Value);
		}
	}
}
