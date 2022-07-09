using WarGames.Business.Game;
using WarGames.Contracts.V2.Sides;
using WarGames.Resources.Sides;

namespace WarGames.Business.Managers
{
	public class PlayerSideManager : IPlayerSideManager
	{
		public readonly CurrentGame currentGame;
		public readonly IPlayerResource playerResource;
		public readonly ISideResource sideResource;

		public PlayerSideManager
				(
					CurrentGame currentGame
					, IPlayerResource playerResource
					, ISideResource sideResource
				)
		{
			this.currentGame = currentGame;
			this.playerResource = playerResource;
			this.sideResource = sideResource;
		}

		public async Task AddAsync(params Player[] players)
		{
			foreach (var player in players)
				await AddAsync(player);
		}

		public async Task AddAsync(Player player)
		{
			await playerResource.SaveAsync(currentGame.GameSession, player);
		}

		public async Task AddAsync(params Side[] sides)
		{
			foreach (var side in sides)
				await AddAsync(side);
		}

		public async Task AddAsync(Side side)
		{
			await sideResource.SaveAsync(currentGame.GameSession, side);
		}

		public async Task ChooseAsync(Player player, Side side)
		{
			await EnforceUniqueSide(player, side);
			await sideResource.AssignAsync(currentGame.GameSession, player, side);
		}

		public int Count(PlayerType playerType)
		{
			return playerResource.RetrieveManyAsync(currentGame.GameSession, playerType).Result.Count();
		}

		public async Task<IEnumerable<Player>> GetPlayersAsync()
		{
			return await playerResource.RetrieveManyAsync(currentGame.GameSession);
		}

		public async Task<Side> GetSideAsync(string sideId)
		{
			return await sideResource.RetrieveAsync(currentGame.GameSession, sideId);
		}

		public bool HasPlayerType(PlayerType playerType)
		{
			return playerResource.RetrieveManyAsync(currentGame.GameSession, playerType).Result.Any();
		}

		public async Task<Side> NextAvailableSideAsync()
		{
			var sides = await sideResource.RetrieveManyAsync(currentGame.GameSession);
			foreach (var side in sides)
				if (await sideResource.IsAvailableAsync(currentGame.GameSession, side))
					return side;
			throw new NoAvailableSideException();
		}

		public async Task<Side> WhatIsPlayerAsync(Player player)
		{
			return await sideResource.RetrieveAsync(currentGame.GameSession, player);
		}

		private async Task EnforceUniqueSide(Player player, Side checkMe)
		{
			var opponentSide = await sideResource.RetrieveOpposingSideAsync(currentGame.GameSession, player);
			if (opponentSide == checkMe)
				throw new SideAlreadyTakenException();
		}

		public class NoAvailableSideException : Exception
		{
		}

		public class SideAlreadyTakenException : Exception
		{
		}
	}
}