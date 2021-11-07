using IdentityPoc.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityPoc.Web.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	[ServiceFilter(typeof(ApiExceptionFilterAttribute))]
	public class HealthController : ControllerBase
	{
		[HttpGet("Status")]
		public async Task<IActionResult> GetStatus()
		{
			dynamic result = new
			{
				Running = true,
				Available = true
			};

			return await Task.FromResult(new JsonResult(result));
		}

		[HttpGet("Throw400")]
		public Task<IActionResult> Throw400Async()
		{
			throw new InvalidOperationException();
		}

		[HttpGet("Throw403")]
		public Task<IActionResult> Throw403Async()
		{
			throw new UnauthorizedAccessException();
		}

		[HttpGet("Throw404")]
		public Task<IActionResult> Throw404Async()
		{
			throw new KeyNotFoundException();
		}

		[HttpGet("Throw500")]
		public Task<IActionResult> Throw500Async()
		{
			throw new Exception();
		}
	}
}
