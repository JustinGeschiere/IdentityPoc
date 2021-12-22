using IdentityPoc.Data.Entities;
using IdentityPoc.Web.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityPoc.Web.Controllers
{
	public class AccountController : Controller
	{
		[HttpGet]
		public IActionResult Login([FromQuery] LoginViewModel model)
		{
			return View(model);
		}

		// TODO: Move to api
		// TODO: Set auth cookie token
		[HttpPost]
		[ActionName("Login")]
		public async Task<IActionResult> LoginPostAsync([FromServices] SignInManager<User> signInManager, [FromForm] LoginInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction(nameof(Login), new LoginViewModel() { ReturnUrl = model.ReturnUrl });
			}

			var signInResult = await signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

			if (signInResult.Succeeded)
			{
				return Redirect(model.ReturnUrl);
			}

			return RedirectToAction(nameof(Login));
		}
	}
}
