using System;
using System.Text;

namespace IdentityPoc.Features.Helpers
{
	public static class TokenHelper
	{
		public static string GuidToToken(Guid id)
		{
			var tokenBytes = Encoding.UTF8.GetBytes(id.ToString());
			return Convert.ToBase64String(tokenBytes);
		}

		public static Guid TokenToGuid(string token)
		{
			var tokenBytes = Convert.FromBase64String(token);
			return Guid.Parse(Encoding.UTF8.GetString(tokenBytes));
		}
	}
}
