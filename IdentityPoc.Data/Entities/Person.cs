using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityPoc.Data.Entities
{
	public class Person
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		public DateTime Created { get; set; }

		public DateTime? Modified { get; set; }

		[ForeignKey(nameof(User))]
		public Guid? UserId { get; set; }

		public virtual PersonUser User { get; set; }

		public virtual ICollection<OrganizationMembership> OrganizationMemberships { get; set; }

		[NotMapped]
		public string FullName => $"{FirstName} {LastName}";
	}
}
