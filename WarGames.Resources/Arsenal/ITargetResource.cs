using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Resources.Arsenal
{
	public interface ITargetResource
	{
		public Task AddTargetAsync(GameSession gameSession, Side side, Settlement settlement, TargetPriority targetPriority);

		public Task<IEnumerable<Target>> RetrieveAllAsync(GameSession gameSession);

		public Task<Target> RetrieveAsync(GameSession gameSession, Settlement settlement);

		public Task<IEnumerable<Target>> RetrieveManyAsync(GameSession gameSession, Side side);
	}
}