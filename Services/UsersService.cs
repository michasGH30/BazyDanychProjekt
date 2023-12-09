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
    public class UsersService(IConfiguration configuration) : IUsersService
    {
        public async Task<List<User>> GetAllUsers()
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("SELECT ID, firstName, lastName, email FROM users", connection);

            List<User> users = [];

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
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

            using var command = new MySqlCommand("SELECT ID FROM users WHERE ID NOT IN (SELECT memberID FROM teamsmembers) AND ID NOT IN (SELECT leaderID FROM teams) and ID NOT IN (SELECT directorID FROM departments) AND isAdmin = false", connection);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
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
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
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
