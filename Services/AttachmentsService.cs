using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Responses;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
    public interface IAttachmentsService
    {
        public Task<List<TypesOfAttachmentsResponse>> GetAttachmentsTypes();

        public Task<AddAttachmentResponse> AddNewAttachment(AddNewAttachmentRequest request);

        public Task<string> GetTypeByID(int ID);
    }
    public class AttachmentsService(IConfiguration configuration, ICurrentUser currentUser) : IAttachmentsService
    {
        public async Task<AddAttachmentResponse> AddNewAttachment(AddNewAttachmentRequest request)
        {
            AddAttachmentResponse response = new();

            using var connetion = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connetion.Open();

            using var command = new MySqlCommand("INSERT INTO meetingattachments(name, typeID, meetingID, senderID) VALUES (@NAME,@TID,@MID,@SID)", connetion);
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

            connetion.Open();

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

        public async Task<string> GetTypeByID(int ID)
        {
            using var connetion = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connetion.Open();

            using var command = new MySqlCommand("SELECT typesofattachments.type FROM typesofattachments WHERE typesofattachments.ID = @ID", connetion);
            command.Parameters.AddWithValue("@ID", ID);

            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                return await Task.FromResult(reader.GetString("type"));
            }

            return await Task.FromResult("");
        }
    }
}
