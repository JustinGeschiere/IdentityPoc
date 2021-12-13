﻿using IdentityPoc.Data;
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
	[Obsolete("Replaced with invite flow")]
	public class CreateUserFeatureAsync : BaseFeatureAsync<CreateUserFeatureAsync.Validator, CreateUserFeatureAsync.Handler, CreateUserFeatureAsync.Command, CreateUserFeatureAsync.Result>
	{
		public CreateUserFeatureAsync(Validator validator, Handler handler)
			: base(validator, handler)
		{ }

		public class Command
		{
			[Required]
			public string InviteToken { get; set; }

			[Required]
			public string Password { get; set; }
		}

		public class Result
		{
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
				var invitationId = TokenHelper.TokenToGuid(command.InviteToken);

				var invitation = _dataDbContext.UserInvitations.Find(invitationId);
				if (invitation == null || invitation.Expires <= DateTime.UtcNow)
				{
					throw new InvalidOperationException($"No activate invitation found for token '{command.InviteToken}'");
				}

				var identityUser = new User()
				{
					UserName = invitation.EmailAddress,
					Email = invitation.EmailAddress,
					EmailConfirmed = true
				};

				var identityResult = await _userManager.CreateAsync(identityUser, command.Password);

				if (identityResult.Succeeded)
				{
					return new Result()
					{
						Success = true
					};
				}
				else
				{
					var exceptions = identityResult.Errors.Select(i => new InvalidOperationException(i.Description));
					throw new AggregateException(exceptions);
				}
			}
		}
	}
}
