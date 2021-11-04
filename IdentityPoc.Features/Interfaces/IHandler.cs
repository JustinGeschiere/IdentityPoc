using System.Threading.Tasks;

namespace IdentityPoc.Features.Interfaces
{
	public interface IHandler
	{ }

	public interface IHandler<TCommand, TResult> : IHandler
	{
		TResult Handle(TCommand command);
	}

	public interface IHandlerAsync<TCommand, TResult> : IHandler
	{
		Task<TResult> HandleAsync(TCommand command);
	}
}
