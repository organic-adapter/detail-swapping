using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarGames.WebAPI.Constants;

namespace WarGames.WebAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class MainController : ControllerBase
	{
		[HttpGet("games/list")]
		public IActionResult ListGames()
		{
			return Ok(Games.List);
		}
	}
}