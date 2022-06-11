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

		public async Task<IDictionary<Target, IEnumerable<IMissileDeliverySystem>>> CalculateTargetsInRangeAsync(ICompetitor currentCompetitor, ICompetitor opposingCompetitor)
		{
			var returnMe = new ConcurrentDictionary<Target, IEnumerable<IMissileDeliverySystem>>();
			var options = new ParallelOptions { MaxDegreeOfParallelism = 8 };
			var targets = await targetResource.GetAsync(opposingCompetitor);
			await Parallel.ForEachAsync(
					targets,
					options,
					async (target, token) =>
						{
							await Task.Run(() =>
							{
								var mdsList = currentCompetitor.MissileDeliverySystems.Where(mds => mds.InAttackRange(target.Key.Location)) ?? new List<IMissileDeliverySystem>();
								if (mdsList.Any())
									returnMe.TryAdd(target, mdsList);
							});
						});

			return returnMe;
		}

		public async Task<IEnumerable<Settlement>> GetSettlementsAsync(ICompetitor competitor)
		{
			return await Task.Run(() => competitor.Countries.SelectMany(country => country.Settlements));
		}

		public async Task SetTargetAssignmentsByPriorityAsync(IEnumerable<ICompetitor> competitors)
		{
			var side1 = competitors.First();
			var side2 = competitors.Last();
			var targetsInRangeSide1 = await CalculateTargetsInRangeAsync(side1, side2);
			var targetsInRangeSide2 = await CalculateTargetsInRangeAsync(side2, side1);
			SetTargetAssignments(targetsInRangeSide1);
			SetTargetAssignments(targetsInRangeSide2);
		}

		private void SetTargetAssignments(IDictionary<Target, IEnumerable<IMissileDeliverySystem>> targetsInRange)
		{
			foreach (var targetKvp in targetsInRange)
			{
				var target = targetKvp.Key;
				var nextMds = targetKvp.Value.FirstOrDefault(mds => !mds.HasTarget);
				if (nextMds != null)
					target.Assign(nextMds);
			}
		}
	}
}