using IdentityPoc.Web.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace IdentityPoc.Web.Controllers
{
	public class AccountController : Controller
	{
		[HttpGet]
		public IActionResult Login([FromQuery] LoginViewModel model)
		{
			return View(model);
		}
	}
}
