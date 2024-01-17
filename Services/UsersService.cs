using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Models;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
    public interface IUsersService
    {
        public Task<List<User>> GetAllUsers();

        public Task<User> GetUserById(int id);

        public Task<List<User>> GetFreeUsers();
    }
    public class UsersService(IConfiguration configuration, ICurrentUser currentUser) : IUsersService
    {
        public async Task<List<User>> GetAllUsers()
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("SELECT ID, firstName, lastName, email FROM users WHERE ID != @ID", connection);
            command.Parameters.AddWithValue("@ID", currentUser.ID);

            List<User> users = [];

            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User() { ID = reader.GetInt32("ID"), FirstName = reader.GetString("firstName"), LastName = reader.GetString("lastName"), Email = reader.GetString("email") });
            }

            connection.Close();

            return await Task.FromResult(users);
        }

        public async Task<List<User>> GetFreeUsers()
        {
            List<User> users = [];

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("SELECT users.ID FROM users LEFT JOIN teamsmembers ON users.ID = teamsmembers.memberID LEFT JOIN departments ON users.ID = departments.directorID WHERE teamsmembers.memberID IS NULL AND departments.directorID IS NULL AND users.isAdmin = false", connection);

            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var resposne = await GetUserById(reader.GetInt32("ID"));
                users.Add(resposne);
            }

            return await Task.FromResult(users);
        }

        public async Task<User> GetUserById(int id)
        {
            User response = new()
            {
                ID = -1,
                FirstName = "firstName",
                LastName = "lastName",
                Email = "email"
            };
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            using var command = new MySqlCommand("SELECT firstName, lastName, email FROM users WHERE ID=@ID", connection);
            command.Parameters.AddWithValue("@ID", id);
            MySqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    response.ID = id;
                    response.FirstName = reader.GetString("firstName");
                    response.LastName = reader.GetString("lastName");
                    response.Email = reader.GetString("email");
                }

            }

            return await Task.FromResult(response);
        }
    }
}
