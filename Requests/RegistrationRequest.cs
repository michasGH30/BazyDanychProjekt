using System.ComponentModel.DataAnnotations;

namespace bazyProjektBlazor.Requests
{
	public class RegistrationRequest
	{
		[Required(ErrorMessage = "First name is required.")]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Last name is required.")]
		public string LastName { get; set;} = string.Empty;

		[Required(ErrorMessage = "Email is required.")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password name is required.")]
		public string Password { get; set; } = string.Empty;
	}
}
