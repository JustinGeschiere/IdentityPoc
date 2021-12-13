using IdentityPoc.Data;
using IdentityPoc.Data.Entities;
using IdentityPoc.Data.Enums;
using IdentityPoc.Features.Bases;
using IdentityPoc.Features.Helpers;
using IdentityPoc.Features.Interfaces;
using IdentityPoc.Features.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPoc.Features.OrganizationMemberships
{
	public class GetPagedOrganizationMembershipsFeatureAsync : BaseFeatureAsync<GetPagedOrganizationMembershipsFeatureAsync.Validator, GetPagedOrganizationMembershipsFeatureAsync.Handler, GetPagedOrganizationMembershipsFeatureAsync.Command, GetPagedOrganizationMembershipsFeatureAsync.Result>
	{
		public GetPagedOrganizationMembershipsFeatureAsync(Validator validator, Handler handler)
			: base(validator, handler)
		{ }

		public class Command
		{
			[Required]
			public Guid OrganizationId { get; set; }

			public OrganizationMembershipType Type { get; set; }

			[Range(1, int.MaxValue)]
			public int Page { get; set; } = 1;

			[Range(1, int.MaxValue)]
			public int PageSize { get; set; } = 5;
		}

		public class Result
		{
			public OrganizationMembership[] Items { get; set; }
			public PagingModel Paging { get; set; }
		}

		public class Validator : IValidator<Command>
		{
			public void Validate(Command command)
			{
				var exceptions = ValidationHelper.ValidateAnnotations(command);

				if (command.OrganizationId == Guid.Empty)
				{
					throw new ValidationException($"Field '{nameof(command.OrganizationId)}' cannot must have a specified value");
				}

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
				var query = _dataDbContext.OrganizationMemberships
					.Where(i => i.Organization.Id == command.OrganizationId);

				if (command.Type != OrganizationMembershipType.None)
				{
					query = query
						.Where(i => i.Type == command.Type);
				}

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
