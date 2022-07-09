using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Resources.Planet
{
	public class QADCountryResource : ICountryResource
	{
		/// <summary>
		/// Can you read these? Didn't think so. This screams abstraction.
		///
		/// But we are going to prove a quick point first.
		/// </summary>
		private readonly Dictionary<GameSession, Dictionary<string, Country>> countries;

		private readonly Dictionary<GameSession, Dictionary<Player, HashSet<Country>>> playerMap;
		private readonly Dictionary<GameSession, Dictionary<Country, Player>> playerReverseMap;
		private readonly Dictionary<GameSession, Dictionary<Side, HashSet<Country>>> sideMap;
		private readonly Dictionary<GameSession, Dictionary<Country, Side>> sideReverseMap;

		public QADCountryResource()
		{
			countries = new();
			playerMap = new();
			playerReverseMap = new();
			sideMap = new();
			sideReverseMap = new();
		}

		public QADCountryResource
			(
				Bucket<Dictionary<GameSession, Dictionary<string, Country>>> countries,
				Bucket<Dictionary<GameSession, Dictionary<Player, HashSet<Country>>>> playerMap,
				Bucket<Dictionary<GameSession, Dictionary<Side, HashSet<Country>>>> sideMap
			)
		{
			this.countries = countries.Dump();
			this.playerMap = playerMap.Dump();
			this.sideMap = sideMap.Dump();

			playerReverseMap = DeriveReverseMaps(this.playerMap);
			sideReverseMap = DeriveReverseMaps(this.sideMap);
		}

		public async Task AssignAsync(GameSession game, Player player, Country country)
		{
			await Task.Run(() =>
			{
				EnforceExistence(game, player);
				UnassignPlayer(game, country);

				playerMap[game][player].Add(country);
				playerReverseMap[game].Add(country, player);
			});
		}

		public async Task AssignAsync(GameSession game, Side side, Country country)
		{
			await Task.Run(() =>
			{
				EnforceExistence(game, side);
				UnassignSide(game, country);

				sideMap[game][side].Add(country);
				sideReverseMap[game].Add(country, side);
			});
		}

		public async Task<Country> RetrieveAsync(GameSession game, string countryId)
		{
			return await Task.Run(() => countries[game][countryId]);
		}

		public async Task<IEnumerable<Country>> RetrieveManyAsync(GameSession game)
		{
			return await Task.Run(() => countries[game].Select(kvp => kvp.Value));
		}

		public async Task<IEnumerable<Country>> RetrieveManyAsync(GameSession game, Player player)
		{
			return await Task.Run(() => playerMap[game][player]);
		}

		public async Task<IEnumerable<Country>> RetrieveManyAsync(GameSession game, Side side)
		{
			return await Task.Run(() => sideMap[game][side]);
		}

		public async Task SaveAsync(GameSession game, Country country)
		{
			await Task.Run(() =>
			{
				EnforceExistence(game);

				if (!countries[game].ContainsKey(country.Id))
					countries[game].Add(country.Id, Country.Empty);

				countries[game][country.Id] = country;
			});
		}

		public async Task SaveManyAsync(GameSession game, IEnumerable<Country> countries)
		{
			foreach (var country in countries)
				await SaveAsync(game, country);
		}

		private static Dictionary<GameSession, Dictionary<Country, T>> DeriveReverseMaps<T>(Dictionary<GameSession, Dictionary<T, HashSet<Country>>> forwardMaps)
		{
			///This method could use some fluency techniques to help understand the code better
			var returnMe = new Dictionary<GameSession, Dictionary<Country, T>>();
			foreach (var map in forwardMaps)
			{
				var gameSession = map.Key;
				var subjectCountryMaps = map.Value;
				returnMe.Add(gameSession, new Dictionary<Country, T>());
				foreach (var subjectCountries in subjectCountryMaps)
				{
					var subject = subjectCountries.Key;
					var countries = subjectCountries.Value;
					foreach (var country in countries)
						returnMe[gameSession].Add(country, subject);
				}
			}
			return returnMe;
		}

		private void EnforceExistence(GameSession game)
		{
			if (!countries.ContainsKey(game))
				countries.Add(game, new());

			if (!playerMap.ContainsKey(game))
				playerMap.Add(game, new());

			if (!playerReverseMap.ContainsKey(game))
				playerReverseMap.Add(game, new());

			if (!sideMap.ContainsKey(game))
				sideMap.Add(game, new());

			if (!sideReverseMap.ContainsKey(game))
				sideReverseMap.Add(game, new());
		}

		private void EnforceExistence(GameSession game, Player player)
		{
			if (playerMap[game].ContainsKey(player))
				return;

			playerMap[game].Add(player, new HashSet<Country>());
		}

		private void EnforceExistence(GameSession game, Side side)
		{
			if (sideMap[game].ContainsKey(side))
				return;

			sideMap[game].Add(side, new HashSet<Country>());
		}

		private void UnassignPlayer(GameSession game, Country country)
		{
			if (!playerReverseMap[game].ContainsKey(country))
				return;

			var player = playerReverseMap[game][country];
			playerReverseMap[game].Remove(country);
			playerMap[game][player].Remove(country);
		}

		private void UnassignSide(GameSession game, Country country)
		{
			if (!sideReverseMap[game].ContainsKey(country))
				return;

			var side = sideReverseMap[game][country];
			sideReverseMap[game].Remove(country);
			sideMap[game][side].Remove(country);
		}
	}
}