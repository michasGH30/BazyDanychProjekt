using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Models;
using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Responses;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
    public interface IMessagesService
    {
        public Task<AddMessageResponse> SendNewMessage(AddNewMessageRequest message);

        public Task<MeetingMessage> GetMessageByID(int ID);

        public Task<bool> DeleteMessageByID(int ID);

        public Task<AddMessageResponse> UpdateMessage(AddNewMessageRequest message);
    }
    public class MessagesService(IConfiguration configuration, IUsersService usersService, ICurrentUser currentUser) : IMessagesService
    {
        public async Task<bool> DeleteMessageByID(int ID)
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("DELETE FROM meetingschats WHERE ID = @ID", connection);
            command.Parameters.AddWithValue("@ID", ID);

            if (await command.ExecuteNonQueryAsync() > 0)
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<MeetingMessage> GetMessageByID(int ID)
        {
            MeetingMessage response = new();

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("SELECT meetingschats.ID, meetingschats.message, meetingschats.senderID FROM meetingschats WHERE meetingschats.ID = @ID", connection);
            command.Parameters.AddWithValue("@ID", ID);

            MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                response.ID = reader.GetInt32("ID");
                response.Message = reader.GetString("message");
                response.Sender = await usersService.GetUserById(reader.GetInt32("senderID"));
                response.IsSender = reader.GetInt32("senderID") == currentUser.ID;
            }

            return await Task.FromResult(response);
        }

        public async Task<AddMessageResponse> SendNewMessage(AddNewMessageRequest message)
        {
            AddMessageResponse response = new();

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("INSERT INTO meetingschats(message,meetingID,senderID) VALUES(@M, @MEETINGID, @SID)", connection);
            command.Parameters.AddWithValue("@M", message.Message);
            command.Parameters.AddWithValue("@MEETINGID", message.MeetingID);
            command.Parameters.AddWithValue("@SID", currentUser.ID);

            if (await command.ExecuteNonQueryAsync() > 0)
            {
                response.SenderID = currentUser.ID;
                response.IsSuccess = true;
                response.ID = (int)command.LastInsertedId;
            }
            else
            {
                response.IsSuccess = false;
            }

            return await Task.FromResult(response);
        }

        public async Task<AddMessageResponse> UpdateMessage(AddNewMessageRequest message)
        {
            AddMessageResponse response = new();

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("UPDATE meetingschats SET message = @M WHERE ID = @ID", connection);
            command.Parameters.AddWithValue("@M", message.Message);
            command.Parameters.AddWithValue("@ID", message.ID);

            if (await command.ExecuteNonQueryAsync() > 0)
            {
                response.SenderID = currentUser.ID;
                response.IsSuccess = true;
                response.ID = message.ID;
            }
            else
            {
                response.IsSuccess = false;
            }

            return await Task.FromResult(response);
        }
    }
}
