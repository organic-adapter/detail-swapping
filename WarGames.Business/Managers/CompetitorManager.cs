using WarGames.Contracts.Competitors;
using WarGames.Resources;

namespace WarGames.Business.Managers
{
	public class CompetitorManager : ICompetitorManager
	{
		public readonly IRepository<ICompetitor, string> competitorRepository;

		public CompetitorManager(IRepository<ICompetitor, string> competitorRepository)
		{
			this.competitorRepository = competitorRepository;
		}

		public async Task<IEnumerable<ICompetitor>> GetCompetitorsAsync()
		{
			return await competitorRepository.GetAllAsync();
		}

		public async Task<string> SaveCompetitor(ICompetitor competitor)
		{
			return await competitorRepository.SaveAsync(competitor);
		}
	}
}