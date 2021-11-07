using System;

namespace IdentityPoc.Data.Enums
{
	[Flags]
	public enum OrganizationMembershipType
	{
		None = 0,
		Guest = 1,
		Member = 2,
		Manager = 4,
		Admin = 8
	}
}
