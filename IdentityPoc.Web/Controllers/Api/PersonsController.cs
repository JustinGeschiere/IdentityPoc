using IdentityPoc.Data;
using IdentityPoc.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityPoc.Web.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	[ServiceFilter(typeof(ApiExceptionFilterAttribute))]
	public class PersonsController : ControllerBase
	{
		private readonly DataDbContext _dataDbContext;

		public PersonsController(DataDbContext dataDbContext)
		{
			_dataDbContext = dataDbContext;
		}

		[HttpGet("PagedPersons")]
		public async Task<IActionResult> GetPagedPersons()
		{
			var persons = _dataDbContext.Persons.Take(10).ToArray();

			return await Task.FromResult(new JsonResult(persons));
		}
	}
}
