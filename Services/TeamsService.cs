using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Models;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
    public interface ITeamsService
    {
        public Task<List<Team>> GetAllTeams();

        public Task<Team> GetMyTeam();

        public Task<Team> GetTeamByID(int id);
    }
    public class TeamsService(IConfiguration configuration, ICurrentUser currentUser) : ITeamsService
    {
        public async Task<List<Team>> GetAllTeams()
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand(
                "SELECT teams.ID, teams.name, " +
                "users.ID, users.firstName, users.lastName, users.email, " +
                "departments.ID, departments.name, departments.directorID " +
                "FROM teams " +
                "INNER JOIN users on teams.leaderID = users.ID " +
                "INNER JOIN departments on teams.departmentID = departments.ID", connection);

            MySqlDataReader reader = command.ExecuteReader();

            List<Team> teams = [];

            while (reader.Read())
            {
                Team team = new()
                {
                    Name = reader.GetString(1),
                    ID = reader.GetInt32(0),
                    Leader = new User()
                    {
                        ID = reader.GetInt32(2),
                        FirstName = reader.GetString(3),
                        LastName = reader.GetString(4),
                        Email = reader.GetString(5),
                    },
                    Department = new Department()
                    {
                        ID = reader.GetInt32(6),
                        Name = reader.GetString(7),

                    }
                };
                using var departmentConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
                departmentConnection.Open();
                using var departmentCommand = new MySqlCommand("SELECT users.ID, users.firstName, users.lastName, users.email FROM users WHERE users.ID=@ID", departmentConnection);
                departmentCommand.Parameters.AddWithValue("@ID", reader.GetInt32(8));
                MySqlDataReader departmentReader = departmentCommand.ExecuteReader();
                while (departmentReader.Read())
                {
                    team.Department.Director = new User()
                    {
                        ID = departmentReader.GetInt32("ID"),
                        FirstName = departmentReader.GetString("firstName"),
                        LastName = departmentReader.GetString("lastName"),
                        Email = departmentReader.GetString("email")
                    };
                }
                using var membersConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
                membersConnection.Open();

                using var membersCommand = new MySqlCommand("SELECT users.ID, users.firstName, users.lastName, users.email FROM users INNER JOIN teamsmembers ON teamsmembers.memberID = users.ID WHERE teamsmembers.teamID = @ID", membersConnection);
                membersCommand.Parameters.AddWithValue("@ID", reader.GetInt32(0));
                MySqlDataReader membersReader = membersCommand.ExecuteReader();
                List<User> members = [];
                while (membersReader.Read())
                {
                    members.Add(new User()
                    {
                        ID = membersReader.GetInt32("ID"),
                        FirstName = membersReader.GetString("firstName"),
                        Email = membersReader.GetString("email"),
                        LastName = membersReader.GetString("lastName")
                    });
                }

                team.Members = members;
                teams.Add(team);
            }

            return await Task.FromResult(teams);
        }

        public Task<Team> GetMyTeam()
        {
            throw new NotImplementedException();
        }

        public Task<Team> GetTeamByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
