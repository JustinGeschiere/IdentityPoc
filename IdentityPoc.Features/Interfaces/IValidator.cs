namespace IdentityPoc.Features.Interfaces
{
	public interface IValidator
	{ }

	public interface IValidator<TCommand> : IValidator
	{
		void Validate(TCommand command);
	}
}
