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

		public async Task<IEnumerable<Target>> GetAsync(Settlement settlement)
		{
			return await Task.Run(() => targets.Where(t => settlement.Equals(t.Key)));
		}

		public async Task<IEnumerable<Target>> GetAsync(Country country)
		{
			return await Task.Run(() => Get(country));
		}

		public async Task<IEnumerable<Target>> GetAsync(ICompetitor competitor)
		{
			return await Task.Run(() =>
			{
				var countries = targets.SelectMany(target => competitor.Countries);
				return countries.SelectMany(country => Get(country));
			});
		}

		private IEnumerable<Target> Get(Country country)
		{
			return targets.Where(t => country.Settlements.Any(settlement => settlement.Equals(t.Key)));
		}
	}
}