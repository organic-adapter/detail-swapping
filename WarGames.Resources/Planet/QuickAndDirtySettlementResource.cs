using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Resources.Planet
{
	public class QuickAndDirtySettlementResource : ISettlementResource
	{
		private readonly Dictionary<GameInstance, Dictionary<Country, HashSet<Settlement>>> countryMap;

		private readonly Dictionary<GameInstance, Dictionary<Player, HashSet<Settlement>>> playerMap;

		/// <summary>
		/// Can you read these? Didn't think so. This screams abstraction.
		///
		/// But we are going to prove a quick point first.
		/// </summary>
		private readonly Dictionary<GameInstance, Dictionary<string, Settlement>> settlements;

		private readonly Dictionary<GameInstance, Dictionary<Side, HashSet<Settlement>>> sideMap;

		public QuickAndDirtySettlementResource()
		{
			settlements = new();
			countryMap = new();
			playerMap = new();
			sideMap = new();
		}

		public QuickAndDirtySettlementResource
			(
				Bucket<Dictionary<GameInstance, Dictionary<string, Settlement>>> settlements,
				Bucket<Dictionary<GameInstance, Dictionary<Country, HashSet<Settlement>>>> countryMap,
				Bucket<Dictionary<GameInstance, Dictionary<Player, HashSet<Settlement>>>> playerMap,
				Bucket<Dictionary<GameInstance, Dictionary<Side, HashSet<Settlement>>>> sideMap
			)
		{
			this.settlements = settlements.Dump();
			this.countryMap = countryMap.Dump();
			this.playerMap = playerMap.Dump();
			this.sideMap = sideMap.Dump();
		}

		public async Task<Settlement> RetrieveAsync(GameInstance game, string SettlementId)
		{
			return await Task.Run(() => settlements[game][SettlementId]);
		}

		public async Task<IEnumerable<Settlement>> RetrieveManyAsync(GameInstance game)
		{
			return await Task.Run(() => settlements[game].Select(kvp => kvp.Value));
		}

		public async Task<IEnumerable<Settlement>> RetrieveManyAsync(GameInstance game, Player player)
		{
			return await Task.Run(() => playerMap[game][player]);
		}

		public async Task<IEnumerable<Settlement>> RetrieveManyAsync(GameInstance game, Side side)
		{
			return await Task.Run(() => sideMap[game][side]);
		}

		public async Task<IEnumerable<Settlement>> RetrieveManyAsync(GameInstance game, Country country)
		{
			return await Task.Run(() => countryMap[game][country]);
		}
	}
}