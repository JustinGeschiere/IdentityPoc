using IdentityPoc.Data;
using IdentityPoc.Data.Entities;
using IdentityPoc.Features.Bases;
using IdentityPoc.Features.Helpers;
using IdentityPoc.Features.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPoc.Features.Organizations
{
	public class CreateOrganizationFeatureAsync : BaseFeatureAsync<CreateOrganizationFeatureAsync.Validator, CreateOrganizationFeatureAsync.Handler, CreateOrganizationFeatureAsync.Command, CreateOrganizationFeatureAsync.Result>
	{
		public CreateOrganizationFeatureAsync(Validator validator, Handler handler)
			: base(validator, handler)
		{ }

		public class Command
		{
			[MaxLength(256), Required]
			public string Name { get; set; }
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

			public Handler(DataDbContext dataDbContext)
			{
				_dataDbContext = dataDbContext;
			}

			public async Task<Result> HandleAsync(Command command)
			{
				var entity = new Organization()
				{
					Id = new Guid(),
					Name = command.Name,
					Created = DateTime.UtcNow,
					Memberships = new Collection<OrganizationMembership>()
				};

				_dataDbContext.Organizations.Add(entity);
				await _dataDbContext.SaveChangesAsync();

				return new Result(true);
			}
		}
	}
}
