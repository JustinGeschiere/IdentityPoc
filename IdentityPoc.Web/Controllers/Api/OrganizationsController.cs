using IdentityPoc.Features.Organizations;
using IdentityPoc.Web.Extensions;
using IdentityPoc.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityPoc.Web.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	[ServiceFilter(typeof(ApiExceptionFilterAttribute))]
	public class OrganizationsController : ControllerBase
	{
		[HttpGet("GetPagedOrganizations")]
		public async Task<IActionResult> GetPagedOrganizationsAsync([FromServices] GetPagedOrganizationsFeatureAsync feature, [FromQuery] GetPagedOrganizationsFeatureAsync.Command command)
		{
			return await feature.ResolveAsync(command);
		}

		[HttpPost("CreateOrganization")]
		public async Task<IActionResult> CreateOrganizationAsync([FromServices] CreateOrganizationFeatureAsync feature, [FromForm] CreateOrganizationFeatureAsync.Command command)
		{
			return await feature.ResolveAsync(command);
		}
	}
}
