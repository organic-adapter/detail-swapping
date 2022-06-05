using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources;
using WarGames.Resources.Arsenal;

namespace WarGames.Business.Arsenal
{
	public class TargetingEngine : ITargetingEngine
	{
		private readonly IRepository<ICompetitor, string> competitorRepository;
		private readonly ITargetResource targetResource;

		public TargetingEngine(ITargetResource targetResource, IRepository<ICompetitor, string> competitorRepository)
		{
			this.competitorRepository = competitorRepository;
			this.targetResource = targetResource;
		}

		public async Task<IEnumerable<Target>> CalculateTargetsInRangeAsync(IPlayer activePlayer)
		{
			var returnMe = new List<Target>();

			return returnMe;
		}

		public async Task<IEnumerable<Settlement>> GetSettlementsAsync(ICompetitor competitor)
		{
			return await Task.Run(() => competitor.Countries.SelectMany(country => country.Settlements));
		}
	}
}