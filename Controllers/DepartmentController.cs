using bazyProjektBlazor.Responses;
using Microsoft.AspNetCore.Mvc;

namespace bazyProjektBlazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController(IConfiguration configuration) : Controller
    {
        [HttpGet]
        public async Task<AllDepartmentsResponse> AllDepartments()
        {
            AllDepartmentsResponse response = new();

            // TODO: Change to get all Departments

            return await Task.FromResult(response);
        }

        [HttpGet("{my:bool}")]
        public async Task<DepartmentByIDResponse> GetMyDepartment(bool my)
        {
            DepartmentByIDResponse response = new();

            // TODO: Change to get Department by ID from session

            Console.WriteLine(my);

            return await Task.FromResult(response);
        }

        [HttpGet("{id:int}")]

        public async Task<DepartmentByIDResponse> GetDepartmentByID(int id)
        {
            DepartmentByIDResponse response = new();

            Console.WriteLine(id);

            // TODO: Change to get Department by ID

            return await Task.FromResult(response);
        }
    }
}
