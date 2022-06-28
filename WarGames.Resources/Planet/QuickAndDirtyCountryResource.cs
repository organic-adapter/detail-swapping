using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Resources.Planet
{
	public class QuickAndDirtyCountryResource : ICountryResource
	{
		/// <summary>
		/// Can you read these? Didn't think so. This screams abstraction.
		///
		/// But we are going to prove a quick point first.
		/// </summary>
		private readonly Dictionary<GameInstance, Dictionary<string, Country>> countries;

		private readonly Dictionary<GameInstance, Dictionary<Player, HashSet<Country>>> playerMap;
		private readonly Dictionary<GameInstance, Dictionary<Side, HashSet<Country>>> sideMap;

		public QuickAndDirtyCountryResource()
		{
			countries = new Dictionary<GameInstance, Dictionary<string, Country>>();
			playerMap = new Dictionary<GameInstance, Dictionary<Player, HashSet<Country>>>();
			sideMap = new Dictionary<GameInstance, Dictionary<Side, HashSet<Country>>>();
		}

		public QuickAndDirtyCountryResource
			(
				Bucket<Dictionary<GameInstance, Dictionary<string, Country>>> countries,
				Bucket<Dictionary<GameInstance, Dictionary<Player, HashSet<Country>>>> playerMap,
				Bucket<Dictionary<GameInstance, Dictionary<Side, HashSet<Country>>>> sideMap
			)
		{
			this.countries = countries.Dump();
			this.playerMap = playerMap.Dump();
			this.sideMap = sideMap.Dump();
		}

		public async Task<Country> RetrieveAsync(GameInstance game, string countryId)
		{
			return await Task.Run(() => countries[game][countryId]);
		}

		public async Task<IEnumerable<Country>> RetrieveManyAsync(GameInstance game)
		{
			return await Task.Run(() => countries[game].Select(kvp => kvp.Value));
		}

		public async Task<IEnumerable<Country>> RetrieveManyAsync(GameInstance game, Player player)
		{
			return await Task.Run(() => playerMap[game][player]);
		}

		public async Task<IEnumerable<Country>> RetrieveManyAsync(GameInstance game, Side side)
		{
			return await Task.Run(() => sideMap[game][side]);

		}
	}
}