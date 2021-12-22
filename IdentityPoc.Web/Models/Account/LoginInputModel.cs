using System.ComponentModel.DataAnnotations;

namespace IdentityPoc.Web.Models.Account
{
	public class LoginInputModel
	{
		[Required, EmailAddress]
		public string Email { get; set; }

		[Required, DataType(DataType.Password)]
		public string Password { get; set; }

		public string ReturnUrl { get; set; }
	}
}
