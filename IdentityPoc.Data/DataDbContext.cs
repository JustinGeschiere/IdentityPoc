using IdentityPoc.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityPoc.Data
{
	public class DataDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
	{
		public DbSet<Organization> Organizations { get; set; }
		public DbSet<OrganizationMembership> OrganizationMemberships { get; set; }
		public DbSet<UserInvitation> UserInvitations { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=.;Database=IdentityPoc;Integrated Security=True");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// User <->> OrganizationMembership
			modelBuilder.Entity<User>()
				.HasMany(i => i.Memberships)
				.WithOne(i => i.User);

			// Organization <->> OrganizationMembership
			modelBuilder.Entity<Organization>()
				.HasMany(i => i.Memberships)
				.WithOne(i => i.Organization);
		}
	}
}
