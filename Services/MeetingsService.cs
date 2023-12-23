using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Models;
using bazyProjektBlazor.Requests;
using bazyProjektBlazor.Responses;
using MySqlConnector;

namespace bazyProjektBlazor.Services
{
    public interface IMeetingsService
    {
        public Task<List<Meeting>> GetAllMeetings();

        public Task<List<Meeting>> GetMeetingsFromMyDepartment();

        public Task<List<Meeting>> GetMeetingsFromMyTeam();

        public Task<List<Meeting>> GetMyMeetingsCreator();
        public Task<List<Meeting>> GetMyMeetingsMember();

        public Task<Meeting> GetMeetingByID(int id);

        public Task<bool> CreateMeeting(CreateMeetingRequest meeting);

        public Task<List<MeetingSummaryResponse>> GetMeetingSummaries();

        public Task<bool> DeleteMeetingByID(int id);
    }
    public class MeetingsService(IConfiguration configuration, ICurrentUser currentUser, IUsersService usersService) : IMeetingsService
    {
        public Task<bool> CreateMeeting(CreateMeetingRequest meeting)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteMeetingByID(int id)
        {
            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("DELETE FROM meetings WHERE meetings.ID = @ID", connection);
            command.Parameters.AddWithValue("@ID", id);

            if (command.ExecuteNonQuery() > 0)
            {
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<List<Meeting>> GetAllMeetings()
        {
            List<Meeting> response = [];

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("SELECT meetings.ID FROM meetings", connection);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Meeting meeting = await GetMeetingByID(reader.GetInt32(0));
                response.Add(meeting);
            }

            return await Task.FromResult(response);
        }

        public async Task<Meeting> GetMeetingByID(int id)
        {
            Meeting response = new();
            using var meetingConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            meetingConnection.Open();

            using var meetingCommand = new MySqlCommand("SELECT meetings.ID, meetings.title, meetings.date, typeofmeeting.type, repetitionofmeeting.repetition, statusofmeeting.status, users.ID FROM meetings INNER JOIN typeofmeeting on meetings.typeID = typeofmeeting.ID INNER JOIN repetitionofmeeting on meetings.repeatingID = repetitionofmeeting.ID INNER JOIN statusofmeeting on meetings.statusID = statusofmeeting.ID INNER JOIN users on meetings.creatorID = users.ID WHERE meetings.ID = @ID;", meetingConnection);
            meetingCommand.Parameters.AddWithValue("@ID", id);

            MySqlDataReader meetingsReader = meetingCommand.ExecuteReader();

            while (meetingsReader.Read())
            {
                response.ID = meetingsReader.GetInt32(0);
                response.Title = meetingsReader.GetString(1);
                response.Date = meetingsReader.GetDateOnly(2);
                response.TypeOfMeeting = meetingsReader.GetString(3);
                response.RepetitionOfMeeting = meetingsReader.GetString(4);
                response.StatusOfMeeting = meetingsReader.GetString(5);
                response.Creator = await usersService.GetUserById(meetingsReader.GetInt32(6));
            }

            List<User> members = [];

            using var membersConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            membersConnection.Open();

            using var membersCommand = new MySqlCommand("SELECT meetingsmembers.memberID FROM meetingsmembers WHERE meetingsmembers.meetingID = @ID", membersConnection);
            membersCommand.Parameters.AddWithValue("@ID", id);

            MySqlDataReader membersReader = membersCommand.ExecuteReader();

            while (membersReader.Read())
            {
                User member = await usersService.GetUserById(membersReader.GetInt32(0));
                members.Add(member);
            }

            List<MeetingMessage> messages = [];

            using var messagesConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            messagesConnection.Open();

            using var messagesCommand = new MySqlCommand("SELECT meetingschats.ID, meetingschats.message, meetingschats.senderID FROM meetingschats WHERE meetingschats.meetingID = @ID", messagesConnection);
            messagesCommand.Parameters.AddWithValue("@ID", id);

            MySqlDataReader messagesReader = messagesCommand.ExecuteReader();

            while (messagesReader.Read())
            {
                messages.Add(new()
                {
                    ID = messagesReader.GetInt32(0),
                    Message = messagesReader.GetString(1),
                    Sender = await usersService.GetUserById(messagesReader.GetInt32(2))
                });
            }

            List<MeetingAttachment> attachments = [];

            using var attachmentsConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            attachmentsConnection.Open();

            using var attachmentsCommand = new MySqlCommand("SELECT meetingattachments.ID, meetingattachments.name, typesofattachments.type, users.ID FROM meetingattachments INNER JOIN typesofattachments on meetingattachments.typeID = typesofattachments.ID INNER JOIN users ON meetingattachments.senderID = users.ID WHERE meetingattachments.meetingID = @ID", attachmentsConnection);

            attachmentsCommand.Parameters.AddWithValue("@ID", id);

            MySqlDataReader attachmentsReader = attachmentsCommand.ExecuteReader();

            while (attachmentsReader.Read())
            {
                attachments.Add(new()
                {
                    ID = attachmentsReader.GetInt32(0),
                    Name = attachmentsReader.GetString(1),
                    Type = attachmentsReader.GetString(2),
                    Sender = await usersService.GetUserById(attachmentsReader.GetInt32(3))
                });
            }

            response.Members = members;
            response.Messages = messages;
            response.Attachments = attachments;

            return await Task.FromResult(response);
        }

        public Task<List<Meeting>> GetMeetingsFromMyDepartment()
        {
            throw new NotImplementedException();
        }

        public Task<List<Meeting>> GetMeetingsFromMyTeam()
        {
            throw new NotImplementedException();
        }

        public async Task<List<MeetingSummaryResponse>> GetMeetingSummaries()
        {
            List<MeetingSummaryResponse> response = [];

            int myID = currentUser.ID;

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            using var command = new MySqlCommand("SELECT meetings.ID, meetings.title, meetings.date, meetings.creatorID, statusofmeeting.status FROM meetings INNER JOIN statusofmeeting ON meetings.statusID = statusofmeeting.ID WHERE meetings.creatorID = @ID OR meetings.ID IN (SELECT meetingsmembers.meetingID FROM meetingsmembers WHERE meetingsmembers.memberID = @ID)", connection);
            command.Parameters.AddWithValue("@ID", myID);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                response.Add(new()
                {
                    MessageID = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Date = reader.GetDateOnly(2),
                    Creator = reader.GetInt32(3) == myID,
                    Status = reader.GetString(4)
                });
            }

            return await Task.FromResult(response);
        }

        public async Task<List<Meeting>> GetMyMeetingsCreator()
        {
            List<Meeting> response = [];

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            int ID = currentUser.ID;

            using var command = new MySqlCommand("SELECT meetings.ID FROM meetings WHERE meetings.creatorID = @ID", connection);
            command.Parameters.AddWithValue("@ID", ID);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Meeting meeting = await GetMeetingByID(reader.GetInt32(0));
                response.Add(meeting);
            }

            return await Task.FromResult(response);
        }

        public async Task<List<Meeting>> GetMyMeetingsMember()
        {
            List<Meeting> response = [];

            using var connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            int ID = currentUser.ID;

            using var command = new MySqlCommand("SELECT meetings.ID FROM meetings WHERE meetings.ID IN (SELECT meetingsmembers.meetingID FROM meetingsmembers WHERE meetingsmembers.memberID = @ID)", connection);
            command.Parameters.AddWithValue("@ID", ID);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Meeting meeting = await GetMeetingByID(reader.GetInt32(0));
                response.Add(meeting);
            }

            return await Task.FromResult(response);
        }
    }
}
