using System.Collections.Concurrent;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Resources.Arsenal;

namespace WarGames.Business.Arsenal
{
	public class TargetingEngine : ITargetingEngine
	{
		private readonly ITargetResource targetResource;

		public TargetingEngine(ITargetResource targetResource)
		{
			this.targetResource = targetResource;
		}

		public async Task<IEnumerable<Target>> CalculateTargetsInRangeAsync(ICompetitor currentCompetitor, ICompetitor opposingCompetitor)
		{
			var returnMe = new ConcurrentBag<Target>();
			var options = new ParallelOptions { MaxDegreeOfParallelism = 8 };
			var targets = await targetResource.GetAsync(opposingCompetitor);
			await Parallel.ForEachAsync(
					targets,
					options,
					async (target, token) =>
						{
							await Task.Run(() =>
							{
								if (currentCompetitor.MissileDeliverySystems.Any(mds => mds.InAttackRange(target.Key.Location)))
									returnMe.Add(target);
							});
						});

			return returnMe;
		}

		public async Task<IEnumerable<Settlement>> GetSettlementsAsync(ICompetitor competitor)
		{
			return await Task.Run(() => competitor.Countries.SelectMany(country => country.Settlements));
		}
	}
}