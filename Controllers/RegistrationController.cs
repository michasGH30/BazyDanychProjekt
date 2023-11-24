using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Responses;
using bazyProjektBlazor.Utilities;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace bazyProjektBlazor.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RegistrationController(IConfiguration configuration) : Controller
	{
		[HttpPost]
		public async Task<RegistrationResponse> Registration(RegistrationRequest request)
		{
			using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

			connection.Open();

			var query = "INSERT INTO users(firstName, lastName, email, password) VALUES (@firstName, @lastName, @email, @password)";
			using var command = new MySqlCommand(query, connection);
			command.Parameters.AddWithValue("@firstName", request.FirstName);
			command.Parameters.AddWithValue("@lastName", request.LastName);
			command.Parameters.AddWithValue("@email", request.Email);
			command.Parameters.AddWithValue("@password", HashPassword.EncryptSHA256(request.Password));

			int rows = command.ExecuteNonQuery();
			RegistrationResponse response = new();
			if(rows > 0)
			{
				response.Success = true;
			}
			else
			{
				response.Success = false;
			}
			connection.Close();
			return await Task.FromResult(response);
		}
	}
}
