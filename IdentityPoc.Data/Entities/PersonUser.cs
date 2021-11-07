using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityPoc.Data.Entities
{
	public class PersonUser : IdentityUser<Guid>
	{
		[ForeignKey(nameof(Person))]
		public Guid PersonId { get; set; }

		public virtual Person Person { get; set; }
	}
}
