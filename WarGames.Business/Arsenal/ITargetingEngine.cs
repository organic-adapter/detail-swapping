using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Business.Arsenal
{
	public interface ITargetingEngine
	{
		public Task<IEnumerable<Settlement>> GetSettlementsAsync(ICompetitor opposingSide);
		public Task<IEnumerable<Target>> CalculateTargetsInRangeAsync(IPlayer activePlayer);
	}
}