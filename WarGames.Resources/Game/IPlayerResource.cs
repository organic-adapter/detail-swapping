using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Resources.Game
{
	public interface IPlayerResource
	{
		public Task<Player> RetrieveAsync(GameSession game, string playerId);

		public Task<IEnumerable<Player>> RetrieveManyAsync(GameSession game);

		public Task<IEnumerable<Player>> RetrieveManyAsync(GameSession game, PlayerType playerType);

		public Task SaveAsync(GameSession game, Player player);

		public Task SaveManyAsync(GameSession game, IEnumerable<Player> player);
	}
}