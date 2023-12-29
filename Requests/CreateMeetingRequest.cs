using bazyProjektBlazor.Models;
using System.ComponentModel.DataAnnotations;

namespace bazyProjektBlazor.Requests
{
	public class CreateMeetingRequest
	{
		public int ID { get; set; }

		[Required(ErrorMessage = "Title is required.")]
		public string Title { get; set; } = string.Empty;

		public DateOnly Date { get; set; }


		[StringLength(512, ErrorMessage = "Description cannot be longer than 512 characters")]
		public string? Description { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Select correct type of meeting.")]
		public int TypeOfMeeting { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Select correct repetition of meeting.")]
		public int RepetitionOfMeeting { get; set; }

		public int StatusOfMeeting { get; set; } = 1;

		public List<UserFormChange> MembersID { get; set; }

	}
}
