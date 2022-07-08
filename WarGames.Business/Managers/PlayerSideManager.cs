using WarGames.Business.Game;
using WarGames.Contracts.Game;
using WarGames.Contracts.V2.Sides;
using WarGames.Resources.Sides;

namespace WarGames.Business.Managers
{
	public class PlayerSideManager : IPlayerSideManager
	{
		public readonly CurrentGame currentGame;
		public readonly IPlayerResource playerResource;
		public readonly ISideResource sideResource;

		public PlayerSideManager(CurrentGame currentGame, IPlayerResource playerResource, ISideResource sideResource)
		{
			this.currentGame = currentGame;
			this.playerResource = playerResource;
			this.sideResource = sideResource;
		}

		public async Task AddAsync(Contracts.V2.Sides.Player player)
		{
			await playerResource.SaveAsync(currentGame.GameSession, player);
		}

		public async Task AddAsync(Contracts.V2.Sides.Side side)
		{
			await sideResource.SaveAsync(currentGame.GameSession, side);
		}

		public async Task ChooseAsync(Contracts.V2.Sides.Player player, Contracts.V2.Sides.Side side)
		{
			await sideResource.AssignAsync(currentGame.GameSession, player, side);
		}

		public int Count(PlayerType playerType)
		{
			return playerResource.RetrieveManyAsync(currentGame.GameSession, playerType).Result.Count();
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

		public class NoAvailableSideException : Exception
		{
		}
	}
}