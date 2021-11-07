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

		public OrganizationMembershipType Type { get; set; }

		public DateTime Created { get; set; }

		public DateTime? Modified { get; set; }

		[ForeignKey(nameof(Person))]
		public Guid PersonId { get; set; }

		[ForeignKey(nameof(Organization))]
		public Guid OrganizationId { get; set; }

		public virtual Person Person { get; set; }

		public virtual Organization Organization { get; set; }
	}
}
