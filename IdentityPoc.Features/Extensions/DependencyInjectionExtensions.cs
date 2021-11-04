using IdentityPoc.Features.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace IdentityPoc.Features.Extensions
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddFeatures(this IServiceCollection services)
		{
			return services.AddAssemblyFeatures();
		}

		private static IServiceCollection AddAssemblyFeatures(this IServiceCollection services)
		{
			var currentAssembly = Assembly.GetExecutingAssembly();

			var featureInterface = typeof(IFeature);
			var features = currentAssembly.GetTypes().Where(i => featureInterface.IsAssignableFrom(i) && !i.IsAbstract && !i.IsInterface);

			foreach (var feature in features)
			{
				services.AddScoped(feature);
			}

			var validatorInterface = typeof(IValidator);
			var validators = currentAssembly.GetTypes().Where(i => validatorInterface.IsAssignableFrom(i) && !i.IsAbstract && !i.IsInterface);

			foreach (var validator in validators)
			{
				services.AddScoped(validator);
			}

			var handlerInterface = typeof(IHandler);
			var handlers = currentAssembly.GetTypes().Where(i => handlerInterface.IsAssignableFrom(i) && !i.IsAbstract && !i.IsInterface);

			foreach (var handler in handlers)
			{
				services.AddScoped(handler);
			}

			return services;
		}
	}
}
