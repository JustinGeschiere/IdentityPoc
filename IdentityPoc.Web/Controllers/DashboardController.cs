using IdentityPoc.Data.Entities;
using IdentityPoc.Web.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityPoc.Web.Controllers
{
	[Authorize]
	public class DashboardController : Controller
	{
		[HttpGet]
		public async Task<IActionResult> Index([FromServices] UserManager<User> userManager)
		{
			var currentUser = await userManager.GetUserAsync(HttpContext.User);
			var model = new IndexViewModel() { Email = currentUser.Email };

			return View(model);
		}
	}
}
