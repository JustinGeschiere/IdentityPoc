using IdentityPoc.Data;
using IdentityPoc.Data.Entities;
using IdentityPoc.Features.Bases;
using IdentityPoc.Features.Helpers;
using IdentityPoc.Features.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPoc.Features.Users
{
	public class AcceptUserInviteFeatureAsync : BaseFeatureAsync<AcceptUserInviteFeatureAsync.Validator, AcceptUserInviteFeatureAsync.Handler, AcceptUserInviteFeatureAsync.Command, AcceptUserInviteFeatureAsync.Result>
	{
		public AcceptUserInviteFeatureAsync(Validator validator, Handler handler)
			: base(validator, handler)
		{ }

		public class Command
		{
			[Required, MaxLength(256)]
			public string Token { get; set; }

			[Required, DataType(DataType.Password)]
			public string Password { get; set; }
		}

		public class Result
		{
			public Result(bool succes)
			{
				Success = succes;
			}

			public bool Success { get; set; }
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
			private readonly UserManager<User> _userManager;

			public Handler(DataDbContext dataDbContext, UserManager<User> userManager)
			{
				_dataDbContext = dataDbContext;
				_userManager = userManager;
			}

			public async Task<Result> HandleAsync(Command command)
			{
				var invitationId = TokenHelper.TokenToGuid(command.Token);

				var invitation = _dataDbContext.UserInvitations.Find(invitationId);
				if (invitation == null || invitation.Expires <= DateTime.UtcNow)
				{
					throw new InvalidOperationException($"No activate invitation found for token '{command.Token}'");
				}

				var identityUser = new User()
				{
					UserName = invitation.EmailAddress,
					Email = invitation.EmailAddress,
					EmailConfirmed = true
				};

				var identityResult = await _userManager.CreateAsync(identityUser, command.Password);
				if (!identityResult.Succeeded)
				{
					var exceptions = identityResult.Errors.Select(i => new InvalidOperationException(i.Description));
					throw new AggregateException(exceptions);
				}

				// Assign OrganizationMembership to User, if provided
				if (invitation.OrganizationMembershipId != null)
				{
					var membership = await _dataDbContext.OrganizationMemberships
						.Where(i => i.Id == invitation.OrganizationMembershipId)
						.Include(i => i.User)
						.FirstOrDefaultAsync();

					if (membership != null)
					{
						var user = await _userManager.FindByEmailAsync(invitation.EmailAddress);
						membership.User = user;

						await _dataDbContext.SaveChangesAsync();
					}
				}

				return new Result(true);
			}
		}
	}
}
