using IdentityPoc.Data.Entities;
using IdentityPoc.Data.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityPoc.Data
{
	public class DataDbContext : IdentityDbContext<PersonUser, IdentityRole<Guid>, Guid>
	{
		public DbSet<Person> Persons { get; set; }
		public DbSet<Organization> Organizations { get; set; }
		public DbSet<OrganizationMembership> OrganizationMemberships { get; set; }
		public DbSet<PersonInvitation> PersonInvitations { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=.;Database=IdentityPoc;Integrated Security=True");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Person <-> PersonUser
			modelBuilder.Entity<PersonUser>()
				.HasOne(i => i.Person)
				.WithOne(i => i.User);

			// Person <->> OrganizationMembership
			modelBuilder.Entity<Person>()
				.HasMany(i => i.OrganizationMemberships)
				.WithOne(i => i.Person);

			// Organization <->> OrganizationMembership
			modelBuilder.Entity<Organization>()
				.HasMany(i => i.OrganizationMemberships)
				.WithOne(i => i.Organization);
		}
	}
}
