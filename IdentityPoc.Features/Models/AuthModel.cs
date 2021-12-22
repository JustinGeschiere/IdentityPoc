using System;

namespace IdentityPoc.Features.Models
{
	public class AuthModel
	{
		public AuthModel(Guid userId, DateTime expires)
		{
			Id = Guid.NewGuid();
			UserId = userId;
			Expires = expires;
		}

		public AuthModel(string token)
		{
			var segments = token.Split('%');

			Id = Guid.Parse(segments[0]);
			UserId = Guid.Parse(segments[1]);
			Expires = DateTime.Parse(segments[2]);
		}

		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public DateTime Expires { get; set; }

		public override string ToString()
		{
			return $"{Id}%{UserId}%{Expires}";
		}
	}
}
