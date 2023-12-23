namespace bazyProjektBlazor.Models
{
    public class Meeting
    {
        public int ID { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public User Creator { get; set; }

        public string TypeOfMeeting { get; set; } = string.Empty;

        public string StatusOfMeeting { get; set; } = string.Empty;

        public string RepetitionOfMeeting { get; set; } = string.Empty;

        public List<User>? Members { get; set; }

        public List<MeetingMessage>? Messages { get; set; }

        public List<MeetingAttachment>? Attachments { get; set; }

    }
}
