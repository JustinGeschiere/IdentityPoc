using IdentityPoc.Data;
using IdentityPoc.Data.Entities;
using IdentityPoc.Data.Enums;
using IdentityPoc.Features.Bases;
using IdentityPoc.Features.Helpers;
using IdentityPoc.Features.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPoc.Features.OrganizationMemberships
{
	public class CreateOrganizationMembershipFeatureAsync : BaseFeatureAsync<CreateOrganizationMembershipFeatureAsync.Validator, CreateOrganizationMembershipFeatureAsync.Handler, CreateOrganizationMembershipFeatureAsync.Command, CreateOrganizationMembershipFeatureAsync.Result>
	{
		public CreateOrganizationMembershipFeatureAsync(Validator validator, Handler handler)
			: base(validator, handler)
		{ }

		public class Command
		{
			[Required, MaxLength(256)]
			public string FirstName { get; set; }

			[Required, MaxLength(256)]
			public string LastName { get; set; }

			[Required]
			public OrganizationMembershipType Type { get; set; }

			[Required]
			public Guid OrganizationId { get; set; }
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

				if (command.Type == OrganizationMembershipType.None)
				{
					throw new ValidationException($"Field '{nameof(command.Type)}' cannot must have a specified value");
				}

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
				var organization = await _dataDbContext.Organizations.FindAsync(command.OrganizationId);

				if (organization == null)
				{
					throw new ArgumentException("Organization not found while creating a membership");
				}

				var entity = new OrganizationMembership()
				{
					Id = new Guid(),
					FirstName = command.FirstName,
					LastName = command.LastName,
					Created = DateTime.UtcNow,
					Type = command.Type,
					Organization = organization
				};

				_dataDbContext.OrganizationMemberships.Add(entity);
				await _dataDbContext.SaveChangesAsync();

				return new Result(true);
			}
		}
	}
}
