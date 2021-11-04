using IdentityPoc.Features.Interfaces;
using System;
using System.Threading.Tasks;

namespace IdentityPoc.Features.Bases
{
	public abstract class BaseFeatureAsync<TValidator, THandler, TCommand, TResult> : IFeatureAsync<TCommand, TResult>
	{
		private readonly IValidator<TCommand> _validator;
		private readonly IHandlerAsync<TCommand, TResult> _handler;

		public BaseFeatureAsync(IValidator<TCommand> validator, IHandlerAsync<TCommand, TResult> handler)
		{
			_validator = validator ?? throw new ArgumentNullException(nameof(validator));
			_handler = handler ?? throw new ArgumentNullException(nameof(handler));
		}

		public async Task<TResult> ExecuteAsync(TCommand command)
		{
			_validator.Validate(command);
			return await _handler.HandleAsync(command);
		}
	}
}
