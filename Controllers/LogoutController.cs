using bazyProjektBlazor.Responses;
using bazyProjektBlazor.Requests;
using Microsoft.AspNetCore.Mvc;

namespace bazyProjektBlazor.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LogoutController : Controller
	{
		[HttpPost]
		public async Task<LogoutResponse> Logout(LogoutRequest request)
		{
			HttpContext.Session.Clear();
			return await Task.FromResult(new LogoutResponse());
		}
	}
}
