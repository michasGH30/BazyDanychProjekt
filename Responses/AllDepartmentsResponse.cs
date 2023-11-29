using bazyProjektBlazor.Models;

namespace bazyProjektBlazor.Responses
{
    public class AllDepartmentsResponse
    {
        public IEnumerable<Department>? Departments { get; set; }

        public IEnumerable<Team>? Teams { get; set; }
    }
}
