using WarGames.Contracts.Competitors;

namespace WarGames.Business.Managers
{
	[Obsolete("Practically Unused. Tear out in Phase 2.1")]
	public interface ICompetitorManager
	{
		public Task<IEnumerable<ICompetitor>> GetCompetitorsAsync();

		public Task<string> SaveCompetitor(ICompetitor competitor);
	}
}