using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityPoc.Data.Entities
{
	public class UserInvitation
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string EmailAddress { get; set; }

		public Guid? OrganizationMembershipId { get; set; }

		public DateTime Expires { get; set; }
	}
}
