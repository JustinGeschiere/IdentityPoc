using IdentityPoc.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityPoc.Data.Entities
{
	public class OrganizationMembership
	{
		[Key]
		public Guid Id { get; set; }

		[Required, MaxLength(256)]
		public string FirstName { get; set; }

		[Required, MaxLength(256)]
		public string LastName { get; set; }

		public OrganizationMembershipType Type { get; set; }

		public DateTime Created { get; set; }

		public DateTime? Modified { get; set; }

		public virtual User User { get; set; }

		public virtual Organization Organization { get; set; }

		[NotMapped]
		public string FullName => $"{FirstName} {LastName}";
	}
}
