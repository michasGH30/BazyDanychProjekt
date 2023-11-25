using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Responses;
using bazyProjektBlazor.Utilities;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace bazyProjektBlazor.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LoginController(IConfiguration configuration) : Controller
	{
		[HttpPost]
		public async Task<LoginResponse> Login(LoginRequest request)
		{
			using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

			connection.Open();

			var query = "SELECT ID, role FROM users WHERE email=@email AND password=@password";
			using var command = new MySqlCommand(query, connection);
			command.Parameters.AddWithValue("@email", request.Email);
			command.Parameters.AddWithValue("@password", HashPassword.EncryptSHA256(request.Password));

			LoginResponse response = new();
			MySqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				response.Success = true;
				while (reader.Read())
				{
					HttpContext.Session.SetInt32("ID", reader.GetInt32(0));
					HttpContext.Session.SetString("role", reader.GetString("role"));

				}
			}
			else
				response.Success = false;

			connection.Close();

			return await Task.FromResult(response);
		}

		[HttpGet]
		public async Task<LoginResponse> Logged()
		{
			LoginResponse response = new();

			if(HttpContext.Session.Keys.Contains("ID"))
			{
				response.Success = true;
				response.Role = HttpContext.Session.GetString("role") ?? "user";
			}
			else
			{
				response.Success = false;
			}
			return await Task.FromResult(response);
		}
	}
}
