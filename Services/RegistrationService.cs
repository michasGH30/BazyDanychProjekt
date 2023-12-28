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
			using var checkEmailConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
			checkEmailConnection.Open();

			using var checkEmailCommand = new MySqlCommand("SELECT users.ID FROM users WHERE email=@email", checkEmailConnection);

			checkEmailCommand.Parameters.AddWithValue("@email", request.Email);

			if (checkEmailCommand.ExecuteReader().HasRows)
			{
				return await Task.FromResult(1);
			}

			using var registerConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

			registerConnection.Open();

			using var registerCommand = new MySqlCommand("INSERT INTO users(firstName, lastName, email, password) VALUES (@firstName, @lastName, @email, @password)", registerConnection);
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
