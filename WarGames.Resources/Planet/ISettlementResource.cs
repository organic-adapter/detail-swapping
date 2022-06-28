using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Resources.Planet
{
	public interface ISettlementResource
	{
		public Task<Settlement> RetrieveAsync(GameInstance game, string settlementId);

		public Task<IEnumerable<Settlement>> RetrieveManyAsync(GameInstance game);

		public Task<IEnumerable<Settlement>> RetrieveManyAsync(GameInstance game, Country country);

		public Task<IEnumerable<Settlement>> RetrieveManyAsync(GameInstance game, Player player);

		public Task<IEnumerable<Settlement>> RetrieveManyAsync(GameInstance game, Side side);
	}
}