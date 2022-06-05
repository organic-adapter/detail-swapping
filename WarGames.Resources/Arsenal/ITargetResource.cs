using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Resources.Arsenal
{
	public interface ITargetResource
	{
		public Task<Target> AddTargetAsync(Settlement settlement, TargetPriority targetPriority);

		public Task<IEnumerable<Target>> GetAsync(Settlement settlement);

		public Task<IEnumerable<Target>> GetAsync(Country country);

		public Task<IEnumerable<Target>> GetAsync(ICompetitor competitor);
	}
}