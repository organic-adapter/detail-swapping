using System.Collections.Concurrent;
using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Arsenal;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Resources.Arsenal
{
	public class QuickAndDirtyTargetResource : ITargetResource
	{
		private readonly ConcurrentDictionary<GameSession, ConcurrentDictionary<Side, HashSet<Target>>> targets;

		public QuickAndDirtyTargetResource()
		{
			targets = new();
		}

		public async Task AddTargetAsync(GameSession gameSession, Side side, Settlement settlement, TargetPriority targetPriority)
		{
			await Task.Run(() =>
			{
				EnforceExistence(gameSession, side);

				var addMe = new Target(settlement, targetPriority);

				targets[gameSession][side].Add(addMe);
			});
		}

		public async Task<IEnumerable<Target>> RetrieveAllAsync(GameSession gameSession)
		{
			return await Task.Run(() => targets[gameSession].SelectMany(kvp => kvp.Value));
		}

		public async Task<Target> RetrieveAsync(GameSession gameSession, Settlement settlement)
		{
			return await Task.Run(() => 
							{
								foreach (KeyValuePair<Side, HashSet<Target>> side in targets[gameSession])
									foreach (Target target in side.Value)
										if (target.Key.Equals(settlement))
											return target;
								throw new KeyNotFoundException();
							});
		}

		public async Task<IEnumerable<Target>> RetrieveManyAsync(GameSession gameSession, Side side)
		{
			return await Task.Run(() => targets[gameSession][side]);
		}

		private void EnforceExistence(GameSession gameSession, Side side)
		{
			if (!targets.ContainsKey(gameSession))
				targets.TryAdd(gameSession, new());

			if (!targets[gameSession].ContainsKey(side))
				targets[gameSession].TryAdd(side, new HashSet<Target>());
		}
	}
}