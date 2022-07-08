using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Resources.Arsenal
{
	public interface ITargetResource
	{
		public Task<Target> AddTargetAsync(Contracts.V2.World.Settlement settlement, TargetPriority targetPriority);

		public Task<IEnumerable<Target>> GetAllAsync();

		public Task<IEnumerable<Target>> GetAsync(Contracts.V2.Sides.Side side);

		public Task<Target> GetAsync(Contracts.V2.World.Settlement settlement);

		public Task<IEnumerable<Target>> GetAsync(Country country);

		public Task<IEnumerable<Target>> GetAsync(ICompetitor competitor);
	}
}