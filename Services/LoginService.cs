using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Utilities;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
	public interface ILoginService
	{
		public Task<bool> Login(LoginRequest loginModel);

		public Task<bool> IsLogged();

		public Task Logout();

		public Task<string> GetRoles();
	}
	public class LoginService(IConfiguration configuration, ICurrentUser currentUser) : ILoginService
	{
		public Task<string> GetRoles()
		{
			return Task.FromResult(currentUser.Roles);
		}

		public async Task<bool> IsLogged()
		{
			if (currentUser.ID == -1)
				return await Task.FromResult(false);
			return await Task.FromResult(true);
		}

		public async Task<bool> Login(LoginRequest request)
		{
			int id = -1;
			string roles = "user";
			using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
			connection.Open();

			var queryIsInDatabase = "SELECT ID, isAdmin FROM users WHERE email=@email AND password=@password";
			using var commandIsInDatabase = new MySqlCommand(queryIsInDatabase, connection);
			commandIsInDatabase.Parameters.AddWithValue("@email", request.Email);
			commandIsInDatabase.Parameters.AddWithValue("@password", Utility.EncryptSHA256(request.Password));

			using var readerIsInDatabase = commandIsInDatabase.ExecuteReader();
			if (!readerIsInDatabase.HasRows)
				return await Task.FromResult(false);
			else
			{
				while (readerIsInDatabase.Read())
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
					if (readerIsDirector.HasRows)
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
					if (readerIsLeader.HasRows)
					{
						roles += ",leader";
					}

				}
				currentUser.ID = id;
				currentUser.Roles = roles;
				return await Task.FromResult(true);
			}
		}

		public Task Logout()
		{
			currentUser.ID = -1;
			currentUser.Roles = string.Empty;
			return Task.CompletedTask;
		}
	}


}
