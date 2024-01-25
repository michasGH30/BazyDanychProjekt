using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Utilities;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
    public interface IRegistrationService
    {
        public Task<int> Register(RegistrationRequest request);
    }
    public class RegistrationService(IConfiguration configuration) : IRegistrationService
    {
        public async Task<int> Register(RegistrationRequest request)
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            using var checkEmailCommand = new MySqlCommand("SELECT users.ID FROM users WHERE email=@email", connection);

            checkEmailCommand.Parameters.AddWithValue("@email", request.Email);

            MySqlDataReader reader = await checkEmailCommand.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                return await Task.FromResult(1);
            }

            await reader.CloseAsync();

            using var registerCommand = new MySqlCommand("INSERT INTO users(firstName, lastName, email, password) VALUES (@firstName, @lastName, @email, @password)", connection);
            registerCommand.Parameters.AddWithValue("@firstName", request.FirstName);
            registerCommand.Parameters.AddWithValue("@lastName", request.LastName);
            registerCommand.Parameters.AddWithValue("@email", request.Email);
            registerCommand.Parameters.AddWithValue("@password", Utility.EncryptSHA256(request.Password));

            if (registerCommand.ExecuteNonQuery() > 0)
            {
                return await Task.FromResult(0);
            }
            else
            {
                return await Task.FromResult(2);
            }
        }
    }
}
