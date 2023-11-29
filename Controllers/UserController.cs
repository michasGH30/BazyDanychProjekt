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

            using var command = new MySqlCommand("SELECT ID, firstName, lastName, email, isAdmin FROM users", connection);

            AllUsersResponse response = new()
            {
                Users = []
            };
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                response.Users.Add(new User() { Id = reader.GetInt32("ID"), FirstName = reader.GetString("firstName"), LastName = reader.GetString("lastName"), Email = reader.GetString("email"), IsAdmin = reader.GetBoolean(4) });
            }

            connection.Close();

            return await Task.FromResult(response);
        }

        [HttpGet("{id:int}")]
        public async Task<User> GetUserById(int id)
        {
            User response = new()
            {
                Id = -1,
                FirstName = "firstName",
                LastName = "lastName",
                Email = "email",
                IsAdmin = false
            };
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            using var command = new MySqlCommand("SELECT firstName, lastName, email, isAdmin FROM users WHERE ID=@ID", connection);
            command.Parameters.AddWithValue("@ID", id);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    response.Id = id;
                    response.FirstName = reader.GetString("firstName");
                    response.LastName = reader.GetString("lastName");
                    response.Email = reader.GetString("email");
                    response.IsAdmin = reader.GetBoolean(3);
                }

            }

            return await Task.FromResult(response);
        }
    }
}
