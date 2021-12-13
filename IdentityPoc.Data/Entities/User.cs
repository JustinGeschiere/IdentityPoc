using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IdentityPoc.Data.Entities
{
	public class User : IdentityUser<Guid>
	{
		public virtual ICollection<OrganizationMembership> Memberships { get; set; }
	}
}
