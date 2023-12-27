using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Responses;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
    public interface IMessagesService
    {
        public Task<AddMessageResponse> SendNewMessage(AddNewMessageRequest message);
    }
    public class MessagesService(IConfiguration configuration, ICurrentUser currentUser) : IMessagesService
    {
        public async Task<AddMessageResponse> SendNewMessage(AddNewMessageRequest message)
        {
            AddMessageResponse response = new();

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("INSERT INTO meetingschats(message,meetingID,senderID) VALUES(@M, @MEETINGID, @SID)", connection);
            command.Parameters.AddWithValue("@M", message.Message);
            command.Parameters.AddWithValue("@MEETINGID", message.MeetingID);
            command.Parameters.AddWithValue("@SID", currentUser.ID);

            if (command.ExecuteNonQuery() > 0)
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
    }
}
