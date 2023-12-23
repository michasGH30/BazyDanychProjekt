namespace bazyProjektBlazor.Models
{
    public class MeetingAttachment
    {
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public User Sender { get; set; }
    }
}
