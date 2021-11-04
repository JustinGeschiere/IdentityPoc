using System.Threading.Tasks;

namespace IdentityPoc.Features.Interfaces
{
	public interface IFeature
	{ }

	public interface IFeature<TCommand, TResult> : IFeature
	{
		TResult Execute(TCommand command);
	}

	public interface IFeatureAsync<TCommand, TResult> : IFeature
	{
		Task<TResult> ExecuteAsync(TCommand command);
	}
}
