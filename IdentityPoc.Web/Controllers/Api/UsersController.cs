using IdentityPoc.Features.Users;
using IdentityPoc.Web.Extensions;
using IdentityPoc.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityPoc.Web.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	[ServiceFilter(typeof(ApiExceptionFilterAttribute))]
	public class UsersController : ControllerBase
	{
		[HttpGet("GetPagedUsers")]
		public async Task<IActionResult> GetPagedUsersAsync([FromServices] GetPagedUsersFeatureAsync feature, [FromQuery] GetPagedUsersFeatureAsync.Command command)
		{
			return await feature.ResolveAsync(command);
		}

		[HttpPost("InviteUser")]
		public async Task<IActionResult> InviteUserAsync([FromServices] InviteUserFeatureAsync feature, [FromForm] InviteUserFeatureAsync.Command command)
		{
			return await feature.ResolveAsync(command);
		}

		[HttpPost("AcceptUserInvite")]
		public async Task<IActionResult> AcceptUserInviteAsync([FromServices] AcceptUserInviteFeatureAsync feature, [FromForm] AcceptUserInviteFeatureAsync.Command command)
		{
			return await feature.ResolveAsync(command);
		}
	}
}
