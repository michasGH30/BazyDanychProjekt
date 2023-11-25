using bazyProjektBlazor.Models;
using bazyProjektBlazor.Responses;
using bazyProjektBlazor.Utilities;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace bazyProjektBlazor.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController(IConfiguration configuration) : Controller
	{
		[HttpGet]
		public async Task<AllUsersResponse> GetAllUsers()
		{
			using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

			connection.Open();

			var query = "SELECT ID, firstName, lastName, email, role FROM users";
			using var command = new MySqlCommand(query, connection);

            AllUsersResponse response = new()
            {
                Users = []
            };
            MySqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
                response.Users.Add(new User() { Id = reader.GetInt32("ID"), FirstName = reader.GetString("firstName"), LastName = reader.GetString("lastName"), Email = reader.GetString("email"), Role = reader.GetString("role") });
			}

			connection.Close();

			return await Task.FromResult(response);
		}
	}
}
