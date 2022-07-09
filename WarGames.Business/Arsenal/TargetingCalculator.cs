using System.Collections.Concurrent;
using WarGames.Business.Game;
using WarGames.Contracts.Game;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;
using WarGames.Resources.Arsenal;
using WarGames.Resources.Planet;

namespace WarGames.Business.Arsenal
{
	public class TargetingCalculator : ITargetingCalculator
	{
		private readonly CurrentGame currentGame;
		private readonly IMissileDeliverySystemResource missileDeliverySystemResource;
		private readonly ITargetResource targetResource;

		public TargetingCalculator
				(
					CurrentGame currentGame
					, IMissileDeliverySystemResource missileDeliverySystemResource
					, ITargetResource targetResource
				)
		{
			this.currentGame = currentGame;
			this.missileDeliverySystemResource = missileDeliverySystemResource;
			this.targetResource = targetResource;
		}

		public async Task<IDictionary<Target, IEnumerable<IMissileDeliverySystem>>> CalculateTargetsInRangeAsync(Side currentSide, Side opposingSide)
		{
			var returnMe = new ConcurrentDictionary<Target, IEnumerable<IMissileDeliverySystem>>();
			var options = new ParallelOptions { MaxDegreeOfParallelism = 8 };
			var targets = await targetResource.RetrieveManyAsync(currentGame.GameSession, opposingSide);

			await Parallel.ForEachAsync(
					targets,
					options,
					async (target, token) =>
						{
							var mdsList = new List<IMissileDeliverySystem>();
							var mds = (await missileDeliverySystemResource.RetrieveManyAsync(currentGame.GameSession, currentSide)).ToList();
							mdsList.AddRange(mds.Where(mds => mds.InAttackRange(target.Key.Coord)));

							if (mdsList.Any())
								returnMe.TryAdd(target, mdsList);
						});

			return returnMe;
		}

		public async Task SetTargetAssignmentsByPriorityAsync(IEnumerable<Side> sides)
		{
			var side1 = sides.First();
			var side2 = sides.Last();
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