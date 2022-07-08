﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarGames.Business.Managers;
using WarGames.WebAPI.Models.Competitors;

namespace WarGames.WebAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class PlayerController : ControllerBase
	{
		private readonly IGameManager gameManager;
		private readonly ICompetitorBasedGame competitorBasedGame;
		/*
		 * This should be a stateless API. Do not instantiate or persist things here
		 * that will linger within an API.
		 */

		public PlayerController(IGameManager gameManager)
		{
			this.gameManager = gameManager;
			competitorBasedGame = this.gameManager as ICompetitorBasedGame;
		}

		[HttpPost("amount")]
		public async Task<IActionResult> SetPlayerAmount()
		{
			return BadRequest();
		}

		[HttpPost("side")]
		public async Task<IActionResult> SetHumanSide(PlayerSelection playerSelection)
		{
			return BadRequest();
		}
		[HttpGet("sides/available")]
		public async Task<IActionResult> AvailableSides()
		{
			var sides = await competitorBasedGame.AvailableSidesAsync();
			return Ok(sides);
		}
	}
}