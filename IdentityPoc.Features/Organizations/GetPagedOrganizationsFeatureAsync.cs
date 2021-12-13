using IdentityPoc.Data;
using IdentityPoc.Data.Entities;
using IdentityPoc.Features.Bases;
using IdentityPoc.Features.Helpers;
using IdentityPoc.Features.Interfaces;
using IdentityPoc.Features.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPoc.Features.Organizations
{
	public class GetPagedOrganizationsFeatureAsync : BaseFeatureAsync<GetPagedOrganizationsFeatureAsync.Validator, GetPagedOrganizationsFeatureAsync.Handler, GetPagedOrganizationsFeatureAsync.Command, GetPagedOrganizationsFeatureAsync.Result>
	{
		public GetPagedOrganizationsFeatureAsync(Validator validator, Handler handler)
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
			public Organization[] Items { get; set; }

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
				var query = _dataDbContext.Organizations;

				var skip = (command.Page - 1) * command.PageSize;
				var items = await query
					.Skip(skip)
					.Take(command.PageSize)
					.ToArrayAsync();

				var totalCount = await query.CountAsync();

				return new Result()
				{
					Items = items,
					Paging = new PagingModel(command.Page, command.PageSize, totalCount)
				};
			}
		}
	}
}
