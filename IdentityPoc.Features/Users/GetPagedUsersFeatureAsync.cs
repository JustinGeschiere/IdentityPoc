using IdentityPoc.Data;
using IdentityPoc.Features.Bases;
using IdentityPoc.Features.Helpers;
using IdentityPoc.Features.Interfaces;
using IdentityPoc.Features.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPoc.Features.Users
{
	public class GetPagedUsersFeatureAsync : BaseFeatureAsync<GetPagedUsersFeatureAsync.Validator, GetPagedUsersFeatureAsync.Handler, GetPagedUsersFeatureAsync.Command, GetPagedUsersFeatureAsync.Result>
	{
		public GetPagedUsersFeatureAsync(Validator validator, Handler handler)
			: base(validator, handler)
		{ }

		public class Command
		{
			[Range(1, int.MaxValue)]
			public int Page { get; set; } = 1;

			[Range(1, int.MaxValue)]
			public int PageSize { get; set; } = 5;
		}

		public class Result
		{
			public class UserModel
			{
				public string Email { get; set; }

				public string[] Memberships { get; set; }

				public string[] Organizations { get; set; }
			}

			public UserModel[] Items { get; set; }

			public PagingModel Paging { get; set; }
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
				var query = _dataDbContext.Users
					.Include(i => i.Memberships)
					.ThenInclude(i => i.Organization);

				var skip = (command.Page - 1) * command.PageSize;
				var items = await query
					.Skip(skip)
					.Take(command.PageSize)
					.ToArrayAsync();

				var totalCount = await query.CountAsync();

				return new Result()
				{
					Items = items.Select(i => new Result.UserModel() 
					{ 
						Email = i.Email, 
						Memberships = i.Memberships.Select(i => i.FullName).ToArray(),
						Organizations = i.Memberships.Select(i => i.Organization.Name).ToArray()
					}).ToArray(),
					Paging = new PagingModel(command.Page, command.PageSize, totalCount)
				};
			}
		}
	}
}
