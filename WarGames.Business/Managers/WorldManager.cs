using WarGames.Business.Game;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;
using WarGames.Resources.Planet;

namespace WarGames.Business.Managers
{
	public class WorldManager : IWorldManager
	{
		private readonly CurrentGame currentGame;
		private readonly ISettlementResource settlementResource;

		public WorldManager
				(
					CurrentGame currentGame,
					ISettlementResource settlementResource
				)
		{
			this.currentGame = currentGame;
			this.settlementResource = settlementResource;
		}

		public async Task AssignAsync(Side side, Settlement settlement)
		{
			await settlementResource.SaveAsync(currentGame.GameSession, settlement);
			await settlementResource.AssignAsync(currentGame.GameSession, side, settlement);
		}

		public async Task<IEnumerable<Settlement>> GetSettlementsAsync(Side side)
		{
			return await settlementResource.RetrieveManyAsync(currentGame.GameSession, side);
		}
	}
}