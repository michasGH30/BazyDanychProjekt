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

        public Task<List<User>> GetMembersLeader();

        public Task<List<User>> GetMembersMember();

    }
    public class TeamsService(IConfiguration configuration, ICurrentUser currentUser, IUsersService usersService) : ITeamsService
    {
        public async Task<List<Team>> GetAllTeams()
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            using var command = new MySqlCommand(
                "SELECT ID FROM teams", connection);

            MySqlDataReader reader = await command.ExecuteReaderAsync();

            List<Team> teams = [];

            while (await reader.ReadAsync())
            {
                Team team = await GetTeamByID(reader.GetInt32("ID"));
                teams.Add(team);
            }

            return await Task.FromResult(teams);
        }

        public async Task<List<User>> GetMembersLeader()
        {
            List<User> response = [];

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT t1.memberID FROM teamsmembers t1 INNER JOIN teamsmembers t2 ON t1.teamID = t2.teamID WHERE t2.memberID = @ID AND t2.isLeader = 1 AND t1.memberID != @ID", connection);
            command.Parameters.AddWithValue("@ID", currentUser.ID);

            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                response.Add(await usersService.GetUserById(reader.GetInt32("memberID")));
            }

            return await Task.FromResult(response);
        }

        public async Task<List<User>> GetMembersMember()
        {
            List<User> response = [];

            using var membersConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            await membersConnection.OpenAsync();

            using var membersCommand = new MySqlCommand("SELECT tm1.memberID FROM teamsmembers tm1 INNER JOIN teamsmembers tm2 ON tm1.teamID = tm2.teamID WHERE tm1.memberID != tm2.memberID AND tm2.memberID = @ID", membersConnection);
            membersCommand.Parameters.AddWithValue("@ID", currentUser.ID);

            MySqlDataReader reader = await membersCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                response.Add(await usersService.GetUserById(reader.GetInt32("memberID")));
            }

            return await Task.FromResult(response);
        }

        public async Task<Team> GetMyTeam()
        {
            Team response = new();
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT teamsmembers.teamID FROM teamsmembers WHERE teamsmembers.memberID = @ID AND teamsmembers.isLeader = 1", connection);

            command.Parameters.AddWithValue("@ID", currentUser.ID);

            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                response = await GetTeamByID(reader.GetInt32("ID"));
            }

            return await Task.FromResult(response);

        }

        public async Task<Team> GetTeamByID(int id)
        {
            Team response = new();
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT teams.ID, teams.name FROM teams WHERE teams.ID=@ID", connection);

            command.Parameters.AddWithValue("@ID", id);

            MySqlDataReader reader = await command.ExecuteReaderAsync();
            List<User> members = [];
            while (await reader.ReadAsync())
            {
                response.ID = reader.GetInt32(0);
                response.Name = reader.GetString(1);


                List<string> DepartmentsNames = [];

                using var departmentsConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

                await departmentsConnection.OpenAsync();

                using var departmentsCommand = new MySqlCommand("SELECT departments.name FROM departments INNER JOIN teamsdepartments ON departments.ID = teamsdepartments.departmentID WHERE teamsdepartments.teamID = @ID", departmentsConnection);

                departmentsCommand.Parameters.AddWithValue("@ID", id);

                MySqlDataReader departmentReader = await departmentsCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    DepartmentsNames.Add(departmentReader.GetString(0));
                }

                response.DepartmentsNames = DepartmentsNames;

                using var membersConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
                await membersConnection.OpenAsync();

                using var membersCommand = new MySqlCommand("SELECT users.ID FROM users INNER JOIN teamsmembers ON teamsmembers.memberID = users.ID WHERE teamsmembers.teamID = @ID ORDER BY teamsmembers.isLeader DESC", membersConnection);
                membersCommand.Parameters.AddWithValue("@ID", id);
                MySqlDataReader membersReader = await membersCommand.ExecuteReaderAsync();

                bool leader = true;

                while (await membersReader.ReadAsync())
                {
                    if (leader)
                    {
                        response.Leader = await usersService.GetUserById(membersReader.GetInt32("ID"));
                        leader = false;
                    }
                    else
                    {
                        members.Add(await usersService.GetUserById(membersReader.GetInt32("ID")));
                    }
                }
            }
            response.Members = members;
            return await Task.FromResult(response);
        }
    }
}
