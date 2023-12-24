namespace bazyProjektBlazor.Models
{
	public class UserForm(User user)
	{
		public int ID { get; set; } = user.ID;
		public string Name { get; set; } = $"{user.FirstName} {user.LastName}";
		public bool IsSelected { get; set; } = false;
	}
}
