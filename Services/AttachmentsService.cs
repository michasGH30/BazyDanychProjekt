using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Models;
using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Responses;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
    public interface IAttachmentsService
    {
        public Task<List<TypesOfAttachmentsResponse>> GetAttachmentsTypes();

        public Task<AddAttachmentResponse> AddNewAttachment(AddNewAttachmentRequest request);
        public Task<AddAttachmentResponse> UpdateAttachment(AddNewAttachmentRequest request);

        public Task<string> GetTypeByID(int ID);

        public Task<MeetingAttachment> GetAttachmentByID(int ID);

        public Task<bool> DeleteAttachmentByID(int ID);
    }
    public class AttachmentsService(IConfiguration configuration, IUsersService usersService, ICurrentUser currentUser) : IAttachmentsService
    {
        public async Task<AddAttachmentResponse> AddNewAttachment(AddNewAttachmentRequest request)
        {
            AddAttachmentResponse response = new();

            using var connetion = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            await connetion.OpenAsync();

            using var command = new MySqlCommand("INSERT INTO meetingsattachments(name, typeID, meetingID, senderID) VALUES (@NAME,@TID,@MID,@SID)", connetion);
            command.Parameters.AddWithValue("@NAME", request.Name);
            command.Parameters.AddWithValue("@TID", request.TypeOfAttachment);
            command.Parameters.AddWithValue("@MID", request.MettingID);
            command.Parameters.AddWithValue("@SID", currentUser.ID);

            if (await command.ExecuteNonQueryAsync() > 0)
            {
                response.ID = (int)command.LastInsertedId;
                response.SenderID = currentUser.ID;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }

            return await Task.FromResult(response);
        }

        public async Task<List<TypesOfAttachmentsResponse>> GetAttachmentsTypes()
        {
            List<TypesOfAttachmentsResponse> response = [];

            using var connetion = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            await connetion.OpenAsync();

            using var command = new MySqlCommand("SELECT typesofattachments.ID, typesofattachments.type FROM typesofattachments", connetion);

            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                response.Add(new()
                {
                    ID = reader.GetInt32("ID"),
                    Type = reader.GetString("type"),
                });
            }

            return await Task.FromResult(response);
        }

        public async Task<MeetingAttachment> GetAttachmentByID(int ID)
        {
            MeetingAttachment response = new();

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT meetingsattachments.ID, meetingsattachments.name, typesofattachments.type, meetingsattachments.senderID FROM meetingsattachments INNER JOIN typesofattachments on meetingsattachments.typeID = typesofattachments.ID WHERE meetingsattachments.ID = @ID", connection);

            command.Parameters.AddWithValue("@ID", ID);

            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                response.ID = reader.GetInt32("ID");
                response.Name = reader.GetString("name");
                response.Type = reader.GetString("type");
                response.Sender = await usersService.GetUserById(reader.GetInt32("senderID"));
                response.IsSender = reader.GetInt32("senderID") == currentUser.ID;
            }

            return await Task.FromResult(response);
        }

        public async Task<string> GetTypeByID(int ID)
        {
            using var connetion = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            await connetion.OpenAsync();

            using var command = new MySqlCommand("SELECT typesofattachments.type FROM typesofattachments WHERE typesofattachments.ID = @ID", connetion);
            command.Parameters.AddWithValue("@ID", ID);

            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                return await Task.FromResult(reader.GetString("type"));
            }

            return await Task.FromResult("");
        }

        public async Task<bool> DeleteAttachmentByID(int ID)
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            using var command = new MySqlCommand("DELETE FROM meetingsattachments WHERE ID = @ID", connection);
            command.Parameters.AddWithValue("@ID", ID);

            if (await command.ExecuteNonQueryAsync() > 0)
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);

        }

        public async Task<AddAttachmentResponse> UpdateAttachment(AddNewAttachmentRequest request)
        {
            AddAttachmentResponse response = new();

            using var connetion = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            await connetion.OpenAsync();

            using var command = new MySqlCommand("UPDATE meetingsattachments SET name = @NAME, typeID = @TID WHERE ID = @ID", connetion);
            command.Parameters.AddWithValue("@NAME", request.Name);
            command.Parameters.AddWithValue("@TID", request.TypeOfAttachment);
            command.Parameters.AddWithValue("@ID", request.ID);

            if (await command.ExecuteNonQueryAsync() > 0)
            {
                response.ID = request.ID;
                response.SenderID = currentUser.ID;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }

            return await Task.FromResult(response);
        }
    }
}
