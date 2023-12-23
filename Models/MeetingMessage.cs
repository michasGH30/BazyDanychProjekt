namespace bazyProjektBlazor.Models
{
    public class MeetingMessage
    {
        public int ID { get; set; }

        public string Message { get; set; } = string.Empty;

        public User Sender { get; set; }
    }
}
