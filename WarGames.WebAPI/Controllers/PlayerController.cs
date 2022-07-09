using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarGames.Business.Managers;

namespace WarGames.WebAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class PlayerController : ControllerBase
	{
		private readonly IGameManager gameManager;
		/*
		 * This should be a stateless API. Do not instantiate or persist things here
		 * that will linger within an API.
		 */

		public PlayerController(IGameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		[HttpPost("amount")]
		public async Task<IActionResult> SetPlayerAmount()
		{
			return BadRequest();
		}
	}
}