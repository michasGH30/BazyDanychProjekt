using System.ComponentModel.DataAnnotations;

namespace bazyProjektBlazor.Requests
{
	public class CreateMeetingRequest
	{
		[Required(ErrorMessage = "Title is required.")]
		public string Title { get; set; } = string.Empty;

		[Required(ErrorMessage = "Date of Meeting is required.")]
		public DateOnly Date { get; set; }

		public int TypeOfMeeting { get; set; }

		public int RepetitionOfMeeting { get; set; }

		public List<int>? MembersID { get; set; }
	}
}
