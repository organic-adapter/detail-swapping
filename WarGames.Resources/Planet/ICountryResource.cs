using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Resources.Planet
{
	public interface ICountryResource
	{
		public Task AssignAsync(GameSession game, Player player, Country country);

		public Task AssignAsync(GameSession game, Side side, Country country);

		public Task<Country> RetrieveAsync(GameSession game, string countryId);

		public Task<IEnumerable<Country>> RetrieveManyAsync(GameSession game);

		public Task<IEnumerable<Country>> RetrieveManyAsync(GameSession game, Player player);

		public Task<IEnumerable<Country>> RetrieveManyAsync(GameSession game, Side side);

		public Task SaveAsync(GameSession game, Country country);

		public Task SaveManyAsync(GameSession game, IEnumerable<Country> countries);
	}
}