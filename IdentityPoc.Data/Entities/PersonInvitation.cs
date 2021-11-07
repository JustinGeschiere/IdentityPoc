using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityPoc.Data.Entities
{
	public class PersonInvitation
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string EmailAddress { get; set; }

		public DateTime Expires { get; set; }
	}
}
