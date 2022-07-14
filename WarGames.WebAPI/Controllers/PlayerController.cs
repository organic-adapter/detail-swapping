using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using WarGames.Business.Managers;
using WarGames.Contracts.V2.Sides;
using WarGames.WebAPI.Models;

namespace WarGames.WebAPI.Controllers
{
	[Authorize]
	[RequiredScope("tasks.game")]
	[Route("api/[controller]")]
	[ApiController]
	public class PlayerController : ControllerBase
	{
		private readonly Dictionary<int, Func<string, Task>> addPlayerMaps;
		private readonly IGameManager gameManager;
		private readonly IPlayerSideManager playerSideManager;

		public PlayerController(IGameManager gameManager, IPlayerSideManager playerSideManager)
		{
			this.gameManager = gameManager;
			this.playerSideManager = playerSideManager;
			addPlayerMaps = new Dictionary<int, Func<string, Task>>();
			addPlayerMaps.Add(1, SetSinglePlayer);
			addPlayerMaps.Add(2, SetTwoPlayers);
		}

		[HttpPost("amount")]
		public async Task<IActionResult> SetPlayerAmount(SessionPlayerCount sessionPlayerCount)
		{
			await addPlayerMaps[sessionPlayerCount.Count](sessionPlayerCount.GameSessionId);
			return Ok();
		}

		private async Task SetSinglePlayer(string gameSessionId)
		{
			await gameManager.SetGameSession(gameSessionId);

			var human = new Player("Hooman", Guid.NewGuid().ToString(), PlayerType.Human);
			await playerSideManager.AddAsync(human);
		}

		private async Task SetTwoPlayers(string gameSessionId)
		{
			await gameManager.SetGameSession(gameSessionId);
			
			var human1 = new Player("Hooman1", Guid.NewGuid().ToString(), PlayerType.Human);
			var human2 = new Player("Hooman2", Guid.NewGuid().ToString(), PlayerType.Human);
			await playerSideManager.AddAsync(human1);
			await playerSideManager.AddAsync(human2);
		}
	}
}