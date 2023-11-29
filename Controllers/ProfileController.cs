using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Responses;
using bazyProjektBlazor.Utilities;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Reflection;

namespace bazyProjektBlazor.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController(IConfiguration configuration) :Controller
    {
        [HttpGet]
        public async Task<ProfileResponse> GetProfile()
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            int? ID = HttpContext.Session.GetInt32("ID");

            connection.Open();

            var query = "SELECT firstName, lastName, email FROM users WHERE ID=@ID";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", ID);

            ProfileResponse response = new();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                response.FirstName = reader.GetString("firstName");
                response.LastName = reader.GetString("lastName");
                response.Email = reader.GetString("email");
            }

            connection.Close();

            return await Task.FromResult(response);
        }

        [HttpPut]
        public async Task<ChangeProfileResponse> ChangeProfile(ChangeProfileRequest request)
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            int? ID = HttpContext.Session.GetInt32("ID");

            connection.Open();
            var query = "UPDATE users SET firstName=@firstName, lastName=@lastName, email=@email WHERE ID=@ID";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@firstName", request.FirstName);
            command.Parameters.AddWithValue("@lastName", request.LastName);
            command.Parameters.AddWithValue("@email", request.Email);
            command.Parameters.AddWithValue("@ID", ID);

            int rows = command.ExecuteNonQuery();
            ChangeProfileResponse response = new();
            if (rows > 0)
            {
                response.Success = true;
            }
            else
            {
                response.Success = false;
            }
            //connection.Close();
            return await Task.FromResult(response);
        }
    }
}
