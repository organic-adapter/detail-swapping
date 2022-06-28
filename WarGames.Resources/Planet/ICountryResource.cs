using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Resources.Planet
{
	public interface ICountryResource
	{
		public Task<Country> RetrieveAsync(GameInstance game, string countryId);

		public Task<IEnumerable<Country>> RetrieveManyAsync(GameInstance game);

		public Task<IEnumerable<Country>> RetrieveManyAsync(GameInstance game, Player player);

		public Task<IEnumerable<Country>> RetrieveManyAsync(GameInstance game, Side side);
	}
}