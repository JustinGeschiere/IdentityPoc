using IdentityPoc.Data;
using IdentityPoc.Data.Entities;
using IdentityPoc.Features.Bases;
using IdentityPoc.Features.Helpers;
using IdentityPoc.Features.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityPoc.Features.Users
{
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
			private readonly UserManager<PersonUser> _userManager;

			public Handler(DataDbContext dataDbContext, UserManager<PersonUser> userManager)
			{
				_dataDbContext = dataDbContext;
				_userManager = userManager;
			}

			public async Task<Result> HandleAsync(Command command)
			{
				var tokenBytes = Convert.FromBase64String(command.InviteToken);
				var token = Encoding.UTF8.GetString(tokenBytes);

				if (!Guid.TryParse(token, out var invitationId))
				{
					throw new InvalidOperationException("Invalid invitation token provided");
				}

				var invitation = _dataDbContext.PersonInvitations.Find(invitationId);
				if (invitation == null || invitation.Expires <= DateTime.UtcNow)
				{
					throw new InvalidOperationException("No activate invitation could be found");
				}

				var identityUser = new PersonUser()
				{
					UserName = invitation.EmailAddress,
					Email = invitation.EmailAddress
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
