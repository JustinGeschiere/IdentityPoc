using IdentityPoc.Data;
using IdentityPoc.Data.Entities;
using IdentityPoc.Features.Bases;
using IdentityPoc.Features.Helpers;
using IdentityPoc.Features.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPoc.Features.Users
{
	public class InviteUserFeatureAsync : BaseFeatureAsync<InviteUserFeatureAsync.Validator, InviteUserFeatureAsync.Handler, InviteUserFeatureAsync.Command, InviteUserFeatureAsync.Result>
	{
		public InviteUserFeatureAsync(Validator validator, Handler handler)
			: base(validator, handler)
		{ }

		public class Command
		{
			public Guid MembershipId { get; set; }

			[EmailAddress, Required]
			public string EmailAddress { get; set; }
		}

		public class Result
		{
			public string InvitationUrl { get; set; }
			public DateTime Expires { get; set; }
		}

		public class Validator : IValidator<Command>
		{
			public void Validate(Command command)
			{
				var exceptions = ValidationHelper.ValidateAnnotations(command);

				if (exceptions.Any())
				{
					throw new AggregateException(exceptions);
				}
			}
		}

		public class Handler : IHandlerAsync<Command, Result>
		{
			private readonly DataDbContext _dataDbContext;

			public Handler(DataDbContext dataDbContext)
			{
				_dataDbContext = dataDbContext;
			}

			public async Task<Result> HandleAsync(Command command)
			{
				Guid? membershipId = null;

				// If MembershipId is provided, it should point to an existing OrganizationMembership entity
				if (command.MembershipId != Guid.Empty)
				{
					var membership = await _dataDbContext.OrganizationMemberships.FindAsync(command.MembershipId);

					if (membership == null)
					{
						throw new ArgumentException($"No membership was found for the provided '{nameof(command.MembershipId)}' value");
					}

					membershipId = command.MembershipId;
				}

				var invitation = new UserInvitation()
				{
					Id = Guid.NewGuid(),
					EmailAddress = command.EmailAddress,
					OrganizationMembershipId = membershipId,
					Expires = DateTime.UtcNow.AddDays(7)
				};

				_dataDbContext.UserInvitations.Add(invitation);
				await _dataDbContext.SaveChangesAsync();

				// TODO: Send email with token url
				var token = TokenHelper.GuidToToken(invitation.Id);

				return new Result()
				{
					InvitationUrl = $"https://localhost:5001/api/users/invitation/{token}",
					Expires = invitation.Expires
				};
			}
		}
	}
}
