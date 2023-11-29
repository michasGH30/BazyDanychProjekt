using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Responses;
using bazyProjektBlazor.Utilities;
using Microsoft.AspNetCore.Http;
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
            LoginResponse response = new();

            int id = -1;
            string roles = "user";
            using (var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var queryIsInDatabase = "SELECT ID, isAdmin FROM users WHERE email=@email AND password=@password";
                using var commandIsInDatabase = new MySqlCommand(queryIsInDatabase, connection);
                commandIsInDatabase.Parameters.AddWithValue("@email", request.Email);
                commandIsInDatabase.Parameters.AddWithValue("@password", HashPassword.EncryptSHA256(request.Password));

                using var readerIsInDatabase = commandIsInDatabase.ExecuteReader();
                if (readerIsInDatabase.HasRows)
                {
                    response.Success = true;

                    while(readerIsInDatabase.Read())
                    {
                        id = readerIsInDatabase.GetInt32(0);
                        if (readerIsInDatabase.GetBoolean(1))
                            roles += ",admin";
                    }

                    using (var directorConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection")))
                    {
                        directorConnection.Open();
                        using var commandIsDirector = new MySqlCommand("SELECT directorID FROM departments WHERE directorID=@directorID", directorConnection);
                        commandIsDirector.Parameters.AddWithValue("@directorID", id);
                        using var readerIsDirector = commandIsDirector.ExecuteReader();
                        if(readerIsDirector.HasRows)
                        {
                            roles += ",director";
                        }
                    }

                    using (var leaderConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection")))
                    {
                        leaderConnection.Open();
                        using var commandIsLeader = new MySqlCommand("SELECT leaderID FROM teams WHERE leaderID=@leaderID", leaderConnection);
                        commandIsLeader.Parameters.AddWithValue("@leaderID", id);
                        using var readerIsLeader = commandIsLeader.ExecuteReader();
                        if(readerIsLeader.HasRows)
                        {
                            roles += ",leader";
                        }
                            
                    }
                    response.Role = roles;
                    HttpContext.Session.SetInt32("ID", id);
                    HttpContext.Session.SetString("role", roles);
                }
                else
                    response.Success = false;
            }

            return await Task.FromResult(response);
		}

		[HttpGet]
		public async Task<LoginResponse> Logged()
		{
			LoginResponse response = new();

			if(HttpContext.Session.Keys.Contains("ID"))
			{
				response.Success = true;
				response.Role = HttpContext.Session.GetString("role");
			}
			else
			{
				response.Success = false;
			}
			return await Task.FromResult(response);
		}
	}
}
