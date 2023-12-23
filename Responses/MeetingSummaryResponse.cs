namespace bazyProjektBlazor.Responses
{
    public class MeetingSummaryResponse
    {
        public int MessageID { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public bool Creator { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
