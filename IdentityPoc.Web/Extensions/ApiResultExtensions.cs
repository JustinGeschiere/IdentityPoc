using IdentityPoc.Features.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityPoc.Web.Extensions
{
	public static class ApiResultExtensions
	{
		public static IActionResult Resolve<TCommand, TResult>(this IFeature<TCommand, TResult> feature, TCommand command)
		{
			var result = feature.Execute(command);
			return new JsonResult(result);
		}

		public static async Task<IActionResult> ResolveAsync<TCommand, TResult>(this IFeatureAsync<TCommand, TResult> feature, TCommand command)
		{
			var result = await feature.ExecuteAsync(command);
			return new JsonResult(result);
		}
	}
}
