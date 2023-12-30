using System.ComponentModel.DataAnnotations;

namespace bazyProjektBlazor.Requests
{
    public class AddNewAttachmentRequest
    {
        public int ID { get; set; }
        public int MettingID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Select correct type of attachment.")]
        public int TypeOfAttachment { get; set; }
    }
}
