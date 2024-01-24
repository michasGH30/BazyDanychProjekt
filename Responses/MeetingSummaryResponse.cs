namespace bazyProjektBlazor.Responses
{
    public class MeetingSummaryResponse
    {
        public int MeetingID { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public bool IsOnline { get; set; }

        public string Link { get; set; } = string.Empty;

        public int RoomNumber { get; set; }

        public bool Creator { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
