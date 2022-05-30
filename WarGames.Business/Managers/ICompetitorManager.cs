using WarGames.Contracts.Competitors;

namespace WarGames.Business.Managers
{
	public interface ICompetitorManager
	{
		public Task<IEnumerable<ICompetitor>> GetCompetitorsAsync();

		public Task<string> SaveCompetitor(ICompetitor competitor);
	}
}