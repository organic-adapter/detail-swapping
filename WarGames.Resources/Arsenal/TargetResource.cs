using System.Collections.Concurrent;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;

namespace WarGames.Resources.Arsenal
{
	public class TargetResource : ITargetResource
	{
		private readonly ConcurrentBag<Target> targets;

		public TargetResource()
		{
			targets = new ConcurrentBag<Target>();
		}

		public async Task<Target> AddTargetAsync(Settlement settlement, TargetPriority targetPriority)
		{
			return await Task.Run(() =>
			{
				var returnMe = new Target(settlement, targetPriority);

				targets.Add(returnMe);

				return returnMe;
			});
		}

		public async Task<IEnumerable<Target>> GetAllAsync()
		{
			return await Task.Run(() => targets);
		}

		public async Task<Target> GetAsync(Settlement settlement)
		{
			return await Task.Run(() => Get(settlement));
		}

		public async Task<IEnumerable<Target>> GetAsync(Country country)
		{
			return await Task.Run(() => Get(country));
		}

		public async Task<IEnumerable<Target>> GetAsync(ICompetitor competitor)
		{
			return await Task.Run(() =>
			{
				return competitor
						.Settlements
						.Where(settlement => targets.Any(target => target.Key.Equals(settlement)))
						.Select(settlement => Get(settlement));
			});
		}

		private Target Get(Settlement settlement)
		{
			return targets.First(target => target.Key.Equals(settlement));
		}

		private IEnumerable<Target> Get(Country country)
		{
			return targets.Where(t => country.Settlements.Any(settlement => settlement.Equals(t.Key)));
		}
	}
}