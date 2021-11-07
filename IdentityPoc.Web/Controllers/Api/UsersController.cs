using IdentityPoc.Features.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityPoc.Web.Extensions;
using IdentityPoc.Web.Filters;

namespace IdentityPoc.Web.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	[ServiceFilter(typeof(ApiExceptionFilterAttribute))]
	public class UsersController : ControllerBase
	{
		[HttpPost("CreateUser")]
		public async Task<IActionResult> CreateUserAsync([FromServices] CreateUserFeatureAsync feature, [FromForm] CreateUserFeatureAsync.Command command)
		{
			return await feature.ResolveAsync(command);
		}
	}
}
