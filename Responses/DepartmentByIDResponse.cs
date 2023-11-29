using bazyProjektBlazor.Models;

namespace bazyProjektBlazor.Responses
{
    public class DepartmentByIDResponse
    {
        public Department Department { get; set; }

        public IEnumerable<Team>? Teams { get; set; }
    }
}
