using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityPoc.Data.Entities
{
	public class Organization
	{
		[Key]
		public Guid Id { get; set; }

		[Required, MaxLength(256)]
		public string Name { get; set; }

		public DateTime Created { get; set; }

		public DateTime? Modified { get; set; }

		[Required]
		public virtual ICollection<OrganizationMembership> OrganizationMemberships { get; set; }
	}
}
