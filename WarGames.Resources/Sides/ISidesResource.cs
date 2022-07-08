using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Resources.Sides
{
	public interface ISideResource
	{
		public Task AssignAsync(GameSession game, Player player, Side side);

		public Task<bool> IsAvailableAsync(GameSession game, Side side);

		public Task<Side> RetrieveAsync(GameSession game, string sideId);

		public Task<Side> RetrieveAsync(GameSession game, Player player);

		public Task<IEnumerable<Side>> RetrieveManyAsync(GameSession game);

		public Task<IEnumerable<T>> RetrieveManyAsync<T>(GameSession game)
			where T : ISideUnique;

		public Task<Side> RetrieveOpposingSideAsync(GameSession game, Player player);

		public Task SaveAsync(GameSession game, Side side);

		public Task SaveManyAsync(GameSession game, IEnumerable<Side> sides);
	}
}