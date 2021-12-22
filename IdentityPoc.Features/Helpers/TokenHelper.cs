using IdentityPoc.Features.Models;
using System;
using System.Text;

namespace IdentityPoc.Features.Helpers
{
	public static class TokenHelper
	{
		public static string GuidToToken(Guid id)
		{
			return StringToBase64(id.ToString());
		}

		public static Guid TokenToGuid(string token)
		{
			return Guid.Parse(Base64ToString(token));
		}

		public static string AuthToToken(AuthModel model)
		{
			return StringToBase64(model.ToString());
		}

		public static AuthModel TokenToAuth(string token)
		{
			return new AuthModel(Base64ToString(token));
		}

		private static string StringToBase64(string obj)
		{
			var bytes = Encoding.UTF8.GetBytes(obj);
			return Convert.ToBase64String(bytes);
		}

		private static string Base64ToString(string obj)
		{
			var bytes = Convert.FromBase64String(obj);
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
