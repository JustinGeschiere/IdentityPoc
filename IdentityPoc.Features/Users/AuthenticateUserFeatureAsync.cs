using IdentityPoc.Data;
using IdentityPoc.Data.Entities;
using IdentityPoc.Features.Bases;
using IdentityPoc.Features.Helpers;
using IdentityPoc.Features.Interfaces;
using IdentityPoc.Features.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPoc.Features.Users
{
	public class AuthenticateUserFeatureAsync : BaseFeatureAsync<AuthenticateUserFeatureAsync.Validator, AuthenticateUserFeatureAsync.Handler, AuthenticateUserFeatureAsync.Command, AuthenticateUserFeatureAsync.Result>
	{
		public AuthenticateUserFeatureAsync(Validator validator, Handler handler)
			: base(validator, handler)
		{ }

		public class Command
		{
			[Required, EmailAddress]
			public string Email { get; set; }

			[Required, DataType(DataType.Password)]
			public string Password { get; set; }
		}

		public class Result
		{
			public bool Success { get; set; }

			public string Token { get; set; }
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
			private readonly SignInManager<User> _signInManager;
			private readonly UserManager<User> _userManager;

			private readonly IHttpContextAccessor _httpContextAccessor;

			public Handler(SignInManager<User> signInManager, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
			{
				_signInManager = signInManager;
				_userManager = userManager;
				_httpContextAccessor = httpContextAccessor;
			}

			public async Task<Result> HandleAsync(Command command)
			{
				var signInResult = await _signInManager.PasswordSignInAsync(command.Email, command.Password, true, false);

				var token = string.Empty;

				if (signInResult.Succeeded)
				{
					var user = await _userManager.FindByNameAsync(command.Email);
					var authModel = new AuthModel(user.Id, DateTime.UtcNow.AddMinutes(20));
					token = TokenHelper.AuthToToken(authModel);

					_httpContextAccessor.HttpContext.Response.Cookies.Append("identity_poc_auth", token);
				}

				return new Result()
				{
					Token = token,
					Success = signInResult.Succeeded
				};
			}
		}
	}
}
