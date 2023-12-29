using bazyProjektBlazor.Models;

namespace bazyProjektBlazor.Responses
{
    public class DepartmentByIDResponse
    {
        public Department Department { get; set; }

        public List<Team> Teams { get; set; }
    }
}
