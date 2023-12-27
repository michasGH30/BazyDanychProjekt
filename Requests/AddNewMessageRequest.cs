using System.ComponentModel.DataAnnotations;

namespace bazyProjektBlazor.Requests
{
    public class AddNewMessageRequest
    {
        public int MeetingID { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(255, ErrorMessage = "Message cannot be longer than 50 characters")]
        public string Message { get; set; } = string.Empty;
    }
}
