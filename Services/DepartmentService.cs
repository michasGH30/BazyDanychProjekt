using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Responses;

namespace bazyProjektBlazor.Services
{
    public interface IDepartmentService
    {
        public Task<AllDepartmentsResponse> GetAllDepartments();

        public Task<DepartmentByIDResponse> GetDepartmentByID();

        public Task<DepartmentByIDResponse> GetMyDepartmend();
    }
    public class DepartmentService(IConfiguration configuration, ICurrentUser currentUser, ITeamsService teamsService) : IDepartmentService
    {
        public Task<AllDepartmentsResponse> GetAllDepartments()
        {
            throw new NotImplementedException();
        }

        public Task<DepartmentByIDResponse> GetDepartmentByID()
        {
            throw new NotImplementedException();
        }

        public Task<DepartmentByIDResponse> GetMyDepartmend()
        {
            throw new NotImplementedException();
        }
    }
}
