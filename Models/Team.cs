namespace bazyProjektBlazor.Models
{
    public class Team
    {
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<string> DepartmentsNames { get; set; }

        public User Leader { get; set; }

        public List<User> Members { get; set; }

    }
}
