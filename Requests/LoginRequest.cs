﻿using System.ComponentModel.DataAnnotations;

namespace bazyProjektBlazor.Requests
{
	public class LoginRequest
	{
		[Required(ErrorMessage = "Email is required.")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password name is required.")]
		public string Password { get; set; } = string.Empty;
	}
}
