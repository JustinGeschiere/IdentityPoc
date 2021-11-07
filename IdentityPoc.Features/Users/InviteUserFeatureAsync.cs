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
			private readonly UserManager<PersonUser> _userManager;

			public Handler(DataDbContext dataDbContext, UserManager<PersonUser> userManager)
			{
				_dataDbContext = dataDbContext;
				_userManager = userManager;
			}

			public async Task<Result> HandleAsync(Command command)
			{
				var existingUser = await _userManager.FindByEmailAsync(command.EmailAddress);

				if (existingUser != null)
				{
					throw new InvalidOperationException($"E-mail address '{command.EmailAddress}' is already in use");
				}

				var invitation = new PersonInvitation()
				{
					Id = Guid.NewGuid(),
					EmailAddress = command.EmailAddress,
					Expires = DateTime.UtcNow.AddDays(7)
				};

				_dataDbContext.PersonInvitations.Add(invitation);
				await _dataDbContext.SaveChangesAsync();

				// TODO: Send email with token url
				var token = TokenHelper.GuidToToken(invitation.Id);

				// TODO: Define flow, should person be creatable and then claimable by token (then create user for it?)

				return new Result()
				{
					InvitationUrl = $"https://baseurl.nl/{token}",
					Expires = invitation.Expires
				};
			}
		}
	}
}
