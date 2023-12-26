using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Responses;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
    public interface IProfileService
    {
        public Task<ProfileResponse> GetProfile();

        public Task<bool> UpdateProfile(ChangeProfileRequest request);
    }

    public class ProfileService(IConfiguration configuration, ICurrentUser currentUser) : IProfileService
    {
        public async Task<ProfileResponse> GetProfile()
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("SELECT firstName, lastName, email FROM users WHERE ID=@ID", connection);
            command.Parameters.AddWithValue("@ID", currentUser.ID);

            ProfileResponse response = new();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                response.ID = currentUser.ID;
                response.FirstName = reader.GetString("firstName");
                response.LastName = reader.GetString("lastName");
                response.Email = reader.GetString("email");
            }

            return await Task.FromResult(response);
        }

        public async Task<bool> UpdateProfile(ChangeProfileRequest request)
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();
            using var command = new MySqlCommand("UPDATE users SET firstName=@firstName, lastName=@lastName, email=@email WHERE ID=@ID", connection);
            command.Parameters.AddWithValue("@firstName", request.FirstName);
            command.Parameters.AddWithValue("@lastName", request.LastName);
            command.Parameters.AddWithValue("@email", request.Email);
            command.Parameters.AddWithValue("@ID", request.ID);

            int rows = command.ExecuteNonQuery();
            if (rows > 0)
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
