using AutoMapper;
using System.Collections.Concurrent;
using WarGames.Contracts.Arsenal;
using WarGames.Contracts.Competitors;
using WarGames.Contracts.Game;
using WarGames.Contracts.V2.Sides;

namespace WarGames.Resources.Arsenal
{
	public class TargetResource : ITargetResource
	{
		private readonly ConcurrentBag<Target> targets;
		private readonly IMapper mapper;
		public TargetResource()
		{
			targets = new ConcurrentBag<Target>();
		}
		public TargetResource(IMapper mapper)
		{
			this.mapper = mapper;
			targets = new ConcurrentBag<Target>();
		}

		public async Task<Target> AddTargetAsync(Contracts.V2.World.Settlement settlement, TargetPriority targetPriority)
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

		public async Task<Target> GetAsync(Contracts.V2.World.Settlement settlement)
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
				//WHAT'S This? Oh noes! We forgot about mutability and immutability. By doing this we lose the reference to the competitor settlements.
				//This actually creates a bug when it tries to calculate damage.
				var settlements = mapper.Map<List<Contracts.V2.World.Settlement>>(competitor.Settlements);
				return settlements
						.Where(settlement => targets.Any(target => target.Key.Equals(settlement)))
						.Select(settlement => Get(settlement));
			});
		}

		public async Task<IEnumerable<Target>> GetAsync(Side side)
		{
			throw new NotImplementedException();
		}

		private Target Get(Contracts.V2.World.Settlement settlement)
		{
			return targets.First(target => target.Key.Equals(settlement));
		}

		private IEnumerable<Target> Get(Country country)
		{
			return targets.Where(t => country.Settlements.Any(settlement => settlement.Equals(t.Key)));
		}
	}
}