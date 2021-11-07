using IdentityPoc.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityPoc.Data.Seeders
{
	internal static class PersonSeederExtensions
	{
		public static ModelBuilder SeedAdminPerson(this ModelBuilder modelBuilder)
		{
			Person person = new Person()
			{
				Id = Guid.Parse("fab4fac1-c546-41de-aebc-a14da6895711"),
				FirstName = "Super",
				LastName = "Admin",
				Created = DateTime.UtcNow
		};

			PersonUser user = new PersonUser()
			{
				Id = Guid.Parse("b74ddd14-6340-4840-95c2-db12554843e5"),
				UserName = "admin@hotseflots.nl",
				Email = "admin@hotseflots.nl",
				LockoutEnabled = false,
				PhoneNumber = "1234567890"
			};

			person.UserId = user.Id;
			user.PersonId = person.Id;

			PasswordHasher<PersonUser> passwordHasher = new PasswordHasher<PersonUser>();
			passwordHasher.HashPassword(user, "P@$$w0rd");

			modelBuilder.Entity<Person>().HasData(person);
			modelBuilder.Entity<PersonUser>().HasData(user);

			return modelBuilder;
		}

	}
}
