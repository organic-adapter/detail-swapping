using WarGames.Business.Game;
using WarGames.Contracts.V2.Sides;
using WarGames.Resources.Game;
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

		public async Task AddAsync(Side side)
		{
			await sideResource.SaveAsync(currentGame.GameSession, side);
		}

		public async Task ChooseAsync(Contracts.V2.Sides.Player player, Side side)
		{
			await sideResource.AssignAsync(currentGame.GameSession, player, side);
		}
	}
}