using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal
{
	public interface ITargetingEngine
	{
		public Task<IDictionary<Target, IEnumerable<IMissileDeliverySystem>>> CalculateTargetsInRangeAsync(ICompetitor currentCompetitor, ICompetitor opposingCompetitor);

		public Task<IEnumerable<Settlement>> GetSettlementsAsync(ICompetitor opposingSide);

		Task SetTargetAssignmentsByPriorityAsync(IEnumerable<ICompetitor> competitors);
	}
}