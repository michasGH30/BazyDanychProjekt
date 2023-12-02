namespace bazyProjektBlazor.Auth
{
    public class CurrentUser : ICurrentUser
    {
        public int ID { get; set; } = -1;
        public string Roles { get; set; } = string.Empty;
    }

    public interface ICurrentUser
    {
        public int ID { get; set; }

        public string Roles { get; set; }
    }
}
