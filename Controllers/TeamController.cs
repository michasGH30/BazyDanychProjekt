using bazyProjektBlazor.Responses;
using Microsoft.AspNetCore.Mvc;

namespace bazyProjektBlazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController(IConfiguration configuration) : Controller
    {
        [HttpGet]
        public async Task<AllTeamsResponse> GetAllTeams()
        {
            AllTeamsResponse response = new();

            // TODO: Change to get all Teams

            return await Task.FromResult(response);
        }

        [HttpGet("{my:bool}")]
        public async Task<TeamByIDResponse> GetMyTeam(bool my)
        {
            TeamByIDResponse response = new();

            // TODO: Change to get Team by ID from session

            Console.WriteLine(my);

            return await Task.FromResult(response);
        }

        [HttpGet("{id:int}")]

        public async Task<TeamByIDResponse> GetTeamByID(int id)
        {
            TeamByIDResponse response = new();

            Console.WriteLine(id);

            // TODO: Change to get Team by ID

            return await Task.FromResult(response);
        }
    }
}
