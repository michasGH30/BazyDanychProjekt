using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Models;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
    public interface IProfileService
    {
        public Task<User> GetProfile();

        public Task<bool> UpdateProfile(User request);
    }

    public class ProfileService(IConfiguration configuration, ICurrentUser currentUser) : IProfileService
    {
        public async Task<User> GetProfile()
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("SELECT firstName, lastName, email FROM users WHERE ID=@ID", connection);
            command.Parameters.AddWithValue("@ID", currentUser.ID);

            User response = new();
            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                response.ID = currentUser.ID;
                response.FirstName = reader.GetString("firstName");
                response.LastName = reader.GetString("lastName");
                response.Email = reader.GetString("email");
            }

            return await Task.FromResult(response);
        }

        public async Task<bool> UpdateProfile(User request)
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();
            using var command = new MySqlCommand("UPDATE users SET firstName=@firstName, lastName=@lastName, email=@email WHERE ID=@ID", connection);
            command.Parameters.AddWithValue("@firstName", request.FirstName);
            command.Parameters.AddWithValue("@lastName", request.LastName);
            command.Parameters.AddWithValue("@email", request.Email);
            command.Parameters.AddWithValue("@ID", request.ID);

            if (await command.ExecuteNonQueryAsync() > 0)
            {
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }
    }
}
