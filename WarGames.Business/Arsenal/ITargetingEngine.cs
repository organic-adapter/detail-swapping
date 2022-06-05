using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal
{
	public interface ITargetingEngine
	{
		public Task<IEnumerable<Target>> CalculateTargetsInRangeAsync(ICompetitor currentCompetitor, ICompetitor opposingCompetitor);

		public Task<IEnumerable<Settlement>> GetSettlementsAsync(ICompetitor opposingSide);
	}
}