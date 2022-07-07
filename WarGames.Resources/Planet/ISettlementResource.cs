using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Resources.Planet
{
	public interface ISettlementResource
	{
		public Task AssignAsync(GameSession game, Country country, Settlement settlement);

		public Task AssignAsync(GameSession game, Player player, Settlement settlement);

		public Task AssignAsync(GameSession game, Side side, Settlement settlement);

		public Task<Settlement> RetrieveAsync(GameSession game, string settlementId);

		public Task<IEnumerable<Settlement>> RetrieveManyAsync(GameSession game);

		public Task<IEnumerable<Settlement>> RetrieveManyAsync(GameSession game, Country country);

		public Task<IEnumerable<Settlement>> RetrieveManyAsync(GameSession game, Player player);

		public Task<IEnumerable<Settlement>> RetrieveManyAsync(GameSession game, Side side);

		public Task SaveAsync(GameSession game, Settlement settlement);

		public Task SaveManyAsync(GameSession game, IEnumerable<Settlement> settlements);
	}
}