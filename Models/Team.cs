namespace bazyProjektBlazor.Models
{
    public class Team
    {
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public Department Department { get; set; }

        public User Leader { get; set; }

        public IEnumerable<User>? Members { get; set; }

    }
}
