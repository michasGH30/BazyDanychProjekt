using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Models;
using bazyProjektBlazor.Responses;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
    public interface IDepartmentService
    {
        public Task<List<DepartmentByIDResponse>> GetAllDepartments();

        public Task<DepartmentByIDResponse> GetDepartmentByID(int id);

        public Task<DepartmentByIDResponse> GetMyDepartment();

        public Task<List<User>> GetUsersFromMyDepartment();
    }
    public class DepartmentService(IConfiguration configuration, ICurrentUser currentUser, ITeamsService teamsService, IUsersService usersService) : IDepartmentService
    {
        public async Task<List<DepartmentByIDResponse>> GetAllDepartments()
        {
            List<DepartmentByIDResponse> response = [];
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            using var command = new MySqlCommand(
                 "SELECT departments.ID FROM departments", connection);
            MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var departmentByID = await GetDepartmentByID(reader.GetInt32("ID"));
                response.Add(departmentByID);

            }
            return await Task.FromResult(response);
        }

        public async Task<DepartmentByIDResponse> GetDepartmentByID(int id)
        {
            DepartmentByIDResponse response = new();
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            using var command = new MySqlCommand("SELECT departments.ID, departments.name, departments.directorID FROM departments WHERE departments.ID=@ID", connection);
            command.Parameters.AddWithValue("@ID", id);
            MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Department department = new()
                {
                    ID = reader.GetInt32("ID"),
                    Name = reader.GetString("name"),
                    Director = await usersService.GetUserById(reader.GetInt32("directorID"))
                };
                response.Department = department;
                List<Team> teams = [];
                using var teamConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
                teamConnection.Open();
                using var teamCommand = new MySqlCommand("SELECT teams.ID FROM teams WHERE teams.departmentID=@ID", teamConnection);
                teamCommand.Parameters.AddWithValue("@ID", id);
                MySqlDataReader teamsReader = await teamCommand.ExecuteReaderAsync();
                while (await teamsReader.ReadAsync())
                {
                    Team team = await teamsService.GetTeamByID(teamsReader.GetInt32("ID"));
                    teams.Add(team);
                }
                response.Teams = teams;
            }

            return await Task.FromResult(response);
        }

        public async Task<DepartmentByIDResponse> GetMyDepartment()
        {
            DepartmentByIDResponse response = new();

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("SELECT departments.ID FROM departments WHERE departments.directorID=@ID", connection);
            command.Parameters.AddWithValue("@ID", currentUser.ID);
            MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                response = await GetDepartmentByID(reader.GetInt32("ID"));
            }

            return await Task.FromResult(response);
        }

        public async Task<List<User>> GetUsersFromMyDepartment()
        {
            List<User> response = [];

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("SELECT teamsmembers.memberID FROM teamsmembers INNER JOIN teams ON teamsmembers.teamID = teams.ID INNER JOIN departments ON teams.departmentID = departments.ID WHERE departments.directorID = @ID", connection);
            command.Parameters.AddWithValue("@ID", currentUser.ID);

            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                response.Add(await usersService.GetUserById(reader.GetInt32("memberID")));
            }

            return await Task.FromResult(response);
        }
    }
}
