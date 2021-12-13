using IdentityPoc.Features.OrganizationMemberships;
using IdentityPoc.Web.Extensions;
using IdentityPoc.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityPoc.Web.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	[ServiceFilter(typeof(ApiExceptionFilterAttribute))]
	public class OrganizationMembershipsController : ControllerBase
	{
		[HttpGet("GetPagedMemberships")]
		public async Task<IActionResult> GetPagedOrganizationsAsync([FromServices] GetPagedOrganizationMembershipsFeatureAsync feature, [FromQuery] GetPagedOrganizationMembershipsFeatureAsync.Command command)
		{
			return await feature.ResolveAsync(command);
		}

		[HttpPost("CreateMembership")]
		public async Task<IActionResult> CreateOrganizationMembershipAsync([FromServices] CreateOrganizationMembershipFeatureAsync feature, [FromForm] CreateOrganizationMembershipFeatureAsync.Command command)
		{
			return await feature.ResolveAsync(command);
		}
	}
}
