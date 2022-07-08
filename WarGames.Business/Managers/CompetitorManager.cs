using WarGames.Contracts.Competitors;
using WarGames.Contracts.V2.Sides;
using WarGames.Resources;

namespace WarGames.Business.Managers
{
	[Obsolete("Version 2 contracts incoming.")]
	public class CompetitorManager : IPlayerSideManager
	{
		public readonly IRepository<ICompetitor, string> competitorRepository;

		public CompetitorManager(IRepository<ICompetitor, string> competitorRepository)
		{
			this.competitorRepository = competitorRepository;
		}

		public Task AddAsync(Player player)
		{
			throw new NotImplementedException();
		}

		public Task AddAsync(Side side)
		{
			throw new NotImplementedException();
		}

		public Task ChooseAsync(Player player, Side side)
		{
			throw new NotImplementedException();
		}
	}
}