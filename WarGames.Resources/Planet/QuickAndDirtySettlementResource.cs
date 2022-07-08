using WarGames.Contracts.V2;
using WarGames.Contracts.V2.Sides;
using WarGames.Contracts.V2.World;

namespace WarGames.Resources.Planet
{
	public class QuickAndDirtySettlementResource : ISettlementResource
	{
		private readonly Dictionary<GameSession, Dictionary<Country, HashSet<Settlement>>> countryMap;
		private readonly Dictionary<GameSession, Dictionary<Settlement, Country>> countryReverseMap;
		private readonly Dictionary<GameSession, Dictionary<Player, HashSet<Settlement>>> playerMap;
		private readonly Dictionary<GameSession, Dictionary<Settlement, Player>> playerReverseMap;

		/// <summary>
		/// Can you read these? Didn't think so. This screams abstraction.
		///
		/// But we are going to prove a quick point first.
		/// </summary>
		private readonly Dictionary<GameSession, Dictionary<string, Settlement>> settlements;

		private readonly Dictionary<GameSession, Dictionary<Side, HashSet<Settlement>>> sideMap;
		private readonly Dictionary<GameSession, Dictionary<Settlement, Side>> sideReverseMap;

		public QuickAndDirtySettlementResource()
		{
			settlements = new();
			countryMap = new();
			countryReverseMap = new();
			playerMap = new();
			playerReverseMap = new();
			sideMap = new();
			sideReverseMap = new();
		}

		public QuickAndDirtySettlementResource
			(
				Bucket<Dictionary<GameSession, Dictionary<string, Settlement>>> settlements,
				Bucket<Dictionary<GameSession, Dictionary<Country, HashSet<Settlement>>>> countryMap,
				Bucket<Dictionary<GameSession, Dictionary<Player, HashSet<Settlement>>>> playerMap,
				Bucket<Dictionary<GameSession, Dictionary<Side, HashSet<Settlement>>>> sideMap
			)
		{
			this.settlements = settlements.Dump();
			this.countryMap = countryMap.Dump();
			this.playerMap = playerMap.Dump();
			this.sideMap = sideMap.Dump();

			countryReverseMap = DeriveReverseMaps(this.countryMap);
			playerReverseMap = DeriveReverseMaps(this.playerMap);
			sideReverseMap = DeriveReverseMaps(this.sideMap);
		}

		public async Task AssignAsync(GameSession game, Country country, Settlement settlement)
		{
			await Task.Run(() =>
			{
				InitializeCollection(game, country);
				UnassignCountry(game, settlement);

				countryMap[game][country].Add(settlement);
				countryReverseMap[game].Add(settlement, country);
			});
		}

		public async Task AssignAsync(GameSession game, Player player, Settlement settlement)
		{
			await Task.Run(() =>
			{
				InitializeCollection(game, player);
				UnassignPlayer(game, settlement);

				playerMap[game][player].Add(settlement);
				playerReverseMap[game].Add(settlement, player);
			});
		}

		public async Task AssignAsync(GameSession game, Side side, Settlement settlement)
		{
			await Task.Run(() =>
			{
				InitializeCollection(game, side);
				UnassignSide(game, settlement);

				sideMap[game][side].Add(settlement);
				sideReverseMap[game].Add(settlement, side);
			});
		}

		public async Task<Settlement> RetrieveAsync(GameSession game, string SettlementId)
		{
			return await Task.Run(() => settlements[game][SettlementId]);
		}

		public async Task<IEnumerable<Settlement>> RetrieveManyAsync(GameSession game)
		{
			return await Task.Run(() => settlements[game].Select(kvp => kvp.Value));
		}

		public async Task<IEnumerable<Settlement>> RetrieveManyAsync(GameSession game, Player player)
		{
			return await Task.Run(() => playerMap[game][player]);
		}

		public async Task<IEnumerable<Settlement>> RetrieveManyAsync(GameSession game, Side side)
		{
			return await Task.Run(() => sideMap[game][side]);
		}

		public async Task<IEnumerable<Settlement>> RetrieveManyAsync(GameSession game, Country country)
		{
			return await Task.Run(() => countryMap[game][country]);
		}

		public async Task SaveAsync(GameSession game, Settlement settlement)
		{
			await Task.Run(() =>
			{
				if (!settlements.ContainsKey(game))
					settlements.Add(game, new());

				if (!settlements[game].ContainsKey(settlement.Id))
					settlements[game].Add(settlement.Id, Settlement.Empty);

				settlements[game][settlement.Id] = settlement;
			});
		}

		public async Task SaveManyAsync(GameSession game, IEnumerable<Settlement> settlements)
		{
			foreach (var settlement in settlements)
				await SaveAsync(game, settlement);
		}

		private static Dictionary<GameSession, Dictionary<Settlement, T>> DeriveReverseMaps<T>(Dictionary<GameSession, Dictionary<T, HashSet<Settlement>>> forwardMaps)
		{
			///This method could use some fluency techniques to help understand the code better
			var returnMe = new Dictionary<GameSession, Dictionary<Settlement, T>>();
			foreach (var map in forwardMaps)
			{
				var gameSession = map.Key;
				var subjectSettlementMaps = map.Value;
				returnMe.Add(gameSession, new Dictionary<Settlement, T>());
				foreach (var subjectSettlements in subjectSettlementMaps)
				{
					var subject = subjectSettlements.Key;
					var settlements = subjectSettlements.Value;
					foreach (var settlement in settlements)
						returnMe[gameSession].Add(settlement, subject);
				}
			}
			return returnMe;
		}

		private void InitializeCollection(GameSession game, Country country)
		{
			if (countryMap[game].ContainsKey(country))
				return;

			countryMap[game].Add(country, new HashSet<Settlement>());
		}

		private void InitializeCollection(GameSession game, Player player)
		{
			if (playerMap[game].ContainsKey(player))
				return;

			playerMap[game].Add(player, new HashSet<Settlement>());
		}

		private void InitializeCollection(GameSession game, Side side)
		{
			if (sideMap[game].ContainsKey(side))
				return;

			sideMap[game].Add(side, new HashSet<Settlement>());
		}

		private void UnassignCountry(GameSession game, Settlement settlement)
		{
			if (!countryReverseMap[game].ContainsKey(settlement))
				return;

			var country = countryReverseMap[game][settlement];
			countryReverseMap[game].Remove(settlement);
			countryMap[game][country].Remove(settlement);
		}

		private void UnassignPlayer(GameSession game, Settlement settlement)
		{
			if (!playerReverseMap[game].ContainsKey(settlement))
				return;

			var player = playerReverseMap[game][settlement];
			playerReverseMap[game].Remove(settlement);
			playerMap[game][player].Remove(settlement);
		}

		private void UnassignSide(GameSession game, Settlement settlement)
		{
			if (!sideReverseMap[game].ContainsKey(settlement))
				return;

			var side = sideReverseMap[game][settlement];
			sideReverseMap[game].Remove(settlement);
			sideMap[game][side].Remove(settlement);
		}
	}
}